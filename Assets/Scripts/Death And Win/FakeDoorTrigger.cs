using UnityEngine;

public class FakeDoorTrigger : MonoBehaviour
{
    private DeathScreen deathScreen;

    private void Start()
    {
        deathScreen = FindObjectOfType<DeathScreen>(); // Find the existing DeathScreen script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && deathScreen != null) // Ensure it's the player
        {
            deathScreen.TriggerDeathScreen(); // Trigger the death screen
        }
    }
}