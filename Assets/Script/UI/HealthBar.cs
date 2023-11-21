using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("Connect Object")]
    public Slider slider;
    public TMP_Text healthText;
    public Gradient gradient;
    public Image fill;

    PlayerBehaviour player;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        slider.value = player.currentHealth / player.maxHealth;
        healthText.text = $"{Mathf.RoundToInt(player.currentHealth)} / {player.maxHealth}";
        fill.color = gradient.Evaluate(player.currentHealth / player.maxHealth);
    }
}
