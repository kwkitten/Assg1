using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    MeshRenderer mymeshRenderer;

    [SerializeField]
    Material highlightMaterial; // Material to highlight the part

    Material originalMaterial; // Original material of the part

    [SerializeField]
    int partValue = 1;



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
        // Logic for collecting the part
        Debug.Log("Part collected!");

        // Add the part value to the player's score
        // This is done by calling the ModifyScore method on the player object
        // The partValue is passed as an argument to the method
        // This allows the player to gain points when they collect the part
        player.ModifyScore(partValue);

        Destroy(gameObject); // Destroy the part object
    }
}
