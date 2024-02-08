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
    [SerializeField] private SpawnerController spawner;
    [SerializeField] private ChestController meleeChestController, rangeChestController, potionChestController;
    [SerializeField] private List<GameObject> potList, crateList;
    [SerializeField] private int progressNum;


    void Start()
    {
        //setup notification
        notification.Close();
        notification.title = "";
        notification.description = "";
        notification.enableTimer = false;

        //setup chest controller
        meleeChestController.interactable.interactable = true;
        rangeChestController.interactable.interactable = false;
        potionChestController.interactable.interactable = false;

        progressNum = 0;

        
    }

    void Update()
    {
        notification.UpdateUI();

        switch (progressNum)
        {
            case 0:
                if (!meleeChestController.isOpen)
                {
                    SetHint("Hint", "Open the chest");
                    notification.Open();
                }
                else
                {
                    notification.Close();
                    SetHint("Complete", "Chest has been opened!");
                    notification.Open();

                    progressNum = 1;
                }
                break;

            case 1:
                if (!crateList.All(item => item == null))
                {
                    SetHint("Hint", "Press 'F' to open backpack and equip the sword. \n Press 'Alpha 1' to select melee weapon then use 'Mouse 0' to swing the sword.");
                    notification.Open();
                }
                else
                {
                    notification.Close();
                    SetHint("Complete", "The crate has been broken!");
                    notification.Open();

                    progressNum = 2;
                }
                break;

            case 2:
                if (!rangeChestController.isOpen)
                {
                    SetHint("Hint", "Open the chest");
                    notification.Open();

                    rangeChestController.interactable.interactable = true;
                }
                else
                {
                    notification.Close();
                    SetHint("Complete", "Chest has been opened!");
                    notification.Open();

                    progressNum = 3;
                }
                break;

            case 3:
                if (!potList.All(item => item == null))
                {
                    SetHint("Hint", "Press 'F' to open backpack and equip the bow \n Press 'Alpha 2' to select ranged weapon then use 'Mouse 0' to shot the pots in the water");
                    notification.Open();
                }
                else
                {
                    notification.Close();
                    SetHint("Complete", "The pot has been broken!");
                    notification.Open();

                    progressNum = 4;
                }
                break;

            case 4:
                if (!potionChestController.isOpen)
                {
                    SetHint("Hint", "Open the chest");
                    notification.Open();

                    potionChestController.interactable.interactable = true;
                }
                else
                {
                    notification.Close();
                    SetHint("Complete", "Chest has been opened!");
                    notification.Open();

                    progressNum = 5;
                }
                break;

            case 5:
                spawner.SpawnMobs();
                GameObject enemy = GameObject.FindWithTag("Enemy");
                bool enemyAlive = true;
                try
                {
                    if (enemy != null) enemyAlive = true;
                    else enemyAlive = false;
                }
                catch { }

                if (enemyAlive)
                {
                    SetHint("Hint", "Press 'F' to open backpack and equip the potions \n Press 'Alpha 3' to consume Potion then try to kill the enemy ahead");
                    notification.Open();
                }
                else
                {
                    notification.Close();
                    SetHint("Complete", "Enemy has been defeated!");
                    notification.Open();

                    progressNum = 6;
                }
                break;

            case 6:
                SetHint("Hint", "Walk forward to the forest");
                notification.enableTimer = false;
                notification.Open();
                break;
        }
    }

    private void SetHint(string title, string description)
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

    public void enterForest()
    {
        PlayerPrefs.SetString("loadscene", "Town");
    }

    IEnumerator missionComplete(string completeMsg)
    {
        notification.Close();

        yield return new WaitForSeconds(1f);

        SetHint("Complete", completeMsg);
        notification.enableTimer = true;
        notification.Open();
    }
}
