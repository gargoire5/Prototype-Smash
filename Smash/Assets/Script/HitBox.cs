using UnityEngine;

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
        other.TryGetComponent<DamageReceiver>(out DamageReceiver receiver);

        if (owner != null)
        {
            if (owner != null)
            {
                if (other.gameObject == owner.gameObject)
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
                Debug.Log($"{owner.name} inflige {actualDamage} dégâts et {knockbackForce} de knockback à {other.name}");
            }
            else
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                receiver.TakeDamage(damage, direction);
            }
        }
        else if (receiver != null)
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            receiver.TakeDamage(damage, direction);
            Debug.Log("Dégâts infligés à " + other.name);
        }
    }
}
