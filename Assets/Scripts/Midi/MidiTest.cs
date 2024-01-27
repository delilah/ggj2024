using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiTest : MonoBehaviour
{
    public List<TimedNote> notes;

    void Start()
    {
        // songtest needs to be a midi file with the .midi replaced to .bytes extension
        notes = MidiReader.GetNotesList("songtest");
    }
}
