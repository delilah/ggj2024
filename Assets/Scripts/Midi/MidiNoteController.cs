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
    }

    public List<TimedNote> _notes;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private PawTrack _leftPawTrack;
    [SerializeField] private PawTrack _rightPawTrack;
    [SerializeField] private SpawnedNote _notePrefab;

    private Queue<TimedNote> _notesQueue;
    private List<SpawnedNote> _spawnedNotes = new List<SpawnedNote>();
    private float _currentTime;

    private void Awake()
    {
        _notesQueue = new Queue<TimedNote>(_notes);
    }

    bool _isStable;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;
        _isStable = true;
    }

    private void Update()
    {
        if (!_isStable) return;
        SpawnNotes();
    }

    private void FixedUpdate()
    {
        MoveNotes();
    }

    private void MoveNotes()
    {
        foreach(var spawnedNote in _spawnedNotes)
        {
            var movement = Vector3.down * _speed * Time.deltaTime;
            var rb = spawnedNote.GetComponent<Rigidbody>();
            var potentialNewPos = rb.position + movement;
            rb.MovePosition(potentialNewPos);
        }

        //for (int i = _spawnedNotes.Count - 1; i >= 0; i--)
        //{
        //    SpawnedNote spawnedNote = _spawnedNotes[i];
        //    var movement = Vector3.down * _speed * Time.deltaTime;

        //    var potentialNewPos = spawnedNote.transform.position + movement;

        //    spawnedNote.GetComponent<Rigidbody>().MovePosition(potentialNewPos);
        //    return;

        //    if (potentialNewPos.y < _hitPosition.position.y)
        //    {
        //        Debug.Log($"Missed note: {spawnedNote.TimedNote.Note}");

        //        _spawnedNotes.RemoveAt(i);
        //        Destroy(spawnedNote.gameObject);
        //    }
        //    else
        //    {
        //        spawnedNote.transform.Translate(movement);
        //    }
        //}
    }

    void SpawnNotes()
    {
        while (_notesQueue.Count > 0)
        {
            var currentNote = _notesQueue.Peek();

            var spawnDistance = 0f;
            Transform spawner = null;

            switch ((NoteName)currentNote.Note)
            {
                case NoteName.LeftPawA:
                case NoteName.LeftPawB:
                    spawnDistance = Mathf.Abs(_leftPawTrack.Spawn.position.y - _leftPawTrack.Hit.position.y);
                    spawner = _leftPawTrack.Spawn;
                    break;
                case NoteName.RightPawA:
                case NoteName.RightPawB:
                    spawnDistance = Mathf.Abs(_rightPawTrack.Spawn.position.y - _rightPawTrack.Hit.position.y);
                    spawner = _rightPawTrack.Spawn;
                    break;
            }


            var spawnTime = currentNote.TimeInSeconds - spawnDistance / _speed;

            if (spawnTime > _currentTime) break;

            var queuedNote = _notesQueue.Dequeue();

            Debug.Log($"Spawned: {queuedNote.Note} :: {queuedNote.TimeInSeconds}");

            var spawnedNote = Instantiate(_notePrefab, spawner.position, Quaternion.identity) as SpawnedNote;
            spawnedNote.TimedNote = queuedNote;

            _spawnedNotes.Add(spawnedNote);
        }
        _currentTime += Time.deltaTime;
    }
}
