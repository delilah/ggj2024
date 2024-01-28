using System;
using UnityEngine;

public enum PawType
{
    Left,
    Right
}

public class SmashTheNotes : MonoBehaviour
{
    private Vector3 initialPosition;
    private float actionTimer = 0.1f;
    private bool hasMoved = false;
    private bool hitDetected = false;

    public Transform _origin;
    public Transform _target;


    [SerializeField] private PawType pawType;
    [SerializeField] private float _offsetLeft = 2f;
    [SerializeField] private float _offsetRight = 1f;
    [SerializeField] private float colliderRadius;
    [SerializeField] private LayerMask noteLayer;
    [SerializeField] private float _graceRange = 2f;


    void Start()
    {
        initialPosition = this.transform.position;
    }

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
            //GameMessages.RequestGoodLayer();
            ReturnToInitialPosition();
        }
    }

    private void Move()
    {
        this.GetComponent<Rigidbody>().MovePosition(_target.position);
        actionTimer = 0.1f; // reset timer
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
        this.transform.position = initialPosition;
        actionTimer = 0.1f; // reset timer
        hitDetected = false; // reset hit detection
        hasMoved = false;
    }
}