using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CHA_Greg : CharacterAttack
{
    [SerializeField]
    private GameObject ultObject;

    [SerializeField]
    private GameObject skillObject;

    public PlayerController playerController;
    private Vector2 inputdirection;

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
        currentSkill.transform.parent = transform;
        Vector2 dir = playerController.inputDirection;

        Vector3 dash = transform.forward * dir.x;

        Rigidbody rb = playerController.GetComponent<Rigidbody>();

        rb.AddForce(dash * 10, ForceMode.Impulse);
        
        Destroy(currentSkill, skillDuration);
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