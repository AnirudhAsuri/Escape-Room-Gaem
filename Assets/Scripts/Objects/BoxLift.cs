using UnityEngine;

public class BoxLift : MonoBehaviour, IButtonAction
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Activate()
    {
        animator.Play("BoxOpen");
    }

    public void Deactivate()
    {
        animator.Play("BoxClose");
    }
}