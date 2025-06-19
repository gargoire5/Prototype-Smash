using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : CharacterAttack
{
    /*public override float BasicDamage => 8f;
    public override float SkillDamage => 18f;
    public override float UltimateDamage => 35f;

    public override float BasicKnockback => 6f;
    public override float SkillKnockback => 12f;
    public override float UltimateKnockback => 25f;*/

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
        if (!canUseSkill)
            return;

        currentAttackType = AttackType.Skill;
        StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        canUseSkill = false;

        Debug.Log("▶️ Skill Dash lancé");

        Vector3 dashDirection = (lastMoveDirection.x > 0) ? Vector3.forward : Vector3.back;

        float dashSpeed = 45f;
        float dashTime = 0.1f;
        float elapsed = 0f;

        Vector3 velocity = dashDirection * dashSpeed;

        GameObject dashHitbox = selectedHitbox; // hitbox actuelle selon la direction
        StartCoroutine(ActivateHitboxCollider(dashHitbox));

        while (elapsed < dashTime)
        {
            transform.position += velocity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(UseSkill());
    }

    protected override void UltimeAttack()
    {
        currentAttackType = AttackType.Ultimate;
        base.BasicAttack();
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
