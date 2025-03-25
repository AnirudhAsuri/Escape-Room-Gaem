using UnityEngine;
using System.Collections.Generic;

public class GrandfatherClockMechanics : MonoBehaviour
{
    public GameObject clockWithPendulum;
    public List<GameObject> darts = new List<GameObject>(); // List to store multiple darts

    public void SwitchClock()
    {
        if (clockWithPendulum != null)
        {
            GameObject newClock = Instantiate(clockWithPendulum, transform.position, transform.rotation);

            // Get all animators in the new clock and restart them
            Animator[] animators = newClock.GetComponentsInChildren<Animator>();
            foreach (Animator animator in animators)
            {
                if (animator != null)
                {
                    animator.Rebind(); // Reset animation state
                    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0); // Restart animation
                }
            }

            // Activate all darts after 0.5 seconds
            foreach (GameObject dart in darts)
            {
                if (dart != null)
                {
                    dart.SetActive(true);
                }
            }

            Destroy(gameObject); // Remove the old clock
        }
        else
        {
            Debug.LogWarning("Clock with pendulum is not assigned!");
        }
    }
}