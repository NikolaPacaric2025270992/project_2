using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int meleeDamage = 1;
    public Transform attackPoint;
    public GameObject fireSlashPrefab;
    public float attackRangeX = 1f;
    public float attackRangeY = 1f;
    public LayerMask enemyLayers;

    private PlayerControls controls;
    private Animator slashAnimator;

    public float attackCooldown = 0.5f;
    private bool canAttack = true;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Melee_Attack.performed += ctx => MeleeAttack();

        if (fireSlashPrefab != null)
        {
            slashAnimator = fireSlashPrefab.GetComponent<Animator>();
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void MeleeAttack()
    {
        if (!canAttack) return;
        if (fireSlashPrefab != null)
        {
            fireSlashPrefab.SetActive(true);
            slashAnimator.SetTrigger("PlaySlash");
        }

        StartCoroutine(AttackCooldownRoutine());
    }

    public void ApplyDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(meleeDamage);
            }
        }
        Debug.Log("Melee fire slash!");
    }

    private IEnumerator AttackCooldownRoutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
        

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}
