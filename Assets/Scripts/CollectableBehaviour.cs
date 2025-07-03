/*
* Author: Katriel Wong
* Date: 2025-06-15
* Description: This script handles the collection of parts in a Unity game.
*/

using UnityEditor.Build.Content;
using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    [SerializeField]
    public int partValue = 1;

    [SerializeField]
    public AudioClip collectSound; // Sound to play when collecting a part
    MeshRenderer mymeshRenderer; // Reference to the MeshRenderer component of the part

    [SerializeField]
    Material highlightMaterial;

    Material originalMaterial;

    void Start()
    {
        mymeshRenderer = GetComponent<MeshRenderer>();

        originalMaterial = mymeshRenderer.material; // Store the original material of the part
    }

    // Method to highlight the part
    // This method will be called when the player is close enough to interact with the part
    public void highlightPart()
    {
        mymeshRenderer.material = highlightMaterial;
    }

    // Method to unhighlight the part
    // This method will be called when the player moves away from the part
    public void unhighlightPart()
    {
        mymeshRenderer.material = originalMaterial;
    }

    // Method to collect the part
    // This method will be called when the player interacts with the part
    // It takes a PlayerBehaviour object as a parameter
    // This allows the part to modify the player's score
    // The method is public so it can be accessed from other scripts

    // Update is called once per frame

    public void Collect(PlayerBehaviour player)
    {
        GameManager.instance.ModifyScore(partValue);
        // Play the collect sound if it is assigned
        // AudioSource.PlayClipAtPoint(collectSound, transform.position);

        Destroy(gameObject);
        // Destroy the part object after collection
    }
}
