using UnityEngine;

public class LockedBox : MonoBehaviour
{
    public DialRotate[] dials; // Array of all dials
    public string[] correctCombination; // Correct symbol combination

    public GameObject noteInside; // The note that appears when unlocked

    void Start()
    {
        noteInside.SetActive(false); // Hide the note at the start
    }

    public void CheckSolution()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            if (dials[i].symbols == null || dials[i].symbols.Length == 0)
            {
                Debug.LogError($"Dial {i}: symbols array is null or empty!");
                return;
            }

            if (dials[i].currentSymbolIndex < 0 || dials[i].currentSymbolIndex >= dials[i].symbols.Length)
            {
                Debug.LogError($"Dial {i}: currentSymbolIndex {dials[i].currentSymbolIndex} is out of bounds!");
                return;
            }

            if (dials[i].symbols[dials[i].currentSymbolIndex] != correctCombination[i])
                return;
        }

        UnlockBox();
    }


    void UnlockBox()
    {
        Debug.Log("Box Unlocked!");

        if (noteInside != null)
        {
            noteInside.SetActive(true); // Reveal the note
            noteInside.transform.SetParent(null); // Unparent the note so it stays in the world
        }

        // Optional: Play an animation or sound effect
    }
}