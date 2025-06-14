using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    PlayerBehaviour playerScore; // Reference to the PlayerBehaviour script to access player score
    AudioSource doorAudioSource; // Reference to the AudioSource component for playing door sounds


    private void Start()
    {
        playerScore = FindFirstObjectByType<PlayerBehaviour>(); // Find the PlayerBehaviour script in the scene
        if (playerScore == null)
        {
            Debug.LogError("PlayerBehaviour script not found in the scene!");
        }

        doorAudioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    bool doorOpen = false; // Variable to track if the door is open or closed
    // Method to handle door interaction    
    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;
        if (playerScore.playerScore < 2)
        {
            playerScore.notificationText.text = "You need at least 2 parts to open the door!"; // Update the notification text
            return; // Exit the method if the player does not have enough parts
        }
        else if (playerScore.playerScore >= 2)
        {
            if (doorOpen == true)
            {
                doorRotation.y = -90f; // Reset the door rotation to closed position
                transform.eulerAngles = doorRotation; // Apply the rotation
                doorOpen = false; // Set the door state to closed
            }
            else
            {
                doorRotation.y += -90f; // Rotate the door by 90 degrees
                transform.eulerAngles = doorRotation; // Apply the rotation
                playerScore.notificationText.text = "You have successfully opened the door!"; // Update the notification text
                doorOpen = true; // Set the door state to open
                doorAudioSource.Play(); // Play the door sound
            }
        }
        else if (playerScore.playerScore >= 6)
        {
            playerScore.notificationText.text = "You have collected all parts! Congratulations! You have completed the level."; // Update the notification text
        }
    }

}
