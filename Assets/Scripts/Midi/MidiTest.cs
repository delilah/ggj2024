using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiTest : MonoBehaviour
{
    public static MidiTest Instance { get; private set; }
    public List<TimedNote> notes;
    private Queue<TimedNote> _notesQueue;
    private List<GameObject> _spawnedNotes = new List<GameObject>();
    private Dictionary<GameObject, float> _speedMultipliers = new Dictionary<GameObject, float>();

    public GameObject NoteA;
    public GameObject NoteB;
    public GameObject NoteC;
    public GameObject NoteD;

    [SerializeField] private Transform _lineA;
    [SerializeField] private Transform _lineB;
    [SerializeField] private Transform _lineC;
    [SerializeField] private Transform _lineD;

    [SerializeField] private GameObject _spawnNotesXPos;
    [SerializeField] private GameObject _clearPrefabsPoint;
    [SerializeField] private GameObject _hitSpotLeft;
    [SerializeField] private GameObject _hitSpotRight;

    private float _currentTime = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            Instance = this;
        }
    }

    void Start()
    {
        notes = MidiReader.GetNotesList("songtest");
        _notesQueue = new Queue<TimedNote>(notes);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        SpawnNotes();
        MoveAndClearNotes();
    }

    void SpawnNotes()
    {
        const float speed = 8f;
        var spawnDistance = _spawnNotesXPos.transform.position.x;
        
        while (_notesQueue.Count > 0)
        {
            var currentNote = _notesQueue.Peek();

            if (currentNote.Note == 36 || currentNote.Note == 37)
            {
                spawnDistance = Mathf.Abs(_hitSpotLeft.transform.position.x - _spawnNotesXPos.transform.position.x);
            }
            else if (currentNote.Note == 38 || currentNote.Note == 39)
            {
                spawnDistance = Mathf.Abs(_hitSpotRight.transform.position.x - _spawnNotesXPos.transform.position.x);
            }
            
            var spawnTime = currentNote.TimeInSeconds - spawnDistance / speed;
            
            if (spawnTime > _currentTime) break;

            var spawnedNote = SpawnNotePrefab(currentNote);
            _notesQueue.Dequeue();

            _speedMultipliers[spawnedNote] = speed;
            _spawnedNotes.Add(spawnedNote);
        }
    }

    void MoveAndClearNotes()
    {
        for (int i = _spawnedNotes.Count - 1; i >= 0; i--)
        {
            var spawnedNote = _spawnedNotes[i];
            var speed = _speedMultipliers[spawnedNote];
            var movement = new Vector3(-speed * Time.deltaTime, 0, 0);

            var potentialNewPos = spawnedNote.transform.position + movement;
            
            if (potentialNewPos.x < _clearPrefabsPoint.transform.position.x)
            {
                _speedMultipliers.Remove(spawnedNote);
                Destroy(spawnedNote);
                _spawnedNotes.RemoveAt(i);
            }
            else
            {
                spawnedNote.transform.Translate(movement);
            }
        }
    }

public GameObject GetNoteOnTheBeatLeft(float range)
{
    float leftBound = _hitSpotLeft.transform.position.x - range;
    float rightBound = _hitSpotLeft.transform.position.x + range;

    foreach (var note in _spawnedNotes)
    {
        if (note.transform.position.x >= leftBound && note.transform.position.x <= rightBound)
        {
            return note;
        }
    }

    return null;
}

public GameObject GetNoteOnTheBeatRight(float range)
{
    float leftBound = _hitSpotRight.transform.position.x - range;
    float rightBound = _hitSpotRight.transform.position.x + range;

    foreach (var note in _spawnedNotes)
    {
        if (note.transform.position.x >= leftBound && note.transform.position.x <= rightBound)
        {
            return note;
        }
    }

    return null;
}

    GameObject SpawnNotePrefab(TimedNote currentNote)
    {
        var noteToSpawn = GetNoteToSpawn(currentNote.Note);
        if (noteToSpawn == null) return null;
        
        var heightAdjustment = GetHeightAdjustment(currentNote.Note);
        var adjustedPosition = new Vector3(_spawnNotesXPos.transform.position.x, heightAdjustment, _spawnNotesXPos.transform.position.z);
        return Instantiate(noteToSpawn, adjustedPosition, Quaternion.identity);
    }

    GameObject GetNoteToSpawn(int note)
    {
        switch (note)
        {
            case 36:
                return NoteA;
            case 37:
                return NoteB;
            case 38:
                return NoteC;
            case 39:
                return NoteD;
            default:
                Debug.LogError($"Unexpected note number: {note}");
                return null;
        }
    }

   float GetHeightAdjustment(int note)
    {
        switch (note)
        {
            case 36:
                return _lineA.position.y;
            case 37:
                return _lineB.position.y;
            case 38:
                return _lineC.position.y;
            case 39:
                return _lineD.position.y;
            default:
                Debug.LogError($"Unexpected note number: {note}");
                return 0;
        }
    }
}