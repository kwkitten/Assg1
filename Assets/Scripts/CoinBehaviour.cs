using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    MeshRenderer mymeshRenderer;

    [SerializeField]
    Material highlightMaterial; // Material to highlight the coin

    Material originalMaterial; // Original material of the coin

    [SerializeField]
    int coinValue = 1;

    void Start()
    {
        mymeshRenderer = GetComponent<MeshRenderer>();

        originalMaterial = mymeshRenderer.material; // Store the original material of the coin
    }

    // Method to highlight the coin
    // This method will be called when the player is close enough to interact with the coin
    public void highlightCoin()
    {
        mymeshRenderer.material = highlightMaterial;
    }

    // Method to unhighlight the coin
    // This method will be called when the player moves away from the coin
    public void unhighlightCoin()
    {
        mymeshRenderer.material = originalMaterial;
    }

    // Method to collect the coin
    // This method will be called when the player interacts with the coin
    // It takes a PlayerBehaviour object as a parameter
    // This allows the coin to modify the player's score
    // The method is public so it can be accessed from other scripts
    public void Collect(PlayerBehaviour player)
    {
        // Logic for collecting the coin
        Debug.Log("Coin collected!");

        // Add the coin value to the player's score
        // This is done by calling the ModifyScore method on the player object
        // The coinValue is passed as an argument to the method
        // This allows the player to gain points when they collect the coin
        player.ModifyScore(coinValue);

        Destroy(gameObject); // Destroy the coin object
    }
}
