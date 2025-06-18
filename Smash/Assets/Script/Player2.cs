using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : CharacterAttack
{
    protected override void BasicAttack()
    {
        Debug.Log("BasicAttack");
        base.BasicAttack();
    }

    protected override void ChargeAttack()
    {
        Debug.Log("ChargeAttack");
    }

    protected override void SkillAttack()
    {

    }

    protected override void UltimeAttack()
    {

    }

    protected override void ParadeAction()
    {

    }
}
