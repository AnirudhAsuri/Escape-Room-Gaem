using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    public float breakChance = 0.7f; // 70% chance to break

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakable"))
        {
            Debug.Log("Found Box");
            BreakableBoxMechanics breakable = other.GetComponent<BreakableBoxMechanics>();
            if (breakable != null)
            {
                breakable.BreakBox();
            }
        }
        else
        {
            Debug.Log("Not Recognizing Tag");
        }
    }
}