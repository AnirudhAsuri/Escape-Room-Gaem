using UnityEngine;

public class BoxLiftController : MonoBehaviour
{
    public BoxLift boxLift;
    public PressurePlateMechanics pressurePlateMechanics;

    public void LiftController()
    {
        if (pressurePlateMechanics.isPressed)
        {
            boxLift.Open();
        }
        else
        {
            boxLift.Close();
        }
    }
}