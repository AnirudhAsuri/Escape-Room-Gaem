using System.Collections;
using UnityEngine;

public class HiddenBox : MonoBehaviour
{
    public Transform lid; // Assign the lid transform in the inspector
    public GameObject hiddenObject; // Assign the object to be revealed inside the box
    public float liftHeight = 0.5f; // How high the lid moves
    public float liftDuration = 1f; // Time taken to lift the lid

    private Vector3 originalLidPosition;

    void Start()
    {
        if (lid != null)
        {
            originalLidPosition = lid.position; // Store the initial position of the lid
        }

        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false); // Hide the object at the start
        }
    }

    public void OpenLid()
    {
        if (lid != null)
        {
            StartCoroutine(LerpLidUp());
        }
    }

    private IEnumerator LerpLidUp()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = originalLidPosition + new Vector3(0, liftHeight, 0);

        while (elapsedTime < liftDuration)
        {
            lid.position = Vector3.Lerp(originalLidPosition, targetPosition, elapsedTime / liftDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lid.position = targetPosition; // Ensure the final position is set

        if (hiddenObject != null)
        {
            hiddenObject.SetActive(true); // Reveal the hidden object
        }
    }
}