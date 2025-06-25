using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DamageReceiver : MonoBehaviour
{
    public float damagePercent = 0f;
    public float knockbackMultiplier = 1.5f;
    private Rigidbody rb;
    public bool isCounter = false;

    private bool isFrozen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        damagePercent += amount;
        float force = amount * (1 + damagePercent / 100) * knockbackMultiplier;

        if (!isFrozen)
        {
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }

        Debug.Log($"{gameObject.name} a reçu {amount} dégâts. Total: {damagePercent}%");
    }

    public void FreezePosition(float duration)
    {
        if (isFrozen) return;
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        isFrozen = true;

        Vector3 originalVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        yield return new WaitForSeconds(duration);

        rb.isKinematic = false;
        rb.velocity = originalVelocity;
        isFrozen = false;
    }
}