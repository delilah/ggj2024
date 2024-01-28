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
    [SerializeField] private LayerMask noteLayer;
    [SerializeField] private float _graceRange = 2f;

    void Update()
    {
        GameObject noteInBeatRangeLeft = MidiTest.Instance.GetNoteOnTheBeatLeft(_graceRange);
        GameObject noteInBeatRangeRight = MidiTest.Instance.GetNoteOnTheBeatRight(_graceRange);

        if (pawType == PawType.Left && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a")))
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
        else if (pawType == PawType.Right && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d")))
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

        if (!hitDetected && Physics.CheckSphere(transform.position, colliderRadius, noteLayer))
        {
            hitDetected = true;
            Debug.Log($"Hit {pawType}");
            GameMessages.RequestGoodLayer();
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
        this.GetComponent<Rigidbody>().MovePosition(_target.position);
        // Deactivate Object I collided with for visual cue
        note.SetActive(false);

        hitDetected = true;
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