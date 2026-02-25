using UnityEngine;

public class HealthTester : MonoBehaviour
{

    private PlayerHealth playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth.TakeEnemyDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            playerHealth.TakeEnemyDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            playerHealth.RespawnAfterFall();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            playerHealth.GainHeart();
        }
    }
}
