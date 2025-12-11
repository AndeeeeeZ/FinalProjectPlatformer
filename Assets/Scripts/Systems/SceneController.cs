using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string targetSceneName; 

    public void SetNextScene(string n)
    {
        targetSceneName = n; 
    }
    public void ToTargetScene()
    {
        SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single); 
    }
}
