using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CHA_Manaarii : CharacterAttack
{
    [SerializeField]
    private GameObject ultObject;

    [SerializeField]
    private GameObject skillObject;

    private void Start()
    {
        //Fields to modify when making a character

        basicAttackDuration = 0.2f;
        basicAttackDelay = 0.1f;
        basicAttackRate = 0.2f;
        basicAttackDamage = 5.0f;

        chargeAttackDuration = 0.2f;
        chargeAttackDelay = 0.0f;
        chargeAttackRate = 0.2f;
        chargeAttackDamage = 7.5f;
        chargeTimeTreshold = 0.5f;

        skillDuration = 0.2f;
        skillDelay = 0.0f;
        skillRate = 3.0f;
        skillDamage = 10.0f;

        ultimateDuration = 1f;
        ultimateDelay = 0.0f;
        ultimateRate = 30.0f;
        ultimateDamage = 20.0f;
    }

    protected override void BasicAttack()
    {
        base.BasicAttack();
    }

    protected override void ChargeAttack()
    {

    }

    protected override void SkillAttack()
    {
        base.SkillAttack();

        if (selectedHitbox == null)
            selectedHitbox = hitboxRight;

        GameObject currentSkill = Instantiate(skillObject, selectedHitbox.transform.position, selectedHitbox.transform.rotation);
        SKI_Fugue skill = currentSkill.GetComponent<SKI_Fugue>();

        if (selectedHitbox == hitboxLeft)
            skill.SetDirection(-1);
        Destroy(currentSkill, 2.0f);
    }

    protected override void UltimateAttack()
    {
        base.UltimateAttack();

        GameObject selectedHitbox = ultObject;

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
