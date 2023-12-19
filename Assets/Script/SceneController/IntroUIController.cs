using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroUiController : MonoBehaviour
{
    [SerializeField] private NotificationManager notification;
    [SerializeField] private Sprite complete, hint;
    [SerializeField] private GameObject meleeweaponChest, rangeweaponChest, potionChest;
    [SerializeField] public List<GameObject> pot,crate;
    [SerializeField] private GameObject spawner;
    bool isAllPotBreak,isAnyCrateBroke,monsterisAlive = false;
    ChestController meleechest, rangechest, potionchest;
    [SerializeField] private GameObject Monster;


    void Start()
    {
        notification.Close();
        notification.title = "";
        notification.description = "";
        meleechest = meleeweaponChest.GetComponent<ChestController>();
        potionchest = potionChest.GetComponent<ChestController>();
        rangechest = rangeweaponChest.GetComponent<ChestController>();
        meleechest.interactable.interactable = true;
        rangechest.interactable.interactable = false;
        potionchest.interactable.interactable = false;

        SetHint("Hint", "Open the chest");
        notification.enableTimer = false;
        notification.Open();
    }

    // Update is called once per frame
    void Update()
    {
        notification.UpdateUI();
        if(pot.All(item => item == null) && !isAllPotBreak)
        {
            isAllPotBreak = true;
            StartCoroutine(delayPotAllDestroy());
        }
        if(crate.All(item => item == null) && !isAnyCrateBroke)
        {
            isAnyCrateBroke = true;
            StartCoroutine(backpackHintDone());
        }
        try
        {
            if (Monster != null)
            {
                monsterisAlive = true;
            }
        }
        catch { }

        if(Monster == null && monsterisAlive)
        {
            monsterisAlive = false;
            StartCoroutine(delayMosterDead());
        }

        
    }

    public void SetHint(string title, string description)
    {
        notification.title = title;
        notification.description = description;
        switch (title)
        {
            case "Complete":
                notification.icon = complete;
                break;
            case "Hint":
                notification.icon = hint;
                break;
        }
    }

    public void meleeChestOpened()
    {
        StartCoroutine(delaymeleechestOpened());
    }

    public void rangedChestOpened()
    {
        StartCoroutine(delayrangedchestOpened());
    }

    public void potionChestOpened()
    {
        StartCoroutine(delaypotionChestOpened());
    }

    IEnumerator delayMosterDead()
    {
        notification.Close();
        yield return new WaitForSeconds(1f);
        SetHint("Complete", "The Monster have been defeated!");
        notification.enableTimer = true;
        notification.Open();
        yield return new WaitForSeconds(3f);
        SetHint("Hint", "walk forward to the forest");
        notification.enableTimer = false;
        notification.Open();
    }

    public void enterForest()
    {
        PlayerPrefs.SetString("loadscene", "Town");
    }

    IEnumerator backpackHintDone()
    {
        notification.Close();
        yield return new WaitForSeconds(1f);
        SetHint("Complete", "Open backpack is completed");
        notification.enableTimer = true;
        notification.Open();
        yield return new WaitForSeconds(3f);
        SetHint("Hint", "Open the second chest");
        notification.enableTimer = false;
        notification.Open();
        rangechest.interactable.interactable = true;
    }

    IEnumerator delaymeleechestOpened()
    {
        notification.Close();
        yield return new WaitForSeconds(1f);
        SetHint("Complete", "Open melee chest is completed!");
        notification.enableTimer = true;
        notification.Open();
        yield return new WaitForSeconds(3f);
        SetHint("Hint", "Use 'F' to open backpack \n and equip the sword \n use 'Alpha 1' to select melee weapon \n then use 'Mouse 0' to break crates");
        notification.enableTimer = false;
        notification.Open();
    }

    IEnumerator delayrangedchestOpened()
    {
        notification.Close();
        yield return new WaitForSeconds(1f);
        SetHint("Complete", "Open ranged chest is completed!");
        notification.enableTimer = true;
        notification.Open();
        yield return new WaitForSeconds(3f);
        SetHint("Hint", "Use 'F' to open backpack \n and equip the bow \n use 'Alpha 2' to select ranged weapon \n then Shoot All pots under the water");
        notification.enableTimer = false;
        notification.Open();
    }

    IEnumerator delayPotAllDestroy()
    {
        notification.Close();
        yield return new WaitForSeconds(1f);
        SetHint("Complete", "All pots have been destory!");
        notification.enableTimer = true;
        notification.Open();
        yield return new WaitForSeconds(3f);
        SetHint("Hint", "Open the third chest");
        notification.enableTimer = false;
        notification.Open();
        potionchest.interactable.interactable = true;
    }

    IEnumerator delaypotionChestOpened()
    {
        notification.Close();
        yield return new WaitForSeconds(1f);
        SetHint("Complete", "Open potion chest is completed!");
        notification.enableTimer = true;
        notification.Open();
        yield return new WaitForSeconds(3f);
        SetHint("Hint", "open backpack and equip the potions \n use 'Alpha 3' to consume Potion \n then Click Mouse 0 to attack/kill the monster ahead");
        notification.enableTimer = false;
        notification.Open();
        spawner.GetComponent<SpawnerController>().SpawnMobs();
        Monster = GameObject.FindWithTag("Enemy");
    }
    
}
