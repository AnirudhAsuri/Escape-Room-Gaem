using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathScreenUI;
    public GameObject player;
    private PlayerMovement playerMovement;
    private PlayerCameraController playerCameraController;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerCameraController = player.GetComponent<PlayerCameraController>();
        deathScreenUI.SetActive(false);
    }

    public void TriggerDeathScreen()
    {
        deathScreenUI.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        // Unlock cursor and ensure it's interactable
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Make sure "MainMenu" is spelled correctly!
    }

    public void RestartGame()
    {
        playerMovement.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart level
    }
}