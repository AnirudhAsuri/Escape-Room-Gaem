using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Puzzle Room"); // Change "GameScene" to your actual game scene name
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}