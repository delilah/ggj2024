using UnityEngine;
using System.Collections.Generic;
using System.IO;
using NAudio.Midi;

[System.Serializable]
public struct TimedNote
{
	public float TimeInSeconds;
	public int Note;
}

public class MidiReader
{
	public static List<TimedNote> GetNotesList(string trackAssetName, int eventsTrack = 1, int tempoTrack = 0, int tempoEvent = 3)
	{
		List<TimedNote> notes = new List<TimedNote>();

		TextAsset asset = Resources.Load(trackAssetName) as TextAsset;
		Stream stream = new MemoryStream(asset.bytes);
		MidiFile midi = new MidiFile(stream, true);

		int ticks = midi.DeltaTicksPerQuarterNote;
		var tempo = midi.Events[tempoTrack][tempoEvent] as TempoEvent;
		float bpm = (float)tempo.Tempo;

		foreach (MidiEvent note in midi.Events[eventsTrack])
		{
			if (note.CommandCode == MidiCommandCode.NoteOn)
			{
				NoteOnEvent noe = (NoteOnEvent)note;
				AddNote(noe, bpm);
			}
		}

		void AddNote(NoteOnEvent noe, float bpm)
		{
			float time = (60f * noe.AbsoluteTime) / (bpm * ticks);
			int noteNumber = noe.NoteNumber;

            var note = new TimedNote
            {
                TimeInSeconds = time,
                Note = noteNumber
            };
            notes.Add(note);
		}

		return notes;
	}
}