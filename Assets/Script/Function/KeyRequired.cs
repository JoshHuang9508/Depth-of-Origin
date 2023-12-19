using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRequired : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private string keyName;

    public bool HaveKey()
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        bool haveKey = false;
        int indexOfKeyList = -1;

        foreach (var key in player.keyList)
        {
            if (key.key.Name == keyName)
            {
                haveKey = true;
                indexOfKeyList = player.keyList.IndexOf(key);
            }
        }
        if (haveKey)
        {
            player.keyList[indexOfKeyList].quantity--;
        }

        return haveKey;
    }
}
