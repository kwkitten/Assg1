using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // int score = 0;
    int health = 10;
    int maxHealth = 10;
    CoinBehaviour currentCoin;
    bool canInteract = false;
    public int playerScore = 0;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Hazard"))
        {
            if (health <= 0)
            {
                Debug.Log("Player is dead!");
                // Optionally, you can destroy the player object or reset the game
                // Destroy(gameObject);
            }
            else
            {
                health -= 2;
                Debug.Log("Player has taken damage! Health: " + health);
            }
        }
    }

    void OnInteract()
    {
        if (canInteract)
        {
            Debug.Log("Player is interacting with an object");
            currentCoin.Collect(this);
            currentCoin = null; // Reset current coin after collection
            canInteract = false; // Reset interaction state
        }
        else
        {
            Debug.Log("Player is not interacting with an object");
        }
    }

    public void ModifyScore(int amount)
    {
        playerScore += amount;
        Debug.Log("Player score: " + playerScore);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            // Handle collectable item logic here
            Debug.Log("Player is looking at " + other.gameObject.name);
            currentCoin = other.gameObject.GetComponent<CoinBehaviour>();
            canInteract = true;
        }   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            // Handle collectable item logic here
            currentCoin = null; // Reset current coin when exiting trigger
            canInteract = false; // Reset interaction state
        }
    }

    // public class RecoveryBehaviour : MonoBehaviour
    // {
    //     int healAmount = 5;

    //     void RecoverHealth(PlayerBehaviour player)
    //     {
    //         if (player.health < player.maxHealth)
    //         {
    //             player.health += healAmount;
    //             Debug.Log("Player health recovered: " + player.health);
    //         }
    //         else
    //         {
    //             Debug.Log("Player is already at full health");
    //         }
    //     }
    // }

    // public class PlayerBehaviour : MonoBehaviour
    // {
    //     // This class is empty for now, but you can add player-related methods here
    //     int maxHealth = 100;
    //     int currentHealth = 100;

    //     void OnCollisionStay(Collision collision)
    //     {
    //         if (collision.gameObject.CompareTag("Health"))
    //         {
    //             if (currentHealth < maxHealth)
    //             {
    //                 currentHealth += 10;
    //                 Debug.Log("Player health: " + currentHealth);
    //             }
    //             else
    //             {
    //                 Debug.Log("Player is already at full health");
    //             }
    //         }
    //     }

    //     void ModifyHealth(int amount)
    //     {
    //         currentHealth += amount;
    //         Debug.Log("Player health: " + currentHealth);
    //         if (currentHealth > maxHealth)
    //         {
    //             currentHealth = maxHealth;
    //             Debug.Log("Player is at full health");
    //         }
    //         else if (currentHealth <= 0)
    //         {
    //             Debug.Log("Player is dead");
    //             // Optionally, you can destroy the player object or reset the game
    //             // Destroy(gameObject);
    //         }
    //     }
    // }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Health"))
        {   
            if (health >= maxHealth)
            {
                health = maxHealth;
                Debug.Log("Player is already at full health");
                return;
            }
            else
            {
                ++health;
                Debug.Log("Player health: " + health);
            }   
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
