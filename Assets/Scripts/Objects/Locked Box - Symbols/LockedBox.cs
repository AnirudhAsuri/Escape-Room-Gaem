using UnityEngine;

public class LockedBox : MonoBehaviour
{
    public DialRotate[] dials; // Array of all dials
    public string[] correctCombination; // Correct symbol combination

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
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
        anim.Play("Box Open");
    }
}