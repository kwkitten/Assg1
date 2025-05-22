using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    int coinValue = 1;

    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(coinValue);
        Destroy(gameObject); // Destroy the coin object after collection
    }
}
