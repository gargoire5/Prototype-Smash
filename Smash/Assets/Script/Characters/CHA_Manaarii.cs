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

    public override float BasicDamage => 8f;
    public override float BasicKnockback => 3f;
    public override float BasicDuration => 0.4f;
    public override float BasicDelay => 0.05f;
    public override float BasicRate => 0.1f;

    public override float SkillDamage => 18f;
    public override float SkillKnockback => 17f;
    public override float SkillDuration => 0.2f;
    public override float SkillDelay => 0f;
    public override float SkillRate => 10f;

    public override float UltimateDamage => 10f;
    public override float UltimateKnockback => 7f;
    public override float UltimateDuration => 5f;
    public override float UltimateDelay => 0f;
    public override float UltimateRate => 20f;

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

        GameObject currentSkill = Instantiate(skillObject, transform.position, transform.rotation);
        currentSkill.GetComponent<Hitbox>().owner = this;
        Destroy(currentSkill, SkillDuration);
    }

    protected override void UltimateAttack()
    {
        base.UltimateAttack();

        if (selectedHitbox == null)
            selectedHitbox = hitboxRight;

        GameObject hitbox = ultObject;

        StartCoroutine(UltAttack(hitbox));
    }

    protected override void ParadeAction()
    {

    }

    private IEnumerator UltAttack(GameObject hitbox)
    {
        yield return new WaitForSeconds(ultimateDelay);

        int direction = 1;
        if (selectedHitbox == hitboxLeft)
            direction = -1;

        GameObject currentHitbox = Instantiate(hitbox, selectedHitbox.transform.position, selectedHitbox.transform.rotation);
        currentHitbox.GetComponent<Rigidbody>().AddForce(0, 0, 10 * direction);
        currentHitbox.GetComponent<ULT_Manaarii>().manaarii = gameObject;
        currentHitbox.GetComponent<ULT_Manaarii>().isGoingBack = true;

        yield return new WaitForSeconds(ultimateDuration);

        Destroy(currentHitbox);
    }
}
