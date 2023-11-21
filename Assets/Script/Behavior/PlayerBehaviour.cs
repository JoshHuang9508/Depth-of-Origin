using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;
using System;

public class PlayerBehaviour : MonoBehaviour, Damageable
{
    [Header("Basic Data")]
    public float Basic_walkSpeed;
    public float Basic_maxHealth;
    public float Basic_strength;
    public float Basic_defence;
    public float Basic_critRate;
    public float Basic_critDamage;
    public List<InventoryItem> initialItems;

    [Header("Current Data")]
    public float currentHealth;
    public int currentCoinAmount = 0;
    public int weaponControl = 0;
    public WeaponSO currentWeapon;
    public WeaponSO meleeWeapon;
    public WeaponSO rangedWeapon;
    public EdibleItemSO potions;
    public int currentPotionAmont;
    public EquippableItemSO armor;
    public EquippableItemSO jewelry;
    public EquippableItemSO book;
    public List<EffectionList> effectionList = new();
    public List<KeyList> keyList = new();

    [Serializable]
    public class EffectionList
    {
        public EdibleItemSO effectingItem;
        public float effectingTime;
    }

    [Serializable]
    public class KeyList
    {
        public KeySO key;
        public int quantity;
    }

    [Header("Current Effect")]
    public float E_walkSpeed;
    public float E_maxHealth;
    public float E_strength;
    public float E_defence;
    public float E_critRate;
    public float E_critDamage;

    [Header("Key Settings")]
    public KeyCode sprintKey;
    public KeyCode backpackKey;
    public KeyCode usePotionKey;
    public KeyCode useWeaponKey;
    public KeyCode meleeWeaponKey;
    public KeyCode rangedWeaponKey;

    [Header("Connect Object")]
    public Animator onHitEffect;
    public InventorySO inventoryData, shopData;
    public UIInventory inventoryUI, shopUI;
    public SummonWeapon summonWeapon;
    public GameObject damageText;
    public GameObject itemDropper;

    public float walkSpeed { get { return Basic_walkSpeed + E_walkSpeed; } }
    public float maxHealth { get { return Basic_maxHealth + E_maxHealth; } }
    public float strength { get { return Basic_strength + E_strength; } }
    public float defence { get { return Basic_defence + E_defence; } }
    public float critRate { get { return Basic_critRate + E_critRate; } }
    public float critDamage { get { return Basic_critDamage + E_critDamage; } }

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
                Debug.Log("Player Dead");
                currentCoinAmount = 0;
                currentRb.bodyType = RigidbodyType2D.Static;
                damageEnabler = false;
                behaviourEnabler = false;
                shopUI.gameObject.SetActive(false);
                inventoryUI.gameObject.SetActive(false);

                //drop item
                List<Lootings> lootings = new();
                List<int> indexOfInvetoryItem = new();

                foreach (InventoryItem inventoryItem in inventoryData.inventoryItems)
                {
                    if(UnityEngine.Random.Range(0, 100) >= 50)
                    {
                        lootings.Add(new Lootings(inventoryItem.item, 100, inventoryItem.quantity));
                        indexOfInvetoryItem.Add(inventoryData.inventoryItems.IndexOf(inventoryItem));
                    }
                }
                foreach(int indexNum in indexOfInvetoryItem)
                {
                    Debug.Log(indexNum);
                    if (indexNum <= inventoryData.inventoryItems.Count)
                    {
                        inventoryData.RemoveItem(indexNum, -1);
                    }
                }
                ItemDropper ItemDropper = Instantiate(
                    itemDropper,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.identity,
                    GameObject.FindWithTag("Item").transform
                    ).GetComponent<ItemDropper>();
                ItemDropper.DropItems(lootings);


