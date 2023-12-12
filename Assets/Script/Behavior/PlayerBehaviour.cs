using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;
using System;

public class PlayerBehaviour : MonoBehaviour, Damageable
{
    [Header("Setting")]
    [SerializeField] private float B_WalkSpeed;
    [SerializeField] private float B_MaxHealth;
    [SerializeField] private float B_Strength;
    [SerializeField] private float B_Defence;
    [SerializeField] private float B_CritRate;
    [SerializeField] private float B_CritDamage;
    [SerializeField] private List<InventoryItem> initialItems;

    [Header("Key Settings")]
    [SerializeField] public KeyCode sprintKey;
    [SerializeField] public KeyCode backpackKey;
    [SerializeField] public KeyCode usePotionKey;
    [SerializeField] public KeyCode useWeaponKey;
    [SerializeField] public KeyCode meleeWeaponKey;
    [SerializeField] public KeyCode rangedWeaponKey;

    [Header("Audio")]
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip hitSound;

    [Header("Object Reference")]
    public GameObject inventoryUI;
    public GameObject shopUI;
    [SerializeField] private Animator camEffect;
    [SerializeField] private Animator animator;
    [SerializeField] private SummonWeapon summonWeapon;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D currentRb;
    [SerializeField] private GameObject damageText;
    [SerializeField] private GameObject itemDropper;

    [Header("Dynamic Data")]
    public InventorySO inventoryData;
    public InventorySO equipmentData;
    public float currentHealth;
    public int currentCoinAmount = 0;
    public List<KeyList> keyList = new();

    [Header("Equipment")]
    public int weaponControl = 0;
    public WeaponSO currentWeapon;
    public WeaponSO meleeWeapon;
    public WeaponSO rangedWeapon;
    public EdibleItemSO potions;
    public int currentPotionAmont;
    public EquippableItemSO armor;
    public EquippableItemSO jewelry;
    public EquippableItemSO book;

    [Header("Effection")]
    public List<EffectionList> effectionList = new();
    [SerializeField] private float E_WalkSpeed;
    [SerializeField] private float E_MaxHealth;
    [SerializeField] private float E_Strength;
    [SerializeField] private float E_Defence;
    [SerializeField] private float E_CritRate;
    [SerializeField] private float E_CritDamage;

    [Header("Stats")]
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
    public bool healingEnabler = true;
    public float healingDisableTimer = 0;

    public float walkSpeed { get { return B_WalkSpeed * ((100 + E_WalkSpeed) / 100); } }
    public float maxHealth { get { return B_MaxHealth + E_MaxHealth; } }
    public float strength { get { return B_Strength + E_Strength; } }
    public float defence { get { return B_Defence + E_Defence; } }
    public float critRate { get { return B_CritRate + E_CritRate; } }
    public float critDamage { get { return B_CritDamage + E_CritDamage; } }

    public float Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            //if (value > currentHealth) camEffect.SetTrigger("Heal");

            if (value < currentHealth) camEffect.SetTrigger("OnHit");

            currentHealth = value;

            if (currentHealth <= 0)
            {
                //disable player object
                currentHealth = 0;
                currentCoinAmount = 0;
                currentRb.bodyType = RigidbodyType2D.Static;
                behaviourEnabler = false;
                shopUI.SetActive(false);
                inventoryUI.SetActive(false);

                //drop item
                List<Lootings> dropList = new();
                List<int> indexOfInvetoryItem = new();

                foreach (InventoryItem inventoryItem in inventoryData.inventoryItems)
                {
                    if(UnityEngine.Random.Range(0, 100) >= 50)
                    {
                        dropList.Add(new Lootings(inventoryItem.item, 100, inventoryItem.quantity));
                        indexOfInvetoryItem.Add(inventoryData.inventoryItems.IndexOf(inventoryItem));
                    }
                }
                foreach(int indexNum in indexOfInvetoryItem)
                {
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
                ItemDropper.DropItems(dropList);


                //disable player object
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

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



    public void OnSceneLoaded()
    {
        audioPlayer = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        //initial items
        inventoryData.initialize();
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty) continue;
            inventoryData.AddItem(item);
        }
    }
    
    private void Update()
    {
        if(!behaviourEnabler) return;

        //update timer
        UpdatePlayerStates();
        UpdateTimer();
        UpdateEffectionList();
        UpdateKeyList();
        UpdateCurrentWeapon();

        //actions
        Moving();
        Heal();

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
            inventoryUI.GetComponent<UIInventory>().SetInventoryContent(inventoryData, ActionType.BackpackInventory);
            inventoryUI.SetActive(!inventoryUI.gameObject.activeInHierarchy);
            Time.timeScale = inventoryUI.gameObject.activeInHierarchy ? 0 : 1;
        }

