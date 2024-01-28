using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighestCake : MonoBehaviour
{
    [SerializeField] private Transform _highestCakeTransform;
    [SerializeField] private LayerMask _mask;

    Vector3 _initialPoint;

    private void Update()
    {
        Vector3 highestPoint = _initialPoint;
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, Mathf.Infinity, _mask))
        {
            highestPoint.y = Mathf.Max(hit.point.y, _initialPoint.y);
            _highestCakeTransform.position = highestPoint;
            Debug.DrawLine(transform.position, highestPoint, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, highestPoint, Color.yellow);
        }

        CakeRating.HeightReached = _highestCakeTransform.position.y;
    }
}
