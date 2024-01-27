using UnityEngine;

public class SmashTheNotes : MonoBehaviour
{
    private Vector3 initialPosition;
    private float actionTimer = 0.1f;

    [SerializeField] private float _offsetLeft; 

    void Start () 
    {
        initialPosition = this.transform.position;
        // init
        _offsetLeft = -.33f;
    }

    void Update()
    {
        // move the object when left arrow or "A" is pressed
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            Move();
        }

        // decrease timer
        if (actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
        }
        else
        {
            ReturnToInitialPosition();
        }
    }
    
    private void Move()
    {
        this.transform.position = new Vector3(this.transform.position.x, _offsetLeft, -1.0f);
        actionTimer = 0.1f; // reset timer
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "noteA" || collision.gameObject.name == "noteB" || 
            collision.gameObject.name == "noteC" || collision.gameObject.name == "noteD")
        {
            Debug.Log("Hit");
        }
        else
        {
            Debug.Log("Miss");
        }
    }
    
    private void ReturnToInitialPosition()
    {
        this.transform.position = initialPosition;
        actionTimer = 0.1f; // reset timer
    }
}