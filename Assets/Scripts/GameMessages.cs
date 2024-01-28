public class GameMessages 
{
    public static event System.Action OnGameOver;
    public static void NotifyGameOver() => OnGameOver?.Invoke();

    public static event System.Action OnGoodLayerRequested;
    public static void RequestGoodLayer() => OnGoodLayerRequested?.Invoke();

    public static event System.Action OnBadLayerRequested;
    public static void RequestBadLayer() => OnBadLayerRequested?.Invoke();
}
