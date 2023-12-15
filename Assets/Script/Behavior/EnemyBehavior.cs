using NUnit;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, Damageable
{
    [Header("Object Reference")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    public Rigidbody2D currentRb;
    [SerializeField] private GameObject damageText;
    [SerializeField] private GameObject itemDropper;

    [Header("Audio")]
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deadSound;

    [Header("Dynamic Data")]
    public EnemySO enemy;
    [SerializeField] private GameObject target;
    public float currentHealth;
    public float currnetShieldHealth;
    [SerializeField] private Vector2 currentPos, targetPos, diraction;

    [Header("Stats")]
    public bool movementEnabler = true;
    public float movementDisableTimer = 0;
    public bool attackEnabler = true;
    public float attackDisableTimer = 0;
    public bool damageEnabler = true;
    public float damageDisableTimer = 0;
    public bool dodgeEnabler = true;
    public float dodgeDisableTimer = 0;
    public bool behaviourEnabler = true;

    public float Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;

            if (currentHealth <= 0)
            {
                //drop items
                ItemDropper ItemDropper = Instantiate(
                    itemDropper,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.identity,
                    GameObject.FindWithTag("Item").transform
                    ).GetComponent<ItemDropper>();
                ItemDropper.DropItems(enemy.lootings);
                ItemDropper.DropCoins(enemy.coins);
                ItemDropper.DropWrackages(enemy.wreckage);

                audioPlayer.PlayOneShot(deadSound);

                Destroy(gameObject);
            }
        }
    }

    public float ShieldHealth
    {
        get
        {
            return currnetShieldHealth;
        }
        set
        {
            currnetShieldHealth = value;

            if(currnetShieldHealth <= 0)
            {
                enemy.haveShield = false;
            }
        }
    }



    void Start()
    {
        currentHealth = enemy.health;
        currnetShieldHealth = enemy.shieldHealth;

        audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();

        if (enemy.isBoss) gameObject.tag = "Boss";
    }

    void Update()
    {
        try { target = GameObject.FindWithTag("Player"); } catch { }

        if (!behaviourEnabler) return;

        currentPos = transform.position;
        targetPos = target.transform.position;
        diraction = (targetPos - currentPos).normalized;

        //update timer
        UpdateTimer();

        //actions
        Moving();
        Attacking();
    }

    private void Moving()
    {
        spriteRenderer.flipX = (currentPos.x - targetPos.x) > 0.2;
        animator.enabled = movementEnabler;

        if (!movementEnabler) return;

        switch (enemy.attackType)
        {
            case EnemySO.AttackType.Melee:

                if (!attackEnabler) return;

                if (Vector3.Distance(targetPos, currentPos) < enemy.chaseField)
                {
                    currentRb.MovePosition(currentPos + enemy.moveSpeed * Time.deltaTime * diraction);

                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true);
                }
                else if (Vector3.Distance(targetPos, currentPos) > enemy.chaseField)
                {
                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", false);
                }
                break;

            case EnemySO.AttackType.Sniper:

                if (Vector3.Distance(targetPos, currentPos) < enemy.chaseField)
                {
                    currentRb.MovePosition(currentPos - enemy.moveSpeed * Time.deltaTime * diraction);

                    animator.SetBool("ismove", true);
                    animator.SetBool("ischase", true);
                }
                else if (Vector3.Distance(targetPos, currentPos) > enemy.attackField)
                {
                    currentRb.velocity = Vector2.zero;

                    animator.SetBool("ismove", false);
                    animator.SetBool("ischase", false);
                }
                break;
        }
    }

    private void Attacking()
    {
        if (!attackEnabler) return;

        switch (enemy.attackType)
        {
            case EnemySO.AttackType.Melee:

                if (Vector3.Distance(targetPos, currentPos) < enemy.attackField)
                {
                    Damageable damageableObject = target.GetComponent<Damageable>();

                    if(damageableObject != null)
                    {
                        damageableObject.OnHit(enemy.attackDamage, false, diraction * enemy.knockbackForce, enemy.knockbackTime);

                        attackDisableTimer += enemy.attackSpeed;
                    }
                }
                break;

            case EnemySO.AttackType.Sniper:

                if (Vector3.Distance(targetPos, currentPos) < enemy.attackField)
                {
                    enemy.Attack_Ranged(Mathf.Atan2(diraction.y, diraction.x) * Mathf.Rad2Deg, transform.position + new Vector3(0, 0.5f, 0));

                    attackDisableTimer += enemy.attackSpeed;
                }
                break;
        }
    }


    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (!damageEnabler) return;

        if (enemy.haveShield)
        {
            //update shield health
            ShieldHealth -= damage / (1 + (0.001f * enemy.defence));
        }
        else if (!enemy.haveShield)
        {
            //update heath
            Health -= damage / (1 + (0.001f * enemy.defence));

            //knockback
            currentRb.velocity = knockbackForce / (1 + (0.001f * enemy.defence));

            //play audio
            audioPlayer.PlayOneShot(hitSound);

            movementDisableTimer += knockbackTime / (1 + (0.001f * enemy.defence));
            attackDisableTimer += knockbackTime / (1 + (0.001f * enemy.defence));
        }

        //instantiate damage text
        DamageText.InstantiateDamageText(damageText, transform.position, damage / (1 + (0.001f * enemy.defence)), isCrit ? "DamageCrit" : "Damage");

        //delay
        damageDisableTimer += 0.2f;
    }

    public void SetShield(float _shieldHealth = 0)
    {
        if (_shieldHealth != 0)
        {
            ShieldHealth = _shieldHealth;
            enemy.haveShield = true;
        }
        else
        {
            ShieldHealth = enemy.shieldHealth;
            enemy.haveShield = true;
        }
    }

    private bool UpdateTimer()
    {
        movementDisableTimer = Mathf.Max(0, movementDisableTimer - Time.deltaTime);
        attackDisableTimer = Mathf.Max(0, attackDisableTimer - Time.deltaTime);
        damageDisableTimer = Mathf.Max(0, damageDisableTimer - Time.deltaTime);
        dodgeDisableTimer = Mathf.Max(0, dodgeDisableTimer - Time.deltaTime);

        movementEnabler = movementDisableTimer <= 0;
        attackEnabler = attackDisableTimer <= 0;
        damageEnabler = damageDisableTimer <= 0;
        dodgeEnabler = dodgeDisableTimer <= 0;

        return true;
    }
}