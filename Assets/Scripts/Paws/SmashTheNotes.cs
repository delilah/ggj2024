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

    [SerializeField] private PawType pawType;
    private float _offsetLeft = 3f;
    private float _offsetRight = 1.2f;
    [SerializeField] private float colliderRadius;
    [SerializeField] private LayerMask noteLayer;
    [SerializeField] private float _graceRange = 2f;

    // Actions
    public static event Action GoodLayer;
    public static event Action BadLayer;

    void Start()
    {
        initialPosition = this.transform.position;
    }

    void Update()
    {
        GameObject noteInBeatRange = (pawType == PawType.Left) ? MidiTest.Instance.GetNoteOnTheBeatLeft(_graceRange) : MidiTest.Instance.GetNoteOnTheBeatRight(_graceRange);
        bool isKeyPressed = (pawType == PawType.Left) ? (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a")) : (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"));
        string noteToCheck = (pawType == PawType.Left) ? "noteA" : "noteC";
        string noteToCheck2 = (pawType == PawType.Left) ? "noteB" : "noteD";

        if (isKeyPressed)
        {
            if (noteInBeatRange != null)
            {
                var noteType = noteInBeatRange.name;
                if (noteType.Contains(noteToCheck) || noteType.Contains(noteToCheck2))
                {
                    MoveAndReturn();
                }
            }
            else
            {
                MoveAndReturn();
            }

            hasMoved = true;
        }

        if (actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
        }
        else
        {
            if (hasMoved && !Physics.CheckSphere(transform.position, colliderRadius, noteLayer))
            {
                Debug.Log($"Miss {pawType}");
                BadLayer?.Invoke();
                ReturnToInitialPosition();
            }
        }

        if (!hitDetected && Physics.CheckSphere(transform.position, colliderRadius, noteLayer))
        {
            hitDetected = true;
            Debug.Log($"Hit {pawType}");
            GoodLayer?.Invoke();
            ReturnToInitialPosition();
        }
    }

    private void MoveAndReturn()
    {
        var offset = pawType == PawType.Left ? _offsetLeft : _offsetRight;
        var newPosition = new Vector3(this.transform.position.x, offset, -1.0f);
        this.GetComponent<Rigidbody>().MovePosition(newPosition);
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