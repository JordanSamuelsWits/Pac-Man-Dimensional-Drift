using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int defaultDamageAmount = 10; // Default damage for undefined tags

    // Define specific damage values for each enemy type
    public int ghostDamage = 5;
    public int forestPhantomDamage = 8;
    public int mummyDamage = 10;
    public int iceGolemDamage = 15;
    public int lavaMonsterDamage = 15;
    public int droneDamage = 5;
    public int megaPacmanDamage = 20;

    // Array of tags that should take damage on collision
    private string[] damageableTags = { "PacMan", "Ghost", "ForestPhantom", "Mummy", "IceGolem", "LavaMonster", "Drone", "MegaPacman" };

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a tag that should take damage
        foreach (string tag in damageableTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                // Get the health component of the collided object
                Health health = collision.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    // Determine the damage amount based on the tag
                    int damageAmount = GetDamageAmountByTag(tag);
                    health.TakeDamage(damageAmount);
                }
                break;
            }
        }
    }

    int GetDamageAmountByTag(string tag)
    {
        // Return specific damage amount based on the tag
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
                return defaultDamageAmount; // Default damage if tag doesn't match
        }
    }
}
