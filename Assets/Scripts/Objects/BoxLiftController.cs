using UnityEngine;

public class BoxLiftController : MonoBehaviour
{
    public BoxLift boxLift;
    public ButtonMechanics buttonMechanics;
    public PressurePlateMechanics pressurePlateMechanics;

    public void LiftController()
    {
        if (buttonMechanics.isPressed && pressurePlateMechanics.isPressed)
        {
            boxLift.Open();
        }
        else
        {
            boxLift.Close();
        }
    }
}