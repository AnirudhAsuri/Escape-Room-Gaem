using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreenUI;
    public GameObject player;
    private PlayerMovement playerMovement; // Adjust for your movement script

    void Start()
    {
        winScreenUI.SetActive(false);
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    public void TriggerWinScreen()
    {
        winScreenUI.SetActive(true);

        // Disable player controls
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Make sure "MainMenu" is spelled correctly!
    }
    public void RestartGame()
    {
        playerMovement.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}