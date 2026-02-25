using UnityEngine;

public class DiamondCollectable : MonoBehaviour
{

    public int lifeValue = 1;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.GainHeart();
            }

            animator.SetTrigger("Collected");
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public void DestroyDiamond()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
