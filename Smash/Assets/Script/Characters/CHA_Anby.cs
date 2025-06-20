using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CHA_Anby : CharacterAttack
{
    [SerializeField]
    private GameObject skillObject1;

    [SerializeField]
    private GameObject skillObject2;

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

        GameObject currentSkill;

        if (selectedHitbox == hitboxRight || selectedHitbox == hitboxLeft)
        {
            currentSkill = Instantiate(skillObject1, selectedHitbox.transform.position, selectedHitbox.transform.rotation);
        }
        else
        {
            currentSkill = Instantiate(skillObject2, selectedHitbox.transform.position, selectedHitbox.transform.rotation);
        }

        currentSkill.GetComponent<Hitbox>().owner = this;
        currentSkill.transform.SetParent(gameObject.transform);

        Destroy(currentSkill, SkillDuration);
    }

    protected override void UltimateAttack()
    {
        base.UltimateAttack();

        GetComponent<DamageReceiver>().damagePercent -= 50;
        if (GetComponent<DamageReceiver>().damagePercent < 0)
            GetComponent<DamageReceiver>().damagePercent = 0;
    }

    protected override void ParadeAction()
    {

    }
}