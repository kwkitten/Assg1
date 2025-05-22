using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{

    bool doorOpen = false; // Variable to track if the door is open or closed
    // Method to handle door interaction    
    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;
        if (doorOpen == true)
        {
            doorRotation.y = 0f; // Reset the door rotation to closed position
            transform.eulerAngles = doorRotation; // Apply the rotation
            Debug.Log("Door has been closed!");
            doorOpen = false; // Set the door state to closed
        }
        else
        {
            doorRotation.y += 90f; // Rotate the door by 90 degrees
            transform.eulerAngles = doorRotation; // Apply the rotation
            Debug.Log("Door has been opened!");
            doorOpen = true; // Set the door state to open
        }
        
    }
}
