using UnityEngine;

public class CakeRating : MonoBehaviour
{
    public static int GoodLayers;
    public static int BadLayers;
    public static int TotalLayers => GoodLayers + BadLayers;

    public static float HeightReached;

    private void Awake()
    {
        GoodLayers = BadLayers = 0;
    }
}
