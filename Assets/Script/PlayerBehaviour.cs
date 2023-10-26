using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Inventory.Model;
using static UnityEditor.Progress;
using System;
using Newtonsoft.Json;

public class PlayerBehaviour : MonoBehaviour, Damage_Interface
{
    [Header("Basic Data")]
    public float Basic_walkSpeed;
    public float Basic_maxHealth;
    public float Basic_strength;
    public float Basic_defence;
    public float Basic_critRate;
    public float Basic_critDamage;

    float E_walkSpeed;
    float E_maxHealth;
    float E_strength;
    float E_defence;
    float E_critRate;
    float E_critDamage;

    public List<WeaponSO> weapon;
    public EquippableItemSO armor;
    public EquippableItemSO jewelry;
    public EquippableItemSO book;

    public KeyCode sprintKey;

    [Header("Current Data")]
    public float currentHealth;
    public int currentWeapon = 0;
    public int coinAmount = 0;

    [Header("Connect Object")]
    public GameObject damageText;
    public Animator onHitEffect;
    public InventorySO inventoryData;

    public float walkSpeed { get { return Basic_walkSpeed + E_walkSpeed; } }
    public float maxHealth { get { return Basic_maxHealth + E_maxHealth; } }
    public float strength { get { return Basic_strength + E_strength; } }
    public float defence { get { return Basic_defence + E_defence; } }
    public float critRate { get { return Basic_critRate + E_critRate; } }
    public float critDamage { get { return Basic_critDamage + E_critDamage; } }
    public float Health
    {
        set
        {
            if (value < currentHealth)
            {
                //play hit animation
            }

            if (value > maxHealth) currentHealth = maxHealth;

            currentHealth = value;

            if (currentHealth <= 0)
            {
                Debug.Log("Player Dead");
                //play dead animation
            }
        }
        get
        {
            return currentHealth;
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
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        UpdatePlayerStates();


        //sprint
        if (Input.GetKeyDown(sprintKey)) Sprint();


        //set current weapon
        if (Input.GetKey(KeyCode.Alpha1)) currentWeapon = 0;
        if (Input.GetKey(KeyCode.Alpha2)) currentWeapon = 1;
        if (Input.GetKey(KeyCode.Alpha3)) currentWeapon = 2;
    }
     
    

    public int temp = 0;

    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        Health -= damage;

        //damage text
        RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
        text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

        TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
        text_MeshProUGUI.text = damage.ToString();
        text_MeshProUGUI.color = isCrit ? new Color(255, 255, 0, 255) : new Color(255, 255, 255, 255);
        text_MeshProUGUI.outlineColor = isCrit ? new Color(255, 0, 0, 255) : new Color(255, 255, 255, 0);
        text_MeshProUGUI.outlineWidth = isCrit ? 0.4f : 0f;


        //camera shake
        CameraShake cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
        onHitEffect.SetTrigger("Active");


        //knockback
        currentRb.velocity = knockbackForce;

        
        //delay
        StartCoroutine(delay(enabler => {
            temp += !enabler ? 1 : -1;
            if (temp > 0 && enabler) return;
            movementEnabler = enabler;
            animator.enabled = enabler;
        },knockbackTime * (1f - (0.001f * defence))));
    }

    public void SetEquipment(EquippableItemSO equipment, EquippableItemSO.EquipmentType type)
    {
        switch (type)
        {
            case EquippableItemSO.EquipmentType.armor:
                if (armor != null) inventoryData.AddItem(armor, 1);
                armor = equipment;
                break;
            case EquippableItemSO.EquipmentType.jewelry:
                if (jewelry != null) inventoryData.AddItem(jewelry, 1);
                jewelry = equipment;
                break;
            case EquippableItemSO.EquipmentType.book:
                if (book != null) inventoryData.AddItem(book, 1);
                book = equipment;
                break;
        }
    }

    public void SetEquipment(WeaponSO weapon)
    {
        if (this.weapon[0] == null) this.weapon[0] = weapon;
        else if (this.weapon[1] == null) this.weapon[1] = weapon;
        else if (this.weapon[2] == null) this.weapon[2] = weapon;
        else
        {
            inventoryData.AddItem(this.weapon[0], 1);
            this.weapon[0] = weapon;
        }
    }

    private void Moving()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0 ? true : false;

        if (movementEnabler && Input.anyKey)
        {

            int walkSpeedMutiplyer = walkSpeedMutiplyerEnabler ? 3 : 1;

            Vector3 movement = new Vector3(
                Input.GetAxis("Horizontal") * walkSpeed * walkSpeedMutiplyer,
                Input.GetAxis("Vertical") * walkSpeed * walkSpeedMutiplyer,
                0.0f
            );

            currentRb.velocity = new Vector2(movement.x, movement.y);
        }
    }

    private void Sprint()
    {
        if (!sprintEnabler && movementEnabler)
        {
            StartCoroutine(delay(enabler => {
                sprintEnabler = !enabler;
            }, 2f));
            StartCoroutine(delay(enabler => {
                walkSpeedMutiplyerEnabler = !enabler;
            }, 0.2f));
        }
    }

    private void UpdatePlayerStates()
    {
        E_walkSpeed = (armor != null ? armor.E_walkSpeed : 0) + (jewelry != null ? jewelry.E_walkSpeed : 0) + (book != null ? book.E_walkSpeed : 0) + (weapon[currentWeapon] != null ? weapon[currentWeapon].E_walkSpeed : 0);
        E_maxHealth = (armor != null ? armor.E_maxHealth : 0) + (jewelry != null ? jewelry.E_maxHealth : 0) + (book != null ? book.E_maxHealth : 0) + (weapon[currentWeapon] != null ? weapon[currentWeapon].E_maxHealth : 0);
        E_strength = (armor != null ? armor.E_strength : 0) + (jewelry != null ? jewelry.E_strength : 0) + (book != null ? book.E_strength : 0) + (weapon[currentWeapon] != null ? weapon[currentWeapon].E_strength : 0);
        E_defence = (armor != null ? armor.E_defence : 0) + (jewelry != null ? jewelry.E_defence : 0) + (book != null ? book.E_defence : 0) + (weapon[currentWeapon] != null ? weapon[currentWeapon].E_defence : 0);
        E_critRate = (armor != null ? armor.E_critRate : 0) + (jewelry != null ? jewelry.E_critRate : 0) + (book != null ? book.E_critRate : 0) + (weapon[currentWeapon] != null ? weapon[currentWeapon].E_critRate : 0);
        E_critDamage = (armor != null ? armor.E_critDamage : 0) + (jewelry != null ? jewelry.E_critDamage : 0) + (book != null ? book.E_critDamage : 0) + (weapon[currentWeapon] != null ? weapon[currentWeapon].E_critDamage : 0);
    }

    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}
