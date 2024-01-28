using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CakeReview : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private List<string> _goodReviews;
    [SerializeField] private List<string> _badReviews;

    private void Awake()
    {
        var score = CakeRating.TotalLayers == 0? 0f : CakeRating.GoodLayers / (float) CakeRating.TotalLayers;
        if (score >= .6f)
        {
            _text.SetText(_goodReviews[Random.Range(0, _goodReviews.Count)]);
        }
        else
        {
            _text.SetText(_badReviews[Random.Range(0, _badReviews.Count)]);
        }
        _score.SetText($"{Mathf.RoundToInt(score * 10f)}/10");
    }
}
