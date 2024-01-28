using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CakeReview : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Awake()
    {
        _text.SetText($"Cake Score: {CakeRating.GoodLayers} Good/{CakeRating.BadLayers} Bad Layers. Height Reached: {CakeRating.HeightReached}");
    }
}
