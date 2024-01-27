using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private int _scene;
    [SerializeField] private bool _loadOnAnyInput;

    private void Update()
    {
        if (_loadOnAnyInput && Input.anyKey) Load();
    }

    public void Load()
    {
        SceneManager.LoadScene(_scene);
    }
}