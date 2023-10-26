using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text healthText;
    PlayerBehaviour player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.currentHealth / player.maxHealth;
        healthText.text = $"{player.currentHealth} / {player.maxHealth}";
    }
}
