using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;
using static UnityEditor.Progress;
using System;
using Newtonsoft.Json;
using System.Linq;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerBehaviour : MonoBehaviour, Damageable
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
    public int currentCoinAmount = 0;
    public WeaponSO currentWeapon;
    public int weaponControl = 1;
    public WeaponSO meleeWeapon;
    public WeaponSO rangedWeapon;
    public EdibleItemSO potions;
    public int currentPotionAmont;
    public EquippableItemSO armor;
    public EquippableItemSO jewelry;
    public EquippableItemSO book;
    public int onHitCounter = 0;
    public List<EffectionList> effectionList = new List<EffectionList>();
    public List<KeyList> keyList = new List<KeyList>();

    [Serializable]
    public class EffectionList
    {
        public EdibleItemSO effectingItem;
        public float effectingTime;

        public EffectionList(EdibleItemSO item, float time)
        {
            this.effectingItem = item;
            this.effectingTime = time;
        }
    }

    [Serializable]
    public class KeyList
    {
        public KeySO key;
        public int quantity;

        public KeyList(KeySO item, int amont)
        {
            this.key = item;
            this.quantity = amont;
        }
    }

    [Header("Key Settings")]
    public KeyCode sprintKey;

    [Header("Connect Object")]
    public GameObject damageText;
    public Animator onHitEffect;
    public InventorySO inventoryData, shopData;
    public UIInventory inventoryUI, shopUI;
    public SummonWeapon summonWeapon;

    public float walkSpeed { get { return Basic_walkSpeed + E_walkSpeed; } }
    public float maxHealth { get { return Basic_maxHealth + E_maxHealth; } }
    public float strength { get { return Basic_strength + E_strength; } }
    public float defence { get { return Basic_defence + E_defence; } }
    public float critRate { get { return Basic_critRate + E_critRate; } }
    public float critDamage { get { return Basic_critDamage + E_critDamage; } }

    bool isCrit;
    public float Health
    {
        set
        {
            if (value < currentHealth)
            {
                //play hit animation

                //damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = Mathf.RoundToInt(currentHealth - value).ToString();
                text_MeshProUGUI.color = isCrit ? new Color(255, 255, 0, 255) : new Color(150, 0, 0, 255);
                text_MeshProUGUI.outlineColor = isCrit ? new Color(255, 0, 0, 255) : new Color(255, 255, 255, 0);
                text_MeshProUGUI.outlineWidth = isCrit ? 0.4f : 0f;


                //camera shake
                CameraShake cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
                StartCoroutine(cameraShake.Shake(0.1f, 0.2f));


                //scence effect
                onHitEffect.SetTrigger("OnHit");
            }

            if (value >= currentHealth)
            {
                //damage text
                RectTransform text_Transform = Instantiate(damageText).GetComponent<RectTransform>();
                text_Transform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                text_Transform.SetParent(GameObject.FindFirstObjectByType<Canvas>().transform);

                TextMeshProUGUI text_MeshProUGUI = text_Transform.GetComponent<TextMeshProUGUI>();
                text_MeshProUGUI.text = Mathf.RoundToInt(value - currentHealth).ToString();
                text_MeshProUGUI.color = new Color(0, 150, 0, 255);


                //scence effect
                onHitEffect.SetTrigger("Heal");
            }

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


    bool damageEnabler = true;
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
    
    void Update()
    {
        Moving();
        UpdatePlayerStates();

        //sprint
        if (Input.GetKeyDown(sprintKey)) Sprint();

        //set current weapon
        if (Input.GetKey(KeyCode.Alpha1)) weaponControl = 1;
        if (Input.GetKey(KeyCode.Alpha2)) weaponControl = 2;

        //use potion
        if (Input.GetKeyDown(KeyCode.Alpha3) && potions != null)
        {
            potions.ConsumeObject(1, gameObject);
            currentPotionAmont -= 1;

            if (currentPotionAmont <= 0) potions = null;
        } 

        //test addItem func
        if (Input.GetKeyDown(KeyCode.Alpha4) && potions != null) inventoryData.AddItem(potions, 10);

        //UI
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetInventoryContent(inventoryData, InventoryType.BackpackInventory);
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeInHierarchy);
        }

        //use weapon
        if (Input.GetKey(KeyCode.Mouse0)) summonWeapon.Summon();
    }





    private void Moving()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0;

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
                damageEnabler = enabler;
            }, 0.2f));
        }
    }

    private void UpdatePlayerStates()
    {
        //update player statistics
        string[] attributes = { "E_walkSpeed", "E_maxHealth", "E_strength", "E_defence", "E_critRate", "E_critDamage" };
        List<object> items = new List<object>{ armor, jewelry, book, currentWeapon };
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


        //update player health (in order not to overhealing)
        if (currentHealth > maxHealth) currentHealth = maxHealth;


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


        //update effection list
        int indexOfKeyList = -1;
        foreach (KeyList key in keyList)
        {
            if (key.quantity <= 0)
            {
                indexOfKeyList = keyList.IndexOf(key);
            }
        }
        keyList.Remove(indexOfKeyList != -1 ? keyList[indexOfKeyList] : null);


        //update on used weapon
        switch (weaponControl)
        {
            case 1:
                currentWeapon = meleeWeapon;
                break;
            case 2:
                currentWeapon = rangedWeapon;
                break;
        }
    }





    public void OnHit(float damage, bool _isCrit, Vector2 knockbackForce, float knockbackTime)
    {
        if (damageEnabler)
        {
            isCrit = _isCrit;
            Health -= damage / (1 +(0.001f * defence));

            //knockback
            currentRb.velocity = knockbackForce / (1 + (0.001f * defence));

            //delay
            StartCoroutine(delay(callback => {
                onHitCounter += !callback ? 1 : -1;
                if (onHitCounter > 0 && callback) return;
                movementEnabler = callback;
                animator.SetBool("isHit", !callback);
            }, knockbackTime / (1f + (0.001f * defence))));
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
        //Debug.Log(potions != null);
        if(potions != null) inventoryData.AddItem(potions, currentPotionAmont);
        potions = edibleItem;
        currentPotionAmont = amount;
    }

    public void SetEffection(EdibleItemSO edibleItem, int amount, float effectTime)
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
            effectionList.Add(new EffectionList(edibleItem, effectTime));
        }

        StartCoroutine(delay(callback =>
        {
            if (callback && edibleItem.E_heal != 0) Health += (edibleItem.E_heal + currentHealth) > maxHealth ? maxHealth - currentHealth : edibleItem.E_heal;
        }, 0.001f));
    }





    private IEnumerator delay(System.Action<bool> callback, float delayTime)
    {
        callback(false);
        yield return new WaitForSeconds(delayTime);
        callback(true);
    }
}