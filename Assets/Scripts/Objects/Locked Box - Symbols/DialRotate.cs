using UnityEngine;

public class DialRotate : MonoBehaviour
{
    public int currentSymbolIndex = 0;  // Tracks which symbol is selected
    public string[] symbols;  // Array of symbols (set in Inspector)
    public LockedBox box;  // Reference to the locked box script
    public int totalSymbols = 4;  // Default to 4 symbols per dial
    public AudioSource clickSound;  // Optional: Sound when rotating

    void Start()
    {
        totalSymbols = symbols.Length;
        if (symbols == null || symbols.Length == 0)
        {
            Debug.LogError($"Dial {gameObject.name}: symbols array is empty at Start! Assign values in the Inspector.");
        }
    }

    public void RotateDial()
    {
        currentSymbolIndex = (currentSymbolIndex + 1) % totalSymbols; // Cycle through symbols
        float angle = 360f / totalSymbols; // Calculate rotation angle
        transform.Rotate(0, -angle, 0); // Rotate dial dynamically

        if (clickSound != null)
            clickSound.Play(); // Play sound on rotation

        if (box != null)
            box.CheckSolution(); // Check if puzzle is solved
    }
}