using UnityEngine;
using UnityEngine.Events;

public class KnifeHitbox : MonoBehaviour
{
    public UnityEvent onHitCuttable;
    public UnityEvent onHitOther;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cuttable"))
        {
            Debug.Log("Knife hit a cuttable object.");
            onHitCuttable?.Invoke();
        }
        else
        {
            Debug.Log("Knife hit something else.");
            onHitOther?.Invoke();
        }
    }
}