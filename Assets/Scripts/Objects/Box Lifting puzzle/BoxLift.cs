using UnityEngine;

public class BoxLift : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.Play("BoxOpen");
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            animator.Play("BoxClose");
        }
    }
}