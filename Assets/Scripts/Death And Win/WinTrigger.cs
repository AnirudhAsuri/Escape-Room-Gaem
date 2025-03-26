using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private WinScreen winScreen;

    private void Start()
    {
        winScreen = FindObjectOfType<WinScreen>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winScreen.TriggerWinScreen();
        }
    }
}