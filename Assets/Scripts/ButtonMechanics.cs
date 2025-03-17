using UnityEngine;

public class ButtonMechanics : MonoBehaviour
{
    private Animator animator;
    public bool isPressed = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PressButton()
    {
        animator.Play(isPressed ? "Button Release" : "Button Press");
        isPressed = !isPressed;

        if (ButtonRegistrar.buttonMappings.TryGetValue(gameObject.name, out GameObject affectedObject))
        {
            IButtonAction buttonAction = affectedObject.GetComponent<IButtonAction>();
            if (buttonAction != null)
            {
                if (isPressed) buttonAction.Activate();
                else buttonAction.Deactivate();
            }
        }
    }
}
