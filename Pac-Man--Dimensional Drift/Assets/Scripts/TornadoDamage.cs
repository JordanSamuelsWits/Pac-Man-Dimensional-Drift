using UnityEngine;

public class TornadoDamage : MonoBehaviour
{
    public int damageAmount = 2; // Damage dealt per second
    public float damageInterval = 5f; // Time between damage ticks

    private float nextDamageTime;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PacMan") && Time.time >= nextDamageTime)
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}
