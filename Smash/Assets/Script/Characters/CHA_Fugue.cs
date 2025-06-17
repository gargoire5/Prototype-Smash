using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CHA_Fugue : CharacterAttack
{
    //Fields to modify while making a character
    [Header("Attack Data - Basic Attack")]
    float basicAttackDuration = 0.2f;
    float basicAttackDelay = 0.0f;
    float basicAttackRate = 0.2f;
    float basicAttackDamage = 5.0f;

    [Header("Attack Data - Charge Attack")]
    float chargeAttackDuration = 0.2f;
    float chargeAttackDelay = 0.0f;
    float chargeAttackRate = 0.2f;
    float chargeAttackDamage = 7.5f;
    float chargeTimeTreshold = 0.5f;

    [Header("Attack Data - Skill")]
    float skillDuration = 0.2f;
    float skillDelay = 0.0f;
    float skillRate = 0.2f;
    float skillDamage = 10.0f;

    [Header("Attack Data - Ultimate")]
    float ultimateDuration = 0.2f;
    float ultimateDelay = 30.0f;
    float ultimateRate = 0.2f;
    float ultimateDamage = 20.0f;

    [SerializeField]
    private GameObject ultHitbox;

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

    }

    protected override void UltimeAttack()
    {
        if (canUseUltimate) return;

        Debug.Log("Ultimate");

        GameObject selectedHitbox = ultHitbox;

        StartCoroutine(UltAttack(selectedHitbox));
    }

    protected override void ParadeAction()
    {

    }

    private IEnumerator UltAttack(GameObject hitbox)
    {
        yield return new WaitForSeconds(ultimateDelay);

        GameObject currentHitbox = Instantiate(hitbox, transform.position, transform.rotation);

        yield return new WaitForSeconds(ultimateDuration);

        Destroy(currentHitbox);
    }
}
