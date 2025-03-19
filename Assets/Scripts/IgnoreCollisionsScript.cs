using UnityEngine;

public class IgnoreCollisionsScript : MonoBehaviour
{
    public GameObject objectToIgnore; // Assign this in the Inspector

    void Start()
    {
        if (objectToIgnore != null)
        {
            Collider[] thisColliders = GetComponents<Collider>(); // Get all colliders on this object
            Collider[] otherColliders = objectToIgnore.GetComponents<Collider>(); // Get all colliders on the target object

            if (thisColliders.Length > 0 && otherColliders.Length > 0)
            {
                foreach (Collider thisCollider in thisColliders)
                {
                    foreach (Collider otherCollider in otherColliders)
                    {
                        Physics.IgnoreCollision(thisCollider, otherCollider);
                    }
                }
            }
            else
            {
                Debug.LogWarning("One or both objects are missing colliders!", this);
            }
        }
        else
        {
            Debug.LogWarning("No object assigned to ignore collisions with!", this);
        }
    }
}