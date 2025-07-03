using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene().buildIndex == 0) // Check if the current scene is the main menu
            {
                SceneManager.LoadScene(1); // Load the first level
            }
            else if(SceneManager.GetActiveScene().buildIndex == 1) // Check if the current scene is the first level
            {
                SceneManager.LoadScene(0); // Load the second level
            }
            else if(SceneManager.GetActiveScene().buildIndex == 2) // Check if the current scene is the second level
            {
                SceneManager.LoadScene(3); // Load the third level
            }
        }
    }
}
