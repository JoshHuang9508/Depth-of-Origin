using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRequired : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private string keyName;
    [SerializeField] private List<string> keyNames;

    public bool HaveKey()
    {
        PlayerBehaviour player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();

        bool haveKey = false;

        if(keyName != "")
        {
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
        }
        else if(keyNames.Count != 0)
        {
            List<int> indexOfKeyList = new(); 

            foreach(string keyName in keyNames)
            {
                foreach(var key in player.keyList)
                {
                    if(key.key.Name == keyName)
                    {
                        indexOfKeyList.Add(player.keyList.IndexOf(key));
                    }
                }
                if(indexOfKeyList.Count == keyNames.Count)
                {
                    haveKey = true;
                }
            }
            if (haveKey)
            {
                foreach(int index in indexOfKeyList)
                {
                    player.keyList[index].quantity--;
                }
            }
        }

        return haveKey;
    }
}
