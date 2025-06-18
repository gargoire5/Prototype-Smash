using UnityEngine;
using System.Collections;

public class Hitbox : MonoBehaviour
{
    public float damage = 10f;
    public CharacterAttack owner = null;

    private void Start()
    {
        if (owner == null)
        {
            if (transform.parent != null)
                transform.parent.TryGetComponent<CharacterAttack>(out owner);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == null || other.gameObject == owner.gameObject)
            return;

        if (!other.TryGetComponent(out DamageReceiver receiver))
            return;

        float actualDamage = damage;
        float knockbackForce = 0f;

        switch (owner.currentAttackType)
        {
            case AttackType.Basic:
                actualDamage = owner.BasicDamage;
                knockbackForce = owner.BasicKnockback;
                break;
            case AttackType.Skill:
                actualDamage = owner.SkillDamage;
                knockbackForce = owner.SkillKnockback;
                break;
            case AttackType.Ultimate:
                actualDamage = owner.UltimateDamage;
                knockbackForce = owner.UltimateKnockback;
                break;
        }

        Vector3 direction = (other.transform.position - owner.transform.position).normalized;

        receiver.TakeDamage(actualDamage, direction * knockbackForce);

        if (owner.currentAttackType == AttackType.Skill)
        {
            if (other.TryGetComponent(out Rigidbody rb))
            {
                owner.StartCoroutine(FreezeAndTeleportBehind(other.transform, direction));
            }

            Vector3 direction = (other.transform.position - owner.transform.position).normalized;
            receiver.TakeDamage(actualDamage, direction * knockbackForce);
            Debug.Log($"{owner.name} inflige {actualDamage} dégâts et {knockbackForce} de knockback à {other.name}");
        }

        Debug.Log($"{owner.name} inflige {actualDamage} dégâts et {knockbackForce} knockback à {other.name}");
    }
    private IEnumerator FreezeAndTeleportBehind(Transform target, Vector3 direction)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        //freeze 0.5 secondes
        yield return new WaitForSeconds(0.5f);

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        //calcule la direction Z du dash
        float offsetZ = direction.z >= 0 ? 1.5f : -1.5f;

        //déplacement derrière
        Vector3 newPos = target.position + new Vector3(0, 0, offsetZ);
        owner.transform.position = newPos;
    }

}
