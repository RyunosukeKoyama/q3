using UnityEngine;
using UnityEngine.SceneManagement;

public class SceceLoader : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
