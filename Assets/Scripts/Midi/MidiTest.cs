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

       // foreach (var note in notes)
       // {
      //      Debug.Log("Note time: " + note.TimeInSeconds);
        //}
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        SpawnNotes();
        MoveAndClearNotes();
    }

void SpawnNotes()
{
    const float speed = 8f; // Set a constant speed
    var spawnDistance = _spawnNotesXPos.transform.position.x;
    
    while (_notesQueue.Count > 0)
    {
        var currentNote = _notesQueue.Peek();
        var spawnTime = currentNote.TimeInSeconds - spawnDistance / speed;
        
        // If it's not yet time to spawn this note, break the loop
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

        //DebugZero(spawnedNote, movement);

        // Calculate new position
        var potentialNewPos = spawnedNote.transform.position + movement;

        // Calculate position in relation to the clearPrefabsPoint and evaluate cleanup
        if (potentialNewPos.x < _clearPrefabsPoint.transform.position.x)
        {
            _speedMultipliers.Remove(spawnedNote);
            Destroy(spawnedNote);
            _spawnedNotes.RemoveAt(i);
        }
        else
        {
            // Keep moving until reaches the ClearPrefabsPoint
            spawnedNote.transform.Translate(movement);
        }
    }
}

void DebugZero(GameObject spawnedNote, Vector3 movement)
{
    // Debug to check the value of the prefab when it reaches the middle/hot area
    if (spawnedNote.transform.position.x >= 0 && spawnedNote.transform.position.x + movement.x < 0)
    {
        Debug.Log($"Prefab {spawnedNote.name} crosses x=0 at time {_currentTime} ");
    }
}

public GameObject GetNoteOnTheBeat()
{
    foreach (var note in _spawnedNotes)
    {
        // Instead of getting the exact note on 0, I look for a grace range around that position
        if (note.transform.position.x >= -1f && note.transform.position.x <= 1f)
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