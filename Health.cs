using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")] [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    public HealthBar healthBar;
    private GameController gameController;

    public int MaxHealth = 100;

    [Header("iFrames")] [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();

        if (anim == null)
        {
            Debug.LogError("Animator component is missing from this game object");
        }

        if (spriteRend == null)
        {
            Debug.LogError("SpriteRenderer component is missing from this game object");
        }

        if (gameController == null)
        {
            Debug.LogError("GameController component is missing in the scene");
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth((int)currentHealth);
        }

        if (currentHealth > 0)
        {
            if (anim != null)
            {
                anim.SetTrigger("hurt");
            }

            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                if (anim != null)
                {
                    anim.SetTrigger("die");
                }

                // Player
                if (GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false;
                //Enemy
                if (GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                if (GetComponent<MeleEnemy>() != null)
                    GetComponent<MeleEnemy>().enabled = false;

                dead = true;

                // Appeler OnPlayerDeath dans GameController
                if (gameController != null)
                {
                    gameController.OnPlayerDeath();
                }
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        if (healthBar != null)
        {
            healthBar.SetHealth((int)currentHealth);
        }
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
