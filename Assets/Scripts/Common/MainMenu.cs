using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionPanel;

    void Awake()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);

    }

    public void StartButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OptionButton()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void BackButton()
    {
        optionPanel.SetActive(true == false);
        mainPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
