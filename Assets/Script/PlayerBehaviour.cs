using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Inventory.Model;

public class PlayerBehaviour : MonoBehaviour, Damage_Interface
{
    [Header("Basic Data")]
    public float walkSpeed;
    public float health;
    public KeyCode sprintKey;
    [SerializeField] public WeaponSO weaponSO;
    [SerializeField] private InventorySO inventoryData;

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
    bool walkSpeedMutiplyerEnabler = false;

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

            int walkSpeedMutiplyer = walkSpeedMutiplyerEnabler ? 3 : 1;

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
            StartCoroutine(delay(enabler => {
                sprintEnabler = !enabler;
            },2f));
            StartCoroutine(delay(enabler => {
                walkSpeedMutiplyerEnabler = !enabler;
            }, 0.2f));
        }
    }

    public int temp = 0;

    public void OnHit(float damage, Vector2 knockbackForce, float knockbackTime)
    {
        Health -= damage;

        //camera shake
        CameraShake cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
        onHitEffect.SetTrigger("Active");

        currentRb.velocity = knockbackForce;
        StartCoroutine(delay(enabler => {
            temp += !enabler ? 1 : -1;
            if (temp > 0 && enabler) return;
            movementEnabler = enabler;
            animator.enabled = enabler;
        },knockbackTime / 1));
    }

    public void SetWeapon(WeaponSO weaponItemSO)
    {
        if (weaponSO != null)
        {
            inventoryData.AddItem(weaponSO, 1);
        }
        this.weaponSO = weaponItemSO;

        GetComponentInChildren<SummonWeapon>().weaponSO = weaponSO;
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }

    
}
