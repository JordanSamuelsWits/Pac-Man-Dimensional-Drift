/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int defaultDamageAmount = 10;

    public int ghostDamage = 5;
    public int forestPhantomDamage = 8;
    public int mummyDamage = 10;
    public int iceGolemDamage = 15;
    public int lavaMonsterDamage = 15;
    public int droneDamage = 5;
    public int megaPacmanDamage = 20;

    private string[] damageableTags = { "PacMan", "Ghost", "ForestPhantom", "Mummy", "IceGolem", "LavaMonster", "Drone", "MegaPacman" };

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a tag that should take damage
        if (collision.gameObject.CompareTag("PacMan") || Array.Exists(damageableTags, tag => collision.gameObject.CompareTag(tag)))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                int damageAmount = GetDamageAmountByTag(collision.gameObject.tag);
                health.TakeDamage(damageAmount);
            }
        }
    }

    int GetDamageAmountByTag(string tag)
    {
        switch (tag)
        {
            case "Ghost":
                return ghostDamage;
            case "ForestPhantom":
                return forestPhantomDamage;
            case "Mummy":
                return mummyDamage;
            case "IceGolem":
                return iceGolemDamage;
            case "LavaMonster":
                return lavaMonsterDamage;
            case "Drone":
                return droneDamage;
            case "MegaPacman":
                return megaPacmanDamage;
            default:
                return defaultDamageAmount;
        }
    }
}
*/

using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;  // Amount of damage the enemy deals

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PacMan")) // Check if the enemy collides with the player
        {
            Health playerHealth = other.GetComponent<Health>();  // Get the player's Health script
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);  // Deal damage to the player
            }
        }
    }
}

