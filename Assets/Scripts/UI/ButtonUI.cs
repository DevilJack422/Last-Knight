using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string gameplayScene = "Gameplay";

    public void StartButton()
    {
        SceneManager.LoadScene(gameplayScene);
    }
}
