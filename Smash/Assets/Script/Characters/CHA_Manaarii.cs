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
        yield return new WaitForSeconds(UltimateDelay);

        int direction = 1;
        if (selectedHitbox == hitboxLeft)
            direction = -1;

        GameObject currentHitbox = Instantiate(hitbox, selectedHitbox.transform.position, selectedHitbox.transform.rotation);
        currentHitbox.GetComponent<Hitbox>().owner = this;
        currentHitbox.GetComponent<Rigidbody>().AddForce(0, 0, 30 * direction, ForceMode.Impulse);
        currentHitbox.GetComponent<ULT_Manaarii>().manaarii = gameObject;
        currentHitbox.GetComponent<ULT_Manaarii>().isGoingBack = true;

        yield return new WaitForSeconds(UltimateDuration);
        Destroy(currentHitbox);
    }
}
