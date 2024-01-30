using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScenesLoader : MonoBehaviour
{
    [SerializeField] private int _cakeLayersScene;
    [SerializeField] private int _inputScene;

    private IEnumerator Start()
    {
        var asyncLayers = SceneManager.LoadSceneAsync(_cakeLayersScene, LoadSceneMode.Additive);
        var asyncInput = SceneManager.LoadSceneAsync(_inputScene, LoadSceneMode.Additive);

        while (!asyncLayers.isDone && !asyncInput.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }
}
