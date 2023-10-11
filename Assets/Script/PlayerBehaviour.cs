using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, Damage_Interface
{
    [Header("Basic Data")]
    public float walkSpeed;
    public float health;
    public KeyCode sprintKey;

    [Header("Connect Object")]
    public GameObject damageText;
    public Animator onHitEffect;

    public float Health
    {
        set
        {
            if (value < health)
            {
                //play hit animation

                //show damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.GetComponent<TextMeshProUGUI>().text = (health - value).ToString();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
                text_Transform.SetParent(canvas.transform);
            }

            health = value;

            if (health <= 0)
            {
                Debug.Log("Player Dead");
                //play dead animation
            }
        }
        get
        {
            return health;
        }
    }

    bool movementEnabler = true;
    bool sprintEnabler = false;
    int walkSpeedMutiplyer = 1;

    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D currentRb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();

        if (Input.GetKeyDown(sprintKey)) Sprint();
    }
     
    void Moving()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0 ? true : false;

        if (movementEnabler && Input.anyKey) {

            Vector3 movement = new Vector3(
                Input.GetAxis("Horizontal") * walkSpeed * walkSpeedMutiplyer, 
                Input.GetAxis("Vertical") * walkSpeed * walkSpeedMutiplyer, 
                0.0f
            );
            currentRb.velocity = new Vector2(movement.x, movement.y);
        }
    }

    void Sprint()
    {
        if (!sprintEnabler && movementEnabler)
        {
            StartCoroutine(sprint_delay());
        }
    }

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        Health -= damage;

        //camera shake
        CameraShake cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
        onHitEffect.SetTrigger("Active");

        StopCoroutine(knockback_delay(knockbackTime));
        currentRb.velocity = knockbackForce;
        StartCoroutine(knockback_delay(knockbackTime));
    }

    private IEnumerator knockback_delay(float knockbackTime)
    {
        animator.enabled = false;
        movementEnabler = false;
        yield return new WaitForSeconds(knockbackTime / 1);
        animator.enabled = true;
        movementEnabler = true;
    }

    private IEnumerator sprint_delay()
    {
        sprintEnabler = true;
        walkSpeedMutiplyer = 3;
        yield return new WaitForSeconds(0.2f);
        walkSpeedMutiplyer = 1;
        yield return new WaitForSeconds(2f);
        sprintEnabler = false;
    }
}
