using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using TMPro;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;

public class PlayerBehaviour : MonoBehaviour
{
    // int score = 0;
    int health = 10;
    int maxHealth = 10;
    CoinBehaviour currentCoin;
    DoorBehaviour currentDoor;
    CollectableBehaviour currentCollectable;
    bool canInteract = false;
    public int playerScore = 0;

    // [SerializeField] // exposed variable
    // GameObject projectile;

    [SerializeField]
    Transform spawnPoint;

    // [SerializeField]
    // float fireStrength = 1000f;

    [SerializeField]
    float interactionDistance = 5f;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    UnityEngine.UI.Image partTrackingIcon;

    [SerializeField]
    Sprite[] partTrackingSprites;

    RaycastHit hitinfo;

    int currentPartCount = 0;

    void Start()
    {
        scoreText.text = "Parts Collected: " + playerScore.ToString(); // Initialize score text
        partTrackingIcon.sprite = partTrackingSprites[currentPartCount]; // Initialize part tracking icon
    }


    void Update()
    {
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitinfo, interactionDistance))
        {
            // If the raycast hits an object tagged as "Collectable" or "Door"
            if (hitinfo.collider.CompareTag("Collectable"))
            {
                Debug.Log("Player detected a collectable object: " + hitinfo.collider.name);
                if (hitinfo.collider.GetComponent<CoinBehaviour>() != null)
                {
                    // If the object is a coin, we can interact with it
                    // Get the CoinBehaviour component from the hit object
                    // This allows the player to interact with the coin
                    // The CoinBehaviour script should handle the logic for collecting the coin
                    if (currentCoin != null)
                    {
                        currentCoin.unhighlightCoin(); // Assuming this method exists to unhighlight the previous coin
                    }
                    canInteract = true;
                    currentCoin = hitinfo.collider.GetComponent<CoinBehaviour>();
                    currentCoin.highlightCoin(); // Assuming this method exists to highlight the coin
                }
                else if (hitinfo.collider.GetComponent<CollectableBehaviour>() != null)
                {
                    // If the object is a collectable part, we can interact with it
                    // Get the CollectableBehaviour component from the hit object
                    // This allows the player to interact with the collectable part
                    // The CollectableBehaviour script should handle the logic for collecting the part
                    if (currentCollectable != null)
                    {
                        currentCollectable.unhighlightPart(); // Assuming this method exists to unhighlight the previous collectable
                    }
                    canInteract = true;
                    currentCollectable = hitinfo.collider.GetComponent<CollectableBehaviour>();
                    currentCollectable.highlightPart(); // Assuming this method exists to highlight the collectable
                }
            }
            else if (hitinfo.collider.CompareTag("Door"))
            {
                currentDoor = hitinfo.collider.GetComponent<DoorBehaviour>();
                canInteract = true;
            }
        }
        else if (currentCoin != null || currentDoor != null || currentCollectable != null)
        {
            // If the raycast does not hit any object, reset the interaction state
            // This prevents the player from interacting with a coin or door that is no longer in range
            if (currentCoin != null)
            {
                currentCoin = null; // Reset current coin after interaction
                canInteract = false; // Reset interaction state
                
                currentCoin.unhighlightCoin(); // Assuming this method exists to unhighlight the coin
            }
            if (currentCollectable != null)
            {
                currentCollectable.unhighlightPart(); // Assuming this method exists to unhighlight the collectable
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

        if (collision.gameObject.CompareTag("Hazard"))
        {
            if (health <= 0)
            {
                Debug.Log("Player is dead!");
                // Optionally, you can destroy the player object or reset the game
                Destroy(gameObject);
            }
            else
            {
                health -= 2;
                Debug.Log("Player has taken damage! Health: " + health);
            }
        }

        if (collision.gameObject.CompareTag("Collectable"))
        {
            // If the player collides with a collectable object, we can interact with it
            // This will allow the player to collect the object
            // The CollectableBehaviour script should handle the logic for collecting the object
            if (collision.gameObject.GetComponent<CoinBehaviour>() != null)
            {
                currentCoin = collision.gameObject.GetComponent<CoinBehaviour>();
                canInteract = true;
                currentCoin.Collect(this);
                currentCoin = null; // Reset current coin after interaction
                // ++currentCoinCount; // Increment the coin count
                // if (currentCoinCount >= coinTrackingSprites.Length)
                // {
                //     currentCoinCount = coinTrackingSprites.Length - 1; // Reset to last index if it exceeds the array length
                // }
                // coinTrackingIcon.sprite = coinTrackingSprites[currentCoinCount]; // Update the coin tracking icon
            }
            else if (collision.gameObject.GetComponent<CollectableBehaviour>() != null)
            {
                currentCollectable = collision.gameObject.GetComponent<CollectableBehaviour>();
                canInteract = true;
                currentCollectable.Collect(this);
                currentCollectable = null; // Reset current collectable after interaction

                ++currentPartCount; // Increment the parts count
                if (currentPartCount >= partTrackingSprites.Length)
                {
                    currentPartCount = partTrackingSprites.Length - 1; // Reset to last index if it exceeds the array length
                }
                partTrackingIcon.sprite = partTrackingSprites[currentPartCount]; // Update the part tracking icon
            }
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
            // Check if the player has detected a coin or a door
            if (currentCoin != null)
            {
                Debug.Log("Interacting with coin");
                // Call the Collect method on the coin object
                // Pass the player object as an argument
                currentCoin.Collect(this);
                currentCoin = null; // Reset current coin after interaction
                // ++currentCoinCount; // Increment the coin count
                // if (currentCoinCount >= coinTrackingSprites.Length)
                // {
                //     currentCoinCount = coinTrackingSprites.Length - 1; // Reset to last index if it exceeds the array length
                // }
                // coinTrackingIcon.sprite = coinTrackingSprites[currentCoinCount]; // Update the coin tracking icon

            }
            else if (currentDoor != null)
            {
                Debug.Log("Player is interacting with a door");
                currentDoor.Interact();
                currentDoor = null; // Reset current door after interaction
                canInteract = false; // Reset interaction state
            }
            else if (currentCollectable != null)
            {
                Debug.Log("Interacting with collectable");
                currentCollectable.Collect(this);
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
        scoreText.text = "Score: " + playerScore.ToString(); // Update score text
    }

    // Method to modify the player's health
    // This method takes an integer amount as a parameter
    // It adds the amount to the player's current health
    // The method is public so it can be accessed from other scripts
    public void ModifyHealth(int amount)
    {
        // Check if the current health is less than the maximum health
        // If it is, increase the current health by the amount passed as an argument
        if (health < maxHealth)
        {
            health += amount;
            // Check if the current health exceeds the maximum health
            // If it does, set the current health to the maximum health
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }


    // Collision Callback for when the player collides with another object
    void OnCollisionStay(Collision collision)
    {
        // Check if the player collides with an object tagged as "Health"
        // If it does, call the RecoverHealth method on the object
        // Pass the player object as an argument
        // This allows the player to recover health when in a healing area
        if (collision.gameObject.CompareTag("Health"))
        {
            collision.gameObject.GetComponent<RecoveryBehaviour>().RecoverHealth(this);
        }
    }

    // Trigger Callback for when the player enters a trigger collider
    void OnTriggerEnter(Collider other)
    {
        // Check if the player detects a trigger collider tagged as "Collectable" or "Door"
        if (other.CompareTag("Collectable"))
        {
            Debug.Log(other.gameObject.name);
            if (other.GetComponent<CoinBehaviour>() != null)
            {
                // If the object is a coin, we can interact with it
                // Get the CoinBehaviour component from the detected object
                // Set the canInteract flag to true
                // This allows the player to interact with the coin
                canInteract = true;
                currentCoin = other.GetComponent<CoinBehaviour>();

            }
            else if (other.GetComponent<CollectableBehaviour>() != null)
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
            Debug.Log(other.gameObject.name);
        }
    }

    // Trigger Callback for when the player exits a trigger collider
    void OnTriggerExit(Collider other)
    {
        // Check if the player has a detected coin or door
        if (currentCoin != null)
        {
            // If the object that exited the trigger is the same as the current coin
            if (other.gameObject == currentCoin.gameObject)
            {
                // Set the canInteract flag to false
                // Set the current coin to null
                // This prevents the player from interacting with the coin
                canInteract = false;
                currentCoin = null;
            }
        }
        else if (currentCollectable != null)
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
    

    // Uncomment the following method if you want to implement projectile firing functionality
    // This method is called when the player fires a projectile
    // void OnFire()
    // {
    //     // Instantiate projectile at the spawn point's position and rotation
    //     // Store the projectile to the 'newProjectile' variable
    //     GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);

    //     // Create a new Vector3 variable 'fireForce'
    //     // set it to the forward direction of the spawn point muiltipled by the fire strength
    //     // This will determine the direction and speed of the projectile
    //     Vector3 fireForce = spawnPoint.forward * fireStrength;

    //     // Get the Rigidbody component of the new projectile
    //     // Add a force to the projectile defined by the fireForce variable
    //     newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
    // }

}
