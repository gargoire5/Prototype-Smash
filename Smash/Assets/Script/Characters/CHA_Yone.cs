using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CHA_Yone : CharacterAttack
{
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    private Vector3 dashDirection;

    protected override void BasicAttack()
    {
        Debug.Log("BasicAttack");
        currentAttackType = AttackType.Basic;
        base.BasicAttack();
    }

    protected override void ChargeAttack()
    {
        Debug.Log("ChargeAttack");
        
    }

    protected override void SkillAttack()
    {
        if (!canUseSkill || _pc.isMove == false)
            return;

        currentAttackType = AttackType.Skill;
        StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        canUseSkill = false;

        Debug.Log("Skill Dash lancé");

        Vector3 dashDirection = (lastMoveDirection.x > 0) ? Vector3.forward : Vector3.back;

        float dashSpeed = 45f;
        float dashTime = 0.1f;
        float elapsed = 0f;

        Vector3 velocity = dashDirection * dashSpeed;

        GameObject dashHitbox = selectedHitbox; // hitbox actuelle selon la direction
        StartCoroutine(ActivateHitboxCollider(hitboxRight));
        StartCoroutine(ActivateHitboxCollider(hitboxLeft));

        while (elapsed < dashTime)
        {
            transform.position += velocity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(UseSkill());
    }

    protected override void UltimateAttack()
    {
        if (_pc.isMove)
        {
            Debug.Log("Ult lancé");
            /*if (!canUseUltimate)
                return;*/
            canUseUltimate = true;
            currentAttackType = AttackType.Ultimate;
            Debug.Log("Ult lancé");
            StartCoroutine(UltimateAreaDamage());
        }
    }

    private IEnumerator UltimateAreaDamage()
    {
        yield return new WaitForSeconds(ultimateDelay);

        DamageReceiver[] receivers = FindObjectsOfType<DamageReceiver>();

        foreach (var receiver in receivers)
        {
            if (receiver.gameObject != this.gameObject)
            {
                Vector3 direction = (receiver.transform.position - transform.position).normalized;

                receiver.TakeDamage(UltimateDamage, direction * UltimateKnockback);

                // On freeze la position pendant 2 secondes
                receiver.FreezePosition(2f);

                Debug.Log($"Ultimate inflige {UltimateDamage} à {receiver.name} et le gèle");
            }
        }

        StartCoroutine(UseUlt());
    }


    protected override void ParadeAction()
    {

    }
    public bool IsDashing()
    {
        return isDashing;
    }

    public Vector3 GetDashDirection()
    {
        return dashDirection;
    }
}
