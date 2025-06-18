using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : CharacterAttack
{
    public override float BasicDamage => 8f;
    public override float SkillDamage => 18f;
    public override float UltimateDamage => 35f;

    public override float BasicKnockback => 6f;
    public override float SkillKnockback => 12f;
    public override float UltimateKnockback => 25f;
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
        currentAttackType = AttackType.Skill;
        base.BasicAttack();
    }

    protected override void UltimateAttack()
    {
        currentAttackType = AttackType.Ultimate;
        base.BasicAttack();
    }

    protected override void ParadeAction()
    {

    }
}
