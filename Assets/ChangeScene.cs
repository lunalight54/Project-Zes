using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string newSceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(newSceneName);
    }
}
