using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAnimationEvents : MonoBehaviour
{
    private HammerMechanics hammerMechanics;

    void Start()
    {
        hammerMechanics = GetComponentInParent<HammerMechanics>();
    }

    public void EnableHitbox()
    {
        hammerMechanics?.EnableHitbox();
    }

    public void DisableHitbox()
    {
        hammerMechanics?.DisableHitbox();
    }

    public void EndAttack()
    {
        hammerMechanics?.EndAttack();
    }
}