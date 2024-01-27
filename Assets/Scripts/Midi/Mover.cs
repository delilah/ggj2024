using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 1f;
    void Update()
    {
        Vector3 movement = new Vector3(-speed * Time.deltaTime, 0f, 0f);
        transform.Translate(movement);
    }
}