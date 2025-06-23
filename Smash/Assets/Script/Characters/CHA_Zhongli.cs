using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHA_Zhongli : CharacterAttack
{
    [SerializeField]
    private GameObject ultObject;

    [SerializeField]
    private GameObject skillObject;


    protected override void BasicAttack()
    {
        base.BasicAttack();
    }

    protected override void ChargeAttack()
    {
        Debug.Log("ChargeAttack");
    }

    protected override void SkillAttack()
    {
       
    }

    protected override void UltimateAttack()
    {
        
    }

    protected override void ParadeAction()
    {

    }

    private IEnumerator UltAttack(GameObject hitbox)
    {
        
    }


}
