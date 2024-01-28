using System;
using UnityEngine;

public enum PawType
{
    Left,
    Right
}

public class SmashTheNotes : MonoBehaviour
{
    private float actionTimer = 0.1f;
    [SerializeField] private float _resetTimerTime = 0f;
    private bool hasMoved = false;
    private bool hitDetected = false;

    public Transform _origin;
    public Transform _target;

    [SerializeField] private PawType pawType;
    [SerializeField] private float colliderRadius;
    [SerializeField] private LayerMask missLeftLayer;
    [SerializeField] private LayerMask missRightLayer;
    [SerializeField] private float _graceRange = 2f;

    
    private int _successCounter = 0;
    public int NUMBER_OF_SUCCESS = 15; //const,public for tweaking

    void Update()
    {
        GameObject noteInBeatRangeLeft = MidiTest.Instance.GetNoteOnTheBeatLeft(_graceRange);
        GameObject noteInBeatRangeRight = MidiTest.Instance.GetNoteOnTheBeatRight(_graceRange);

        if (pawType == PawType.Left && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)))
        {
            if (noteInBeatRangeLeft != null)
            {
                var noteType = noteInBeatRangeLeft.name;

                if (noteType.Contains("noteA") || noteType.Contains("noteB"))
                {
                    MoveToNoteAndReturn(noteInBeatRangeLeft);
                }
                else
                {
                    Move();
                }
            }
            else
            {
                Move();
            }

            hasMoved = true;
        }
        else if (pawType == PawType.Right && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.L)))
        {
            if (noteInBeatRangeRight != null)
            {
                var noteType = noteInBeatRangeRight.name;

                if (noteType.Contains("noteC") || noteType.Contains("noteD"))
                {
                    MoveToNoteAndReturn(noteInBeatRangeRight);
                }
                else
                {
                    Move();
                }
            }
            else
            {
                Move();
            }

            hasMoved = true;
        }

        actionTimer -= Time.deltaTime;

        if (!hitDetected && hasMoved && actionTimer <= 0)
        {
            ReturnToInitialPosition();
        }
    }

    private void Move()
    {
        this.GetComponent<Rigidbody>().MovePosition(_target.position);
        actionTimer = _resetTimerTime; // reset timer
    }

    private void MoveToNoteAndReturn(GameObject note)
{
    // Workaround: we already know the notes are in our range when we move the paw
        hitDetected = true;
        //Debug.Log($"Hit {pawType}");

         _successCounter++;

        if (_successCounter % NUMBER_OF_SUCCESS == 0)
        {
           GameMessages.RequestGoodLayer();
        }

        // Deactivate Object I collided with for visual cue
        note.SetActive(false);

    // Move the paw to the target position
    this.GetComponent<Rigidbody>().MovePosition(_target.position);

    ReturnToInitialPosition();
}

    private void ReturnToInitialPosition()
    {
        this.transform.position = _origin.position;
        actionTimer = _resetTimerTime; // reset timer
        hitDetected = false; // reset hit detection
        hasMoved = false;
    }
}
