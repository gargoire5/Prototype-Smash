using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CHA_Fugue : CharacterAttack
{
    [SerializeField]
    private GameObject ultObject;

    [SerializeField]
    private GameObject skillObject;

    public override float BasicDamage => 6f;
    public override float BasicKnockback => 2f;
    public override float BasicDuration => 0.4f;
    public override float BasicDelay => 0.05f;
    public override float BasicRate => 0.1f;

    public override float SkillDamage => 18f;
    public override float SkillKnockback => 7f;
    public override float SkillDuration => 0f;
    public override float SkillDelay => 0f;
    public override float SkillRate => 3f;

    public override float UltimateDamage => 35f;
    public override float UltimateKnockback => 12f;
    public override float UltimateDuration => 0.5f;
    public override float UltimateDelay => 0f;
    public override float UltimateRate => 30f;

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
        ultObject.GetComponent<Hitbox>().owner = this;

        StartCoroutine(UltAttack(selectedHitbox));
    }

    protected override void ParadeAction()
    {

    }

    private IEnumerator UltAttack(GameObject hitbox)
    {
        yield return new WaitForSeconds(UltimateDelay);

        GameObject currentHitbox = Instantiate(hitbox, transform.position, transform.rotation);

        yield return new WaitForSeconds(UltimateDuration);

        Destroy(currentHitbox);
    }
}
