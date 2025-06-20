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
        currentSkill.GetComponent<Hitbox>().owner = this;

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