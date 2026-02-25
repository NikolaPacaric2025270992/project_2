using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHearts = 3;
    public int segmentsPerHeart = 4;
    public int currentSegment;

    public Vector2 startingPossPoint;
    public Vector2 respawnPoint;


    void Start()
    {
        currentSegment = maxHearts * segmentsPerHeart;
        respawnPoint = transform.position;
        startingPossPoint = transform.position;
    }

    public void GainHeart()
    {
        maxHearts++;
        currentSegment = maxHearts * segmentsPerHeart;
        FindAnyObjectByType<HeartsUI>().AddHeart();
    }

    public void RespawnAfterFall()
    {
        int remainder = currentSegment % segmentsPerHeart;

        if (remainder == 0)
        {
            currentSegment -= segmentsPerHeart;
        }
        else
        {
            currentSegment -= remainder;
        }
        currentSegment = Mathf.Max(currentSegment, 0);

        transform.position = respawnPoint;
        if(currentSegment <= 0)
        {
            GameOver();
        }
    }

    public void RespawnAfterEnvironmentDamage(int segmentsLost)
    {
        currentSegment -= segmentsLost;
        currentSegment = Mathf.Max (currentSegment, 0);

        transform.position = respawnPoint;

        if(currentSegment <= 0)
        {
            GameOver();
        }
    }

    public void TakeEnemyDamage(int segmentsLost)
    {
        currentSegment -= segmentsLost;
        currentSegment = Mathf.Max(currentSegment, 0);

        if (currentSegment <= 0)
        {
            GameOver();
        }
    }

    public void Heal(int segmentsGained)
    {
        currentSegment = Mathf.Min(currentSegment + segmentsGained, maxHearts * segmentsGained);
    }

    private void GameOver()
    {
        transform.position = startingPossPoint;
        Debug.Log("Game Over! Restart required.");
        //Load main menu or restart Arc_1 scene
    }

    void Update()
    {
        
    }
}
