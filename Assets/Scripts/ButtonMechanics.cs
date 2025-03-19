using UnityEngine;
using UnityEngine.Events;

public class ButtonMechanics : MonoBehaviour
{
    private Animator animator;
    public bool isPressed = false;

    [Header("Events")]
    public UnityEvent onPress;
    public UnityEvent onRelease;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PressButton()
    {
        animator.Play(isPressed ? "Button Release" : "Button Press");
        isPressed = !isPressed;

        if (isPressed)
        {
            onPress.Invoke(); // Invoke UnityEvent when button is pressed
        }
        else
        {
            onRelease.Invoke(); // Invoke UnityEvent when button is released
        }
    }
}