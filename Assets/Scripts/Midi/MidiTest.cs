using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiTest : MonoBehaviour
{
    public List<TimedNote> notes;
    private Queue<TimedNote> _notesQueue;
    private List<GameObject> _spawnedNotes = new List<GameObject>();

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
    public float speedMultiplier = 1f;

    void Start()
    {
        notes = MidiReader.GetNotesList("songtest");
        _notesQueue = new Queue<TimedNote>(notes);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;

        while (_notesQueue.Count > 0 && _notesQueue.Peek().TimeInSeconds <= _currentTime)
        {
            TimedNote currentNote = _notesQueue.Dequeue();
            GameObject noteSpawned = SpawnNotePrefab(currentNote);
            _spawnedNotes.Add(noteSpawned);
        }


        for (int i = 0; i < _spawnedNotes.Count; i++)
        {
            var spawnedNote = _spawnedNotes[i];
            Vector3 movement = new Vector3(-speedMultiplier * Time.deltaTime, 0, 0);
            spawnedNote.transform.Translate(movement);
        }

        for (int i = _spawnedNotes.Count - 1; i >= 0; i--)
        {
            var spawnedNote = _spawnedNotes[i];
            if (spawnedNote.transform.position.x < _clearPrefabsPoint.transform.position.x)
            {
                Destroy(spawnedNote);
                _spawnedNotes.RemoveAt(i);
            }
        }
    }

    GameObject SpawnNotePrefab(TimedNote currentNote)
    {
        GameObject noteToSpawn = null;
        float heightAdjustment = 0f;

        switch (currentNote.Note)
        {
            case 36:
                noteToSpawn = NoteA;
                heightAdjustment = _lineA.position.y;
                break;
            case 37:
                noteToSpawn = NoteB;
                heightAdjustment = _lineB.position.y;
                break;
            case 38:
                noteToSpawn = NoteC;
                heightAdjustment = _lineC.position.y;
                break;
            case 39:
                noteToSpawn = NoteD;
                heightAdjustment = _lineD.position.y;
                break;
            default:
                Debug.LogError($"Unexpected note number: {currentNote.Note}");
                return null;
        }

        if (noteToSpawn != null)
        {
            Vector3 adjustedPosition = new Vector3(_spawnNotesXPos.transform.position.x, heightAdjustment, _spawnNotesXPos.transform.position.z);
            return Instantiate(noteToSpawn, adjustedPosition, Quaternion.identity);
        }
        else
        {
            return null;
        }
    }
}