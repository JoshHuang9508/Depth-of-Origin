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
    
    public List<InventoryItem> initialItems;

    [Header("Current Data")]
    public float currentHealth;
    public WeaponSO currentWeapon;
    public int coinAmount = 0;
    public WeaponSO meleeWeapon;
    public WeaponSO rangedWeapon;
    public EdibleItemSO potions;
    public int currentPotionAmont;
    public EquippableItemSO armor;
    public EquippableItemSO jewelry;
    public EquippableItemSO book;
    public List<EdibleItemSO> effection;

    [Header("Key Settings")]
    public KeyCode sprintKey;

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
                currentHealth = 0;

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

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentRb = GetComponent<Rigidbody2D>();

        //initial items
        inventoryData.initialize();
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            inventoryData.AddItem(item);
        }
    }

    int control = 1;

    void Update()
    {
        Moving();
        UpdatePlayerStates();

        //sprint
        if (Input.GetKeyDown(sprintKey)) Sprint();

        //set current weapon
        if (Input.GetKey(KeyCode.Alpha1)) control = 1;
        if (Input.GetKey(KeyCode.Alpha2)) control = 2;
        switch (control)
        {
            case 1:
                currentWeapon = meleeWeapon;
                break;
            case 2:
                currentWeapon = rangedWeapon;
                break;
        }

        //use potion
        if (Input.GetKeyDown(KeyCode.Alpha3) && potions != null) potions.PerformAction2(gameObject, 1);
    }
     
    

    int onHitCounter = 0;

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
            onHitCounter += !enabler ? 1 : -1;
            if (onHitCounter > 0 && enabler) return;
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

    public void SetEquipment(WeaponSO weapon, WeaponSO.WeaponType type)
    {
        switch (type)
        {
            case WeaponSO.WeaponType.Melee:
                if (meleeWeapon != null) inventoryData.AddItem(meleeWeapon, 1);
                meleeWeapon = weapon;
                break;
            case WeaponSO.WeaponType.Ranged:
                if (rangedWeapon != null) inventoryData.AddItem(rangedWeapon, 1);
                rangedWeapon = weapon;
                break;
        }
    }

    public void SetEquipment(EdibleItemSO edibleItem, int amount)
    {
        if(potions != null) inventoryData.AddItem(potions, 1);
        potions = edibleItem;
        currentPotionAmont = amount;
    }

    public void SetEffection(EdibleItemSO edibleItem, int amount, float effectTime)
    {
        currentHealth += edibleItem.E_heal;
        currentPotionAmont -= amount;
        if (currentPotionAmont <= 0)
        {
            potions = null;
        }

        StartCoroutine(delay(callback =>
        {
            if (!callback) effection.Add(edibleItem);
            else effection.Remove(edibleItem);
        }, effectTime));
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
        E_walkSpeed = (armor != null ? armor.E_walkSpeed : 0) + (jewelry != null ? jewelry.E_walkSpeed : 0) + (book != null ? book.E_walkSpeed : 0) + (currentWeapon != null ? currentWeapon.E_walkSpeed : 0);
        E_maxHealth = (armor != null ? armor.E_maxHealth : 0) + (jewelry != null ? jewelry.E_maxHealth : 0) + (book != null ? book.E_maxHealth : 0) + (currentWeapon != null ? currentWeapon.E_maxHealth : 0);
        E_strength = (armor != null ? armor.E_strength : 0) + (jewelry != null ? jewelry.E_strength : 0) + (book != null ? book.E_strength : 0) + (currentWeapon != null ? currentWeapon.E_strength : 0);
        E_defence = (armor != null ? armor.E_defence : 0) + (jewelry != null ? jewelry.E_defence : 0) + (book != null ? book.E_defence : 0) + (currentWeapon != null ? currentWeapon.E_defence : 0);
        E_critRate = (armor != null ? armor.E_critRate : 0) + (jewelry != null ? jewelry.E_critRate : 0) + (book != null ? book.E_critRate : 0) + (currentWeapon != null ? currentWeapon.E_critRate : 0);
        E_critDamage = (armor != null ? armor.E_critDamage : 0) + (jewelry != null ? jewelry.E_critDamage : 0) + (book != null ? book.E_critDamage : 0) + (currentWeapon != null ? currentWeapon.E_critDamage : 0);

        for(int i = 0; i < effection.Count; i++)
        {
            E_walkSpeed += effection[i].E_walkSpeed;
            E_maxHealth += effection[i].E_maxHealth;
            E_strength += effection[i].E_strength;
            E_defence += effection[i].E_defence;
            E_critRate += effection[i].E_critRate;
            E_critDamage += effection[i].E_critDamage;
        }

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }



    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}