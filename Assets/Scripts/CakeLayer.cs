using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeLayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    //Use it if needed

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Splatter");
    }
}
