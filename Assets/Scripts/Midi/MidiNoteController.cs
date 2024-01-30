using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MidiNoteController : MonoBehaviour
{

    public enum NoteName
    {
        LeftPawA    = 36,
        LeftPawB    = 37,
        RightPawA   = 38,
        RightPawB   = 39
    }

    [System.Serializable]
    public class PawTrack
    {
        public Transform Spawn;
        public Transform Hit;
        public float GraceArea = 1f;

        public GameObject Nope;
        public float NopeHideTime { get; set; }

        public GameObject Yes;
        public float YesHideTime { get; set; }

        public void Awake()
        {
            Nope.SetActive(false);
            Yes.SetActive(false);
        }

        public void ShowNope()
        {
            Nope.SetActive(true);
            NopeHideTime = 0.2f;
        }

        public void ShowYes()
        {
            Yes.SetActive(true);
            YesHideTime = 0.2f;
        }

        public void Update()
        {
            NopeHideTime -= Time.deltaTime;
            if (NopeHideTime <= 0f) Nope.SetActive(false);

            YesHideTime -= Time.deltaTime;
            if (YesHideTime <= 0f) Yes.SetActive(false);
        }
    }

    public List<TimedNote> _notes;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private PawTrack _leftPawTrack;
    [SerializeField] private PawTrack _rightPawTrack;
    [SerializeField] private SpawnedNote _notePrefab;

    private Queue<TimedNote> _notesQueue;
    private List<SpawnedNote> _spawnedNotesInUse = new List<SpawnedNote>();
    private List<SpawnedNote> _spawnedNotesTooLate = new List<SpawnedNote>();
    private float _currentTime;

    private UnityEngine.Pool.ObjectPool<SpawnedNote> _spawnedNotesPool;

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_leftPawTrack.Hit.position, new Vector3(1f, _leftPawTrack.GraceArea * 2f, 1f));

        Gizmos.DrawCube(_rightPawTrack.Hit.position, new Vector3(1f, _rightPawTrack.GraceArea * 2f, 1f));
    }

    private void Awake()
    {
        _notes = MidiReader.GetNotesList("songtest");
        _notesQueue = new Queue<TimedNote>(_notes.Where(note => note.Note == 36 || note.Note == 38));
        _spawnedNotesPool = new UnityEngine.Pool.ObjectPool<SpawnedNote>(OnSpawnNote, OnGetNote, OnReleaseNote);

        _leftPawTrack.Awake();
        _rightPawTrack.Awake();
    }

    private void Update()
    {
        SpawnNotes();
        CheckInput();
        UpdateNopes();
    }

    private void UpdateNopes()
    {
        _leftPawTrack.Update();
        _rightPawTrack.Update();
    }

    private void FixedUpdate()
    {
        MoveNotes();
    }

    private void CheckInput()
    {
        var isPawLeftDown = _playerInput.GetLeftPawDown();
        var isPawRightDown = _playerInput.GetRightPawDown();

        if (!isPawLeftDown && !isPawRightDown) return;

        bool hasNoteOnLeft = false;
        bool hasNoteOnRight = false;

        for (int i = _spawnedNotesInUse.Count - 1; i >= 0; i--)
        {
            SpawnedNote spawnedNote = _spawnedNotesInUse[i];
            PawTrack pawTrack = null;
            bool isLeftPawNote = false;
            switch ((NoteName)spawnedNote.TimedNote.Note)
            {
                case NoteName.LeftPawA:
                case NoteName.LeftPawB:
                    pawTrack = _leftPawTrack;
                    isLeftPawNote = true;
                    break;
                case NoteName.RightPawA:
                case NoteName.RightPawB:
                    pawTrack = _rightPawTrack;
                    isLeftPawNote = false;
                    break;
            }

            bool isNoteInRangeOfHit = IsInRange(
                spawnedNote.transform.position.y,
                pawTrack.Hit.position.y - pawTrack.GraceArea,
                pawTrack.Hit.position.y + pawTrack.GraceArea);

            if (!isNoteInRangeOfHit) continue;
            else
            {
                hasNoteOnLeft |= isLeftPawNote; 
                hasNoteOnRight |= !isLeftPawNote;
            }

            if (isLeftPawNote && isPawLeftDown)
            {
                Debug.Log($"LEFT PAW HIT: {spawnedNote.TimedNote.Note}");

                _spawnedNotesInUse.RemoveAt(i);
                _spawnedNotesPool.Release(spawnedNote);

                _leftPawTrack.ShowYes();
            }

            if (!isLeftPawNote && isPawRightDown) 
            {
                Debug.Log($"RIGHT PAW HIT: {spawnedNote.TimedNote.Note}");

                _spawnedNotesInUse.RemoveAt(i);
                _spawnedNotesPool.Release(spawnedNote);

                _rightPawTrack.ShowYes();
            }
        }


        if (!hasNoteOnLeft && isPawLeftDown)
        {
            Debug.Log($"LEFT PAW MISS!");
            _leftPawTrack.ShowNope();
        }

        if(!hasNoteOnRight && isPawRightDown)
        {
            Debug.Log($"RIGHT PAW MISS!");
            _rightPawTrack.ShowNope();
        }
    }

    bool IsInRange(float position, float min, float max)
    {
        return position >= min && position <= max;
    }

    private void MoveNotes()
    {
        var movement = Vector3.down * _speed * Time.deltaTime;

        for (int i = _spawnedNotesTooLate.Count - 1; i >= 0; i--)
        {
            SpawnedNote spawnedNote = _spawnedNotesTooLate[i];

            var potentialNewPos = spawnedNote.transform.position + movement;
            PawTrack pawTrack = null;

            switch ((NoteName)spawnedNote.TimedNote.Note)
            {
                case NoteName.LeftPawA:
                case NoteName.LeftPawB:
                    pawTrack = _leftPawTrack;
                    break;
                case NoteName.RightPawA:
                case NoteName.RightPawB:
                    pawTrack = _rightPawTrack;
                    break;
            }

            if (potentialNewPos.y < pawTrack.Hit.position.y - pawTrack.GraceArea * 2f)
            {
                Debug.Log($"Remove note: {spawnedNote.TimedNote.Note}");

                _spawnedNotesTooLate.RemoveAt(i);
                _spawnedNotesPool.Release(spawnedNote);
            }

            spawnedNote.transform.Translate(movement);
        }


        for (int i = _spawnedNotesInUse.Count - 1; i >= 0; i--)
        {
            SpawnedNote spawnedNote = _spawnedNotesInUse[i];

            var potentialNewPos = spawnedNote.transform.position + movement;
            PawTrack pawTrack = null;

            switch ((NoteName)spawnedNote.TimedNote.Note)
            {
                case NoteName.LeftPawA:
                case NoteName.LeftPawB:
                    pawTrack = _leftPawTrack;
                    break;
                case NoteName.RightPawA:
                case NoteName.RightPawB:
                    pawTrack = _rightPawTrack;
                    break;
            }

            bool isNoteInRangeOfHit = IsInRange(
                spawnedNote.transform.position.y,
                pawTrack.Hit.position.y - pawTrack.GraceArea,
                pawTrack.Hit.position.y + pawTrack.GraceArea);

            spawnedNote.SetInRange(isNoteInRangeOfHit);

            if (potentialNewPos.y < pawTrack.Hit.position.y - pawTrack.GraceArea)
            {
                Debug.Log($"Missed note: {spawnedNote.TimedNote.Note}");

                spawnedNote.SetTooLate();
                
                _spawnedNotesInUse.RemoveAt(i);
                _spawnedNotesTooLate.Add(spawnedNote);
                
            }

            spawnedNote.transform.Translate(movement);
        }
    }

    void SpawnNotes()
    {
        while (_notesQueue.Count > 0)
        {
            var currentNote = _notesQueue.Peek();

            var spawnDistance = 0f;
            PawTrack pawTrack = null;

            switch ((NoteName)currentNote.Note)
            {
                case NoteName.LeftPawA:
                case NoteName.LeftPawB:
                    spawnDistance = Mathf.Abs(_leftPawTrack.Spawn.position.y - _leftPawTrack.Hit.position.y);
                    pawTrack = _leftPawTrack;
                    break;
                case NoteName.RightPawA:
                case NoteName.RightPawB:
                    spawnDistance = Mathf.Abs(_rightPawTrack.Spawn.position.y - _rightPawTrack.Hit.position.y);
                    pawTrack = _rightPawTrack;
                    break;
            }


            var spawnTime = currentNote.TimeInSeconds - spawnDistance / _speed;

            if (spawnTime > _currentTime) break;

            var queuedNote = _notesQueue.Dequeue();

            //Debug.Log($"Spawned: {queuedNote.Note} :: {queuedNote.TimeInSeconds}");

            var spawnedNote = _spawnedNotesPool.Get();
            spawnedNote.transform.position = pawTrack.Spawn.position;
            spawnedNote.TimedNote = queuedNote;

            _spawnedNotesInUse.Add(spawnedNote);
        }
        _currentTime += Time.deltaTime;
    }

    private SpawnedNote OnSpawnNote()
    {
        return Instantiate(_notePrefab, Vector3.zero, Quaternion.identity) as SpawnedNote;
    }

    private void OnReleaseNote(SpawnedNote note)
    {
        note.gameObject.SetActive(false);
    }

    private void OnGetNote(SpawnedNote note)
    {
        note.gameObject.SetActive(true);
    }
}
