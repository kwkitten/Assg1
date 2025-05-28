using UnityEngine;

public class GiftBox : MonoBehaviour
{
    // Reference to the coin prefab
    [SerializeField]
    GameObject Loot;

    [SerializeField]
    float amountLoot = 3f; // Delay before the gift box is destroyed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Destroy the gift box
            Destroy(gameObject);
            Destroy(collision.gameObject); // Destroy the projectile

            // Spawn coins
            // Spawn more than one coin
            for (int i = 0; i < amountLoot; i++)
            {
                // Randomize the position slightly around the gift box
                Vector3 randomOffset = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(0f, 0.5f),
                    Random.Range(-0.5f, 0.5f)
                );
                GameObject coin = Instantiate(Loot, transform.position + randomOffset, Quaternion.identity);
            }
        }
    }
}
