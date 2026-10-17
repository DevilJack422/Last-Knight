using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "Main Menu")
        {
            MusicManager.Instance.PlayMusic("Main Menu");
        }    
        else if (sceneName == "Gameplay")
        {
            MusicManager.Instance.PlayMusic("Gameplay");
        }
    }
}
