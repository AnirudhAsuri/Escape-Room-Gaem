using UnityEngine;

public enum DialState { NoDials, OneDial, TwoDials, ThreeDials }

public class MainDialBoxMechanics : MonoBehaviour
{
    public DialState currentState = DialState.ThreeDials;
    public GameObject[] dialBoxPrefabs; // Array: NoDials, OneDial, TwoDials, ThreeDials
    public GameObject dialPrefab; // The individual dial prefab

    public void RemoveDial()
    {
        if (currentState == DialState.NoDials) return;

        // Drop a new dial in front of the player
        Instantiate(dialPrefab, transform.position + -transform.forward * 0.3f, Quaternion.identity);

        // Switch to the correct prefab
        currentState--;
        SwapBox();
    }

    public void InsertDial()
    {
        if (currentState == DialState.ThreeDials) return;

        // Move to the next state
        currentState++;
        SwapBox();
    }

    private void SwapBox()
    {
        // Spawn the new box at the correct position
        GameObject newBox = Instantiate(dialBoxPrefabs[(int)currentState], transform.position, transform.rotation);

        // Get the script from the new prefab and update its state
        MainDialBoxMechanics newBoxScript = newBox.GetComponent<MainDialBoxMechanics>();
        if (newBoxScript != null)
        {
            newBoxScript.currentState = currentState; // Preserve the state
        }

        // Destroy the old box
        Destroy(gameObject);
    }

}
