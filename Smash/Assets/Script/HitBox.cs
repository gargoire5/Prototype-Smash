using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    public float damage = 10f;
    public CharacterAttack owner;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision avec {other.name} | isAttacking = {owner.isAttacking}");

        if (!owner.isAttacking)
        {
            Debug.Log("Ignor� (pas en attaque)");
            return;
        }

        Debug.Log("Attaque en cours : on v�rifie les d�g�ts...");

        if (other.gameObject == owner.gameObject)
            return;

        DamageReceiver receiver = other.GetComponent<DamageReceiver>();
        if (receiver != null)
        {
            Vector3 direction = (other.transform.position - owner.transform.position).normalized;
            receiver.TakeDamage(damage, direction);
            Debug.Log("D�g�ts inflig�s � " + other.name);
        }
        else
        {
            Debug.Log("Receiver NON trouv� sur " + other.name);
        }
    }
}