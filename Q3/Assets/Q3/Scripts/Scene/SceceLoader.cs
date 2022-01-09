using UnityEngine;
using UnityEngine.SceneManagement;

public class SceceLoader : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
