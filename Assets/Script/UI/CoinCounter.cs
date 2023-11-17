using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    [Header("Connect Object")]
    public TMP_Text coinCounterText;

    PlayerBehaviour player;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        coinCounterText.text = $"x{player.currentCoinAmount}";
    }
}
