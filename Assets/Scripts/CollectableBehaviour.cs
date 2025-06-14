/*
* Author: Katriel Wong
* Date: 2025-06-15
* Description: This script handles the collection of parts in a Unity game.
*/

using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    [SerializeField]
    int partValue = 1;

    [SerializeField]
    public AudioClip collectSound; // Sound to play when collecting a part

    // Method to collect the part
    // This method will be called when the player interacts with the part
    // It takes a PlayerBehaviour object as a parameter
    // This allows the part to modify the player's score
    // The method is public so it can be accessed from other scripts

    // Update is called once per frame
    public void Collect(PlayerBehaviour player)
    {
        // Add the part value to the player's score
        player.ModifyScore(partValue);
        // Play the collect sound if it is assigned
        AudioSource.PlayClipAtPoint(collectSound, transform.position);

        Destroy(gameObject);
        // Destroy the part object after collection
    }
}
