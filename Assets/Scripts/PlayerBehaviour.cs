/*
* Author: Katriel Wong
* Date: 2025-06-15
* Description: Player behaviour script for managing player interactions, health, and score.
*/

using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerBehaviour : MonoBehaviour
{
    int currentHealth = 200; // Current health of the player
    int maxHealth = 200; // Maximum health of the player
    int damage = 0; // Variable to track the damage taken by the player
    public Canvas alternateCanvas;

    [SerializeField]
    int totalParts = 6; // Total number of parts to collect in the game
    DoorBehaviour currentDoor; // Reference to the current door the player is interacting with
    CollectableBehaviour currentCollectable; // Reference to the current collectable the player is interacting with
    bool canInteract = false; // Flag to indicate if the player can interact with a collectable or door
    public int playerScore = 0; // Variable to track the player's score

    [SerializeField]
    Transform spawnPoint; // Reference to the spawn point of the player

    [SerializeField]
    float interactionDistance = 5f; // Distance within which the player can interact with collectables or doors

    [SerializeField]
    TextMeshProUGUI scoreText; // Text to display the player's part count

    [SerializeField]
    TextMeshProUGUI partLeftText; // Text to display the player's parts left

    [SerializeField]
    TextMeshProUGUI healthText; // Text to display the player's health

    [SerializeField]
    public TextMeshProUGUI notificationText; // Text to display notifications to the player 

    [SerializeField]
    Image partTrackingIcon; // UI Image to track the number of parts collected

    [SerializeField]
    UnityEngine.Sprite[] partTrackingSprites; // Array of sprites to represent the number of parts collected

    RaycastHit hitinfo; // Raycast hit information for detecting collectables and doors

    public int currentPartCount = 0; // Variable to track the number of parts collected by the player

    [SerializeField]
    GameObject RespawnPoint; // Reference to the respawn point

    void Start()
    {
        scoreText.text = "Parts Collected: " + playerScore.ToString(); // Initialize score text
        healthText.text = "Health: " + maxHealth.ToString(); // Initialize health text
        partLeftText.text = "Parts Left: " + totalParts.ToString(); // Initialize parts left text
        notificationText.text = ""; // Initialize notification text
        partTrackingIcon.sprite = partTrackingSprites[currentPartCount]; // Initialize part tracking icon
    }

    void Update()
    {
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitinfo, interactionDistance))
        {
            // If the raycast hits an object tagged as "Collectable" or "Door"
            if (hitinfo.collider.CompareTag("Collectable"))
            {
                if (hitinfo.collider.GetComponent<CollectableBehaviour>() != null)
                {
                    // If the object is a collectable part, we can interact with it
                    // Get the CollectableBehaviour component from the hit object
                    // This allows the player to interact with the collectable part
                    // The CollectableBehaviour script should handle the logic for collecting the part
                    canInteract = true;
                    currentCollectable = hitinfo.collider.GetComponent<CollectableBehaviour>();
                }
            }
            else if (hitinfo.collider.CompareTag("Door"))
            {
                currentDoor = hitinfo.collider.GetComponent<DoorBehaviour>(); // Get the DoorBehaviour component from the hit object
                canInteract = true; // Set the canInteract flag to true to allow interaction with the door
            }
        }
        else if (currentDoor != null || currentCollectable != null)
        {
            // If the raycast does not hit any object, reset the interaction state
            // This prevents the player from interacting with a collectable or door that is no longer in range
            if (currentCollectable != null)
            {
                currentCollectable = null; // Reset current collectable after interaction
                canInteract = false; // Reset interaction state
            }
            if (currentDoor != null)
            {
                //currentDoor.unhighlightDoor(); // Assuming this method exists to unhighlight the door
                currentDoor = null; // Reset current door after interaction
                canInteract = false; // Reset interaction state
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Hazard")) // If the player collides with a hazard object, we can apply damage
        {
            damage += 2;
            currentHealth -= damage; // Reduce the player's health by the damage amount
            healthText.text = "Health: " + currentHealth.ToString(); // Update health text

            if (currentHealth <= 0)
            {
                Debug.Log("Player is dead!"); // Log a message indicating the player is dead
                // Optionally, you can play a death animation or sound here
                // Instead of destroying the player object, we can respawn the player at a designated respawn point
                // Teleport the player back to the respawn point instead of destroying and instantiating
                // Move the player above the respawn pad
                if (RespawnPoint != null)
                {
                    Vector3 respawnAbove = RespawnPoint.transform.position + Vector3.up * 1.5f;
                    transform.SetPositionAndRotation(respawnAbove, RespawnPoint.transform.rotation);
                    currentHealth = maxHealth; // Optionally reset health
                }
                else
                {
                    Debug.LogError("RespawnPoint is not assigned in the Inspector!");
                }
            }
        }
        // If the player collides with a collectable object, we can interact with it
        // This will allow the player to collect the object
        // The CollectableBehaviour script should handle the logic for collecting the object
        else if (collision.gameObject.GetComponent<CollectableBehaviour>() != null)
        {
            currentCollectable = collision.gameObject.GetComponent<CollectableBehaviour>();
            canInteract = true;
            currentCollectable.Collect(this);
            AudioSource.PlayClipAtPoint(currentCollectable.collectSound, transform.position); // Play the collect sound if it is assigned
            currentCollectable = null; // Reset current collectable after interaction

            ++currentPartCount; // Increment the parts count
            if (currentPartCount >= partTrackingSprites.Length)
            {
                currentPartCount = partTrackingSprites.Length - 1; // Reset to last index if it exceeds the array length
            }
            partTrackingIcon.sprite = partTrackingSprites[currentPartCount]; // Update the part tracking icon
        }
        else if (collision.gameObject.CompareTag("Door"))
        {
            currentDoor = collision.gameObject.GetComponent<DoorBehaviour>();
            canInteract = true;
        }
    }

    void OnInteract()
    {
        if (canInteract)
        {
            // Check if the player has detected a collectable or a door
             if (currentDoor != null)
            {
                currentDoor.Interact();
                if (playerScore >= 6)
                {
                    notificationText.text = "You have collected all parts! Congratulations! You have completed the level."; // Update the notification text
                }
                currentDoor = null; // Reset current door after interaction
                canInteract = false; // Reset interaction state
            }
            else if (currentCollectable != null)
            {
                currentCollectable.Collect(this);
                AudioSource.PlayClipAtPoint(currentCollectable.collectSound, transform.position); // Play the collect sound if it is assigned
                currentCollectable = null; // Reset current collectable after interaction

                ++currentPartCount; // Increment the parts count
                if (currentPartCount >= partTrackingSprites.Length)
                {
                    currentPartCount = partTrackingSprites.Length - 1; // Reset to last index if it exceeds the array length
                }
                partTrackingIcon.sprite = partTrackingSprites[currentPartCount]; // Update the part tracking icon
            }
        }

        else
        {
            Debug.Log("Player is not interacting with an object");
        }
    }

    // Method to modify the player's score
    // This method takes an integer amount as a parameter
    // It adds the amount to the player's current score
    // The method is public so it can be accessed from other scripts
    public void ModifyScore(int amount)
    {
        playerScore += amount;
        scoreText.text = "Parts Collected: " + playerScore.ToString(); // Update score text
        totalParts -= amount; // Reduce the total parts left by the collected amount
        partLeftText.text = "Parts Left: " + totalParts.ToString(); // Update parts left text
    }

    // Collision Callback for when the player collides with another object
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard")) // If the player collides with a hazard object, we can apply damage
        {
            damage += 1;
            currentHealth -= damage; // Reduce the player's health by the damage amount
            // Ensure the health text is updated to reflect the current health
            healthText.text = "Health: " + currentHealth.ToString();
            if (currentHealth <= 0)
            {
                Debug.Log("Player is dead!"); // Log a message indicating the player is dead
            }
        }
        else if (collision.gameObject.CompareTag("InstantDeath"))
        {
            Debug.Log("Player has hit an instant death object!"); // Log a message indicating the player has hit an instant death object
            notificationText.text = "You have died"; // Update the notification text
            // Instead of destroying the player object, we can respawn the player at a designated respawn point
            // Teleport the player back to the respawn point instead of destroying and instantiating
            // Move the player above the respawn pad
            Vector3 respawnAbove = RespawnPoint.transform.position + Vector3.up * 1.5f;
            transform.SetPositionAndRotation(respawnAbove, RespawnPoint.transform.rotation);
            currentHealth = maxHealth; // Optionally reset health
        }
    }

    // Trigger Callback for when the player enters a trigger collider
        void OnTriggerEnter(Collider other)
    {
        // Check if the player detects a trigger collider tagged as "Collectable" or "Door"
        if (other.CompareTag("Collectable"))
        {
            Debug.Log(other.gameObject.name);
            if (other.GetComponent<CollectableBehaviour>() != null)
            {
                // If the object is a collectable part, we can interact with it
                // Get the CollectableBehaviour component from the detected object
                // Set the canInteract flag to true
                // This allows the player to interact with the collectable part
                canInteract = true;
                currentCollectable = other.GetComponent<CollectableBehaviour>();
            }
        }
        else if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.gameObject.GetComponent<DoorBehaviour>();
        }
    }

    // Trigger Callback for when the player exits a trigger collider
    void OnTriggerExit(Collider other)
    {
        // Check if the player has a detected collectable or door
        if (currentCollectable != null)
        {
            // If the object that exited the trigger is the same as the current collectable
            if (other.gameObject == currentCollectable.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current collectable to null
                // This prevents the player from interacting with the collectable
                canInteract = false;
                currentCollectable = null;
            }
        }
        else if (currentDoor != null)
        {
            // If the object that exited the trigger is the same as the current door
            if (other.gameObject == currentDoor.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current door to null
                // This prevents the player from interacting with the door
                canInteract = false;
                currentDoor = null;
            }
        }
    }
}