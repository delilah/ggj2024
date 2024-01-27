using UnityEngine;

public class CakeRating : MonoBehaviour
{
    public static int GoodLayers;
    public static int BadLayers;
    public static int TotalLayers => GoodLayers + BadLayers;

    private void Awake()
    {
        GoodLayers = BadLayers = 0;
    }
}