                //disable player object
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    [Header("Status")]
    public bool behaviourEnabler = true;
    public bool movementEnabler = true;
    public float movementDisableTimer = 0;
    public bool attackEnabler = true;
    public float attackDisableTimer = 0;
    public bool damageEnabler = true;
    public float damageDisableTimer = 0;
    public bool sprintEnabler = true;
    public float sprintDisableTimer = 0;
    public bool walkSpeedMutiplyerEnabler = false;
    public float walkSpeedMutiplyerDisableTimer = 0;

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
            if (item.IsEmpty) continue;
            inventoryData.AddItem(item);
        }
    }
    
    void Update()
    {
        if(!behaviourEnabler) return;

        Moving();
        UpdatePlayerStates();
        UpdateTimer();
        UpdateEffectionList();
        UpdateKeyList();
        UpdateCurrentWeapon();

        //sprint
        if (Input.GetKeyDown(sprintKey)) Sprint();

        //set current weapon
        if (Input.GetKeyDown(meleeWeaponKey)) weaponControl = weaponControl != 1 ? 1 : 0;
        if (Input.GetKeyDown(rangedWeaponKey)) weaponControl = weaponControl != 2 ? 2 : 0;

        //use potion
        if (Input.GetKeyDown(usePotionKey) && potions != null)
        {
            potions.ConsumeObject(1, gameObject);
            currentPotionAmont -= 1;

            if (currentPotionAmont <= 0) potions = null;
        } 

        //UI
        if (Input.GetKeyDown(backpackKey))
        {
            inventoryUI.SetInventoryContent(inventoryData, ActionType.BackpackInventory);
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeInHierarchy);
            Time.timeScale = inventoryUI.gameObject.activeInHierarchy ? 0 : 1;
        }

        //use weapon
        if (Input.GetKey(useWeaponKey) && attackEnabler)
        {
            currentWeapon = UpdateCurrentWeapon();
            if(currentWeapon != null)
            {
                attackDisableTimer += currentWeapon.attackCooldown;
                summonWeapon.Summon();
            }
        }
    }





    private void Moving()
    {
        animator.SetBool("isHit", !movementEnabler);
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        spriteRenderer.flipX = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2 ? Input.GetAxis("Horizontal") < 0 : spriteRenderer.flipX;

        if (movementEnabler && Input.anyKey)
        {
            int walkSpeedMutiplyer = walkSpeedMutiplyerEnabler ? 3 : 1;

            Vector2 movement = new Vector3(
                Input.GetAxis("Horizontal") * walkSpeed * walkSpeedMutiplyer,
                Input.GetAxis("Vertical") * walkSpeed * walkSpeedMutiplyer
            );

            currentRb.velocity = new Vector2(movement.x, movement.y);
        }
    }

    private void Sprint()
    {
        if (sprintEnabler && movementEnabler)
        {
            sprintDisableTimer += 2f;
            walkSpeedMutiplyerDisableTimer += 0.2f;
            damageDisableTimer += 0.2f;
        }
    }





    public bool UpdatePlayerStates()
    {
        //update player statistics
        string[] attributes = { "E_walkSpeed", "E_maxHealth", "E_strength", "E_defence", "E_critRate", "E_critDamage" };
        List<object> items = new(){ armor, jewelry, book, currentWeapon };
        float[] results = new float[attributes.Length];

        for (int j = 0; j < effectionList.Count; j++)
        {
            items.Add(effectionList[j].effectingItem);
        }

        int i = 0;
        foreach (var attribute in attributes)
        {
            results[i] = 0;

            for(int k = 0; k < items.Count; k++)
            {
                if (items[k] != null) results[i] += (float)items[k].GetType().GetField(attribute).GetValue(items[k]);
            }
            i++;
        }

        E_walkSpeed = results[0];
        E_maxHealth = results[1];
        E_strength = results[2];
        E_defence = results[3];
        E_critRate = results[4];
        E_critDamage = results[5];

        return true;
    }

    public bool UpdateTimer()
    {
        //update timer
        movementDisableTimer = Mathf.Max(0, movementDisableTimer - Time.deltaTime);
        attackDisableTimer = Mathf.Max(0, attackDisableTimer - Time.deltaTime);
        damageDisableTimer = Mathf.Max(0, damageDisableTimer - Time.deltaTime);
        sprintDisableTimer = Mathf.Max(0, sprintDisableTimer - Time.deltaTime);
        walkSpeedMutiplyerDisableTimer = Mathf.Max(0, walkSpeedMutiplyerDisableTimer - Time.deltaTime);

        movementEnabler = movementDisableTimer <= 0;
        attackEnabler = attackDisableTimer <= 0;
        damageEnabler = damageDisableTimer <= 0;
        sprintEnabler = sprintDisableTimer <= 0;
        walkSpeedMutiplyerEnabler = !(walkSpeedMutiplyerDisableTimer <= 0);

        return true;
    }

    public List<KeyList> UpdateKeyList()
    {
        //update key list
        int indexOfKeyList = -1;
        foreach (KeyList key in keyList)
        {
            if (key.quantity <= 0)
            {
                indexOfKeyList = keyList.IndexOf(key);
            }
        }
        keyList.Remove(indexOfKeyList != -1 ? keyList[indexOfKeyList] : null);
        return keyList;
    }

    public List<EffectionList> UpdateEffectionList()
    {
        //update effection list
        int indexOfEffectionList = -1;
        foreach (EffectionList effectingItem in effectionList)
        {
            effectingItem.effectingTime -= Time.deltaTime;
            if (effectingItem.effectingTime <= 0)
            {
                indexOfEffectionList = effectionList.IndexOf(effectingItem);
            }
        }
        effectionList.Remove(indexOfEffectionList != -1 ? effectionList[indexOfEffectionList] : null);
        return effectionList;
    }

    public WeaponSO UpdateCurrentWeapon()
    {
        //update current weapon
        switch (weaponControl)
        {
            case 0:
                currentWeapon = attackEnabler ? null : currentWeapon;
                break;
            case 1:
                currentWeapon = attackEnabler ? meleeWeapon : currentWeapon;
                break;
            case 2:
                currentWeapon = attackEnabler ? rangedWeapon : currentWeapon;
                break;
        }
        return currentWeapon;
    }





    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (UpdateTimer() && damageEnabler)
        {
            Health -= damage / (1 +(0.001f * defence));
            InstantiateDamageText(damage / (1 + (0.001f * defence)), "PlayerHit");

            //knockback
            currentRb.velocity = knockbackForce / (1 + (0.001f * defence));

            //delay
            movementDisableTimer = movementDisableTimer > knockbackTime / (1f + (0.001f * defence)) ? movementDisableTimer : knockbackTime / (1f + (0.001f * defence));

            //camera shake
            CameraController camera = GameObject.FindWithTag("MainCamera").GetComponentInParent<CameraController>();
            StartCoroutine(camera.Shake(0.1f, 0.2f));

            //scene effect
            onHitEffect.SetTrigger("OnHit");
        }
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
        if(potions != null) inventoryData.AddItem(potions, currentPotionAmont);
        potions = edibleItem;
        currentPotionAmont = amount;
    }

    public void SetEffection(EdibleItemSO edibleItem, float effectTime)
    {
        int indexOfEffectionList = 0;
        bool isEffectionExist = false;

        foreach (EffectionList effectingItem in effectionList)
        {
            if (edibleItem.ID == effectingItem.effectingItem.ID)
            {
                indexOfEffectionList = effectionList.IndexOf(effectingItem);
                isEffectionExist = true;
            }
        }

        if (isEffectionExist)
        {
            effectionList[indexOfEffectionList].effectingTime = effectTime;
        }
        else
        {
            effectionList.Add(new EffectionList {effectingItem = edibleItem , effectingTime = effectTime});
        }

        if (edibleItem.E_heal != 0) Health += Mathf.Min(maxHealth - currentHealth, edibleItem.E_heal);
        InstantiateDamageText(Mathf.Min(maxHealth - currentHealth, edibleItem.E_heal), "Heal");
    }

    private void InstantiateDamageText(float value, string type)
    {
        var damageTextInstantiated = Instantiate(
            damageText,
            Camera.main.WorldToScreenPoint(gameObject.transform.position),
            Quaternion.identity,
            GameObject.Find("ScreenUI").transform
            ).GetComponent<DamageText>();

        damageTextInstantiated.SetContent(value, type);
    }
}