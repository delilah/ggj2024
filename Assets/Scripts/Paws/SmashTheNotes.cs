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
    [SerializeField] private float _offsetLeft = 1.33f;
    [SerializeField] private float _offsetRight = 2.0f;
    [SerializeField] private float colliderRadius;
    [SerializeField] private LayerMask noteLayer;

    // Actions
    public static event Action GoodLayer;
    public static event Action BadLayer;

    void Start()
    {
        initialPosition = this.transform.position;
    }

    void Update()
    {
        if (pawType == PawType.Left && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a")))
        {
            Move();
            hasMoved = true;
        }
        else if (pawType == PawType.Right && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d")))
        {
            Move();
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

    private void Move()
    {
        var offset = pawType == PawType.Left ? _offsetLeft : _offsetRight;
        var newPosition = new Vector3(this.transform.position.x, offset, -1.0f);
        this.GetComponent<Rigidbody>().MovePosition(newPosition);
        actionTimer = 0.1f; // reset timer
    }

    private void ReturnToInitialPosition()
    {
        this.transform.position = initialPosition;
        actionTimer = 0.1f; // reset timer
        hitDetected = false; // reset hit detection
        hasMoved = false;
    }
}