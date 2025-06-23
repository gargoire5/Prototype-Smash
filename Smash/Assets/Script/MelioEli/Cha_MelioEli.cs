using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cha_MelioEli : CharacterAttack
{
    public Texture melio;
    public Texture melioCounter;
    public Texture eli;
    public Texture eliSkill;
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

        StartCoroutine(Skill());
    }

    protected override void UltimateAttack()
    {
        base.UltimateAttack();

        StartCoroutine(UltBoost());
    }

    protected override void ParadeAction()
    {

    }

    private IEnumerator UltBoost()
    {
        
        yield return new WaitForSeconds(ultimateDelay);
        
        yield return new WaitForSeconds(ultimateDuration);

    }
    
    private IEnumerator Skill()
    {
        yield return new WaitForSeconds(skillDelay);
        GetComponent<DamageReceiver>().isCounter = true;
        GetComponent<MeshRenderer>().material.mainTexture = melioCounter;
        yield return new WaitForSeconds(skillDuration);
        GetComponent<DamageReceiver>().isCounter = false;
        GetComponent<MeshRenderer>().material.mainTexture = melio;
    }
}