        //use weapon
        if (Input.GetKey(useWeaponKey) && attackEnabler)
        {
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

    private void Heal()
    {
        if (healingEnabler && currentHealth != maxHealth)
        {
            float healValue = Mathf.Min(maxHealth - currentHealth, maxHealth * 0.05f);
            Health += healValue;
            DamageText.InstantiateDamageText(damageText, transform.position, healValue, "Heal");
            healingDisableTimer = 5;
        }
    }

    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler && behaviourEnabler)
        {
            //update heath
            Health -= damage / (1 + (0.001f * defence));

            //instantiate damege text
            DamageText.InstantiateDamageText(damageText, transform.position, damage / (1 + (0.001f * defence)), "PlayerHit");

            //play audio
            //audioPlayer.PlayOneShot(hitSound);

            //knockback
            currentRb.velocity = knockbackForce / (1 + (0.001f * defence));

            //set timer
            movementDisableTimer = movementDisableTimer > knockbackTime / (1f + (0.001f * defence)) ? movementDisableTimer : knockbackTime / (1f + (0.001f * defence));
            healingDisableTimer = 20;

            //camera shake
            CameraController camera = GameObject.FindWithTag("MainCamera").GetComponentInParent<CameraController>();
            StartCoroutine(camera.Shake(0.1f, 0.2f));
        }
    }





    private void UpdatePlayerStates()
    {
        //float currentHealthPercent = currentHealth / maxHealth;

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

        E_WalkSpeed = results[0];
        E_MaxHealth = results[1];
        E_Strength = results[2];
        E_Defence = results[3];
        E_CritRate = results[4];
        E_CritDamage = results[5];

        if (currentHealth > maxHealth) currentHealth = maxHealth;
        //currentHealth = maxHealth * currentHealthPercent;
    }

    private void UpdateTimer()
    {
        //update timer
        movementDisableTimer = Mathf.Max(0, movementDisableTimer - Time.deltaTime);
        attackDisableTimer = Mathf.Max(0, attackDisableTimer - Time.deltaTime);
        damageDisableTimer = Mathf.Max(0, damageDisableTimer - Time.deltaTime);
        sprintDisableTimer = Mathf.Max(0, sprintDisableTimer - Time.deltaTime);
        walkSpeedMutiplyerDisableTimer = Mathf.Max(0, walkSpeedMutiplyerDisableTimer - Time.deltaTime);
        healingDisableTimer = Mathf.Max(0, healingDisableTimer - Time.deltaTime);

        movementEnabler = movementDisableTimer <= 0;
        attackEnabler = attackDisableTimer <= 0;
        damageEnabler = damageDisableTimer <= 0;
        sprintEnabler = sprintDisableTimer <= 0;
        walkSpeedMutiplyerEnabler = !(walkSpeedMutiplyerDisableTimer <= 0);
        healingEnabler = healingDisableTimer <= 0;
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

    private void UpdateEffectionList()
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

    public void UnEquipment(EquippableItemSO equipment, EquippableItemSO.EquipmentType type)
    {
        switch (type)
        {
            case EquippableItemSO.EquipmentType.armor:
                if (armor != null) inventoryData.AddItem(armor, 1);
                armor = null;
                break;
            case EquippableItemSO.EquipmentType.jewelry:
                if (jewelry != null) inventoryData.AddItem(jewelry, 1);
                jewelry = null;
                break;
            case EquippableItemSO.EquipmentType.book:
                if (book != null) inventoryData.AddItem(book, 1);
                book = null;
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
    public void UnEquipment(WeaponSO weapon, WeaponSO.WeaponType type)
    {
        switch (type)
        {
            case WeaponSO.WeaponType.Melee:
                if (meleeWeapon != null) inventoryData.AddItem(meleeWeapon, 1);
                meleeWeapon = null;
                break;
            case WeaponSO.WeaponType.Ranged:
                if (rangedWeapon != null) inventoryData.AddItem(rangedWeapon, 1);
                rangedWeapon = null;
                break;
        }
    }

    public void SetEquipment(EdibleItemSO edibleItem, int amount)
    {
        if(potions != null) inventoryData.AddItem(potions, currentPotionAmont);
        potions = edibleItem;
        currentPotionAmont = amount;
    }

    public void UnEquipment(EdibleItemSO edibleItem, int amount)
    {
        if (potions != null) inventoryData.AddItem(potions, currentPotionAmont);
        potions = null;
        currentPotionAmont = 0;
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
            if (edibleItem.effectTime != 0)
            {
                effectionList.Add(new EffectionList { effectingItem = edibleItem, effectingTime = effectTime });
            }
        }

        if (edibleItem.E_heal != 0)
        {
            float healValue = Mathf.Min(maxHealth - currentHealth, edibleItem.E_heal);
            Health += healValue;
            DamageText.InstantiateDamageText(damageText, transform.position, healValue, "Heal");
        }  
    }
}