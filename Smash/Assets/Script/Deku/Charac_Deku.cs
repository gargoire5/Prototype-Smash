using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charac_Deku : CharacterAttack
{
    public Corde corde;
    public float multipUlt;
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
        float startSpeedFouet = corde.speed;

        yield return new WaitForSeconds(ultimateDelay);

        GetComponent<PlayerController>().moveForce *= multipUlt;
        GetComponent<PlayerController>().bounceForce *= (1 + multipUlt/10);
        basicAttackDamage *= multipUlt;
        chargeAttackDamage *= multipUlt;
        skillRate /= multipUlt;
        corde.speed *= multipUlt;

        yield return new WaitForSeconds(ultimateDuration);

        GetComponent<PlayerController>().moveForce = startspeed;
        GetComponent<PlayerController>().bounceForce = startJump;
        basicAttackDamage = startBasicAttackDamage;
        chargeAttackDamage = startChargeAttackDamage;
        skillRate = startSkillRate;
        corde.speed = startSpeedFouet;
    }

}
