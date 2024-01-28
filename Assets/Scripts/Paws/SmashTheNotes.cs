using UnityEngine;

public class SmashTheNotes : MonoBehaviour
{
    private Vector3 initialPosition;
    private float actionTimer = 0.1f;
    private bool hasMoved = false;
    private bool hitDetected = false;

    [SerializeField] private float _offsetLeft; 
    [SerializeField] private float _offsetRight; 

    [SerializeField] private float colliderRadius;
    [SerializeField] private LayerMask noteLayer;

    void Start () 
    {
        initialPosition = this.transform.position;
        _offsetLeft = 1.33f;
        _offsetRight = 2.0f;
    }

    void Update()
    {

          if (gameObject.name == "PawLeft")
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            Move();
            hasMoved = true;
        }
        } 

        if (gameObject.name == "PawRight")
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
        {
            Move();
            hasMoved = true;
        }
        }

        

        // decrease timer
        if (actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
        }
        else
        {
            if (hasMoved && !Physics.CheckSphere(transform.position, colliderRadius, noteLayer))
            {
                if (gameObject.name == "PawLeft")
                {
                    Debug.Log("Miss PawLeft");
                }
                else if (gameObject.name == "PawRight")
                {
                    Debug.Log("Miss PawRight");
                }
            }
            ReturnToInitialPosition();
            hasMoved = false;
        }

        if (!hitDetected && Physics.CheckSphere(transform.position, colliderRadius, noteLayer))
        {
            hitDetected = true;
            if (gameObject.name == "PawLeft")
            {
                Debug.Log("Hit PawLeft");
            }
            else if (gameObject.name == "PawRight")
            {
                Debug.Log("Hit PawRight");
            }
            ReturnToInitialPosition();
            hasMoved = false;
        }
    }
    
    private void Move()
    {
        var newPosition = new Vector3(this.transform.position.x, this.transform.position.y, -1.0f);
        
        if (gameObject.name == "PawLeft")
        {
            newPosition = new Vector3(this.transform.position.x, _offsetLeft, -1.0f);
        } 
        else 
        {
            newPosition = new Vector3(this.transform.position.x, _offsetRight, -1.0f);
        }
        this.GetComponent<Rigidbody>().MovePosition(newPosition);
        actionTimer = 0.1f; // reset timer
    }
    
    private void ReturnToInitialPosition()
    {
        this.transform.position = initialPosition;
        actionTimer = 0.1f; // reset timer
        hitDetected = false; // reset hit detection
    }
}