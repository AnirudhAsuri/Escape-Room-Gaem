using UnityEngine;

public class DartboardMechanics : MonoBehaviour
{
    private string[] correctSequence = { "Dartboard 10", "Dartboard 30", "Dartboard 10" };
    private int currentIndex = 0;

    public HiddenBox hiddenBox; // Assign this in the inspector

    public void CheckDartHit(string hitTag)
    {
        if (hitTag == correctSequence[currentIndex])
        {
            currentIndex++; // Move to the next required hit
            Debug.Log($"Correct hit: {hitTag} ({currentIndex}/{correctSequence.Length})");

            if (currentIndex >= correctSequence.Length)
            {
                Debug.Log("Dartboard puzzle complete!");
                hiddenBox.OpenLid(); // Unlock the hidden box
                currentIndex = 0; // Reset after completion
            }
        }
        else
        {
            Debug.Log("Incorrect hit! Resetting sequence.");
            currentIndex = 0; // Reset the sequence if the wrong section is hit
        }
    }
}