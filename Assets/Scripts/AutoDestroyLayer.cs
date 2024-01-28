using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyLayer : MonoBehaviour
{
    [SerializeField] private GameObject _clearPoint;
    
    void Update()
    {
       
        if (transform.position.y < _clearPoint.transform.position.y)
        {
            Destroy(gameObject);
        }
    }
}
