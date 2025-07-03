using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    TextMeshProUGUI scoreText; // Text to display the player's part count

    public int playerScore = 0; // Variable to track the player's score


    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // This is a LAZY singleton
        // Check if there is instance already and if the instance is the object
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // If it's not, destroy this object
        }
        else
        {
            instance = this; // If the instance is already set, do nothing
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
    }

    // Method to modify the player's score
    // This method takes an integer amount as a parameter
    // It adds the amount to the player's current score
    // The method is public so it can be accessed from other scripts
    public void ModifyScore(int amount)
    {
        playerScore += amount;
        scoreText.text = "Score: " + playerScore; // Update score text
    }

    public void TestFunction()
    {
        Debug.Log("Test function called from GameManager");
    }

}
