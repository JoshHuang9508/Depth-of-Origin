using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthWarning : MonoBehaviour
{
    [Header("Setting")]
    public Gradient gradient;
    public Image fill;

    PlayerBehaviour player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        fill.color = gradient.Evaluate(player.currentHealth / player.maxHealth);
    }
}
