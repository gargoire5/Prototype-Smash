using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charac_Deku : CharacterAttack
{
    public Corde corde;

    private void Start()
    {
        //Fields to modify when making a character

        basicAttackDuration = 0.15f;
        basicAttackDelay = 0.05f;
        basicAttackRate = 0.18f;
        basicAttackDamage = 4.0f;

        chargeAttackDuration = 0.2f;
        chargeAttackDelay = 0.0f;
        chargeAttackRate = 0.2f;
        chargeAttackDamage = 7f;
        chargeTimeTreshold = 0.5f;

        skillDuration = 0.2f;
        skillDelay = 0.0f;
        skillRate = 3.0f;

        ultimateDuration = 20f;
        ultimateDelay = 0.0f;
        ultimateRate = 20.0f;
    }
    protected override void BasicAttack()
    {
        base.BasicAttack();
        Debug.Log("BasicAttack bleb");
    }

    protected override void ChargeAttack()
    {
        Debug.Log("ChargeAttack");
    }

    protected override void SkillAttack()
    {
        base.SkillAttack();

        corde.StartFouet();
    }

    protected override void UltimateAttack()
    {
        base.UltimateAttack();

        StartCoroutine(UltBoost());
    }

    protected override void ParadeAction()
    {

    }

    private IEnumerator UltBoost()
    {
        float startspeed = GetComponent<PlayerController>().moveForce;
        float startJump = GetComponent<PlayerController>().bounceForce;
        float startBasicAttackDamage = basicAttackDamage;
        float startChargeAttackDamage = chargeAttackDamage;
        float startSkillRate = skillRate;

        yield return new WaitForSeconds(ultimateDelay);

        GetComponent<PlayerController>().moveForce *= 2;
        GetComponent<PlayerController>().bounceForce *= 1.2f;
        basicAttackDamage *= 2;
        chargeAttackDamage *= 2;
        skillRate /= 2;


        yield return new WaitForSeconds(ultimateDuration);

        GetComponent<PlayerController>().moveForce = startspeed;
        GetComponent<PlayerController>().bounceForce = startJump;
        basicAttackDamage = startBasicAttackDamage;
        chargeAttackDamage = startChargeAttackDamage;
        skillRate = startSkillRate;
    }

}
