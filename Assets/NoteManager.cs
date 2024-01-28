using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }

    public List<GameObject> ActiveNotesAandC { get; } = new List<GameObject>();
    public List<GameObject> ActiveNotesBandD { get; } = new List<GameObject>();

    [SerializeField] private GameObject[] notePrefabsAC; // NoteA and NoteC prefabs
    [SerializeField] private GameObject[] notePrefabsBD; // NoteB and NoteD prefabs

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject CreateNoteAC(int noteTypeIndex)
    {
        GameObject note = Instantiate(notePrefabsAC[noteTypeIndex]);
        ActiveNotesAandC.Add(note);
        return note;
    }

    public GameObject CreateNoteBD(int noteTypeIndex)
    {
        GameObject note = Instantiate(notePrefabsBD[noteTypeIndex]);
        ActiveNotesBandD.Add(note);
        return note;
    }

    public void DestroyNoteAC(GameObject note)
    {
        ActiveNotesAandC.Remove(note);
        Destroy(note);
    }

    public void DestroyNoteBD(GameObject note)
    {
        ActiveNotesBandD.Remove(note);
        Destroy(note);
    }
}