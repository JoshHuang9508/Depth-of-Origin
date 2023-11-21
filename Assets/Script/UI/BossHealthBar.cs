using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    [Header("Connect Object")]
    public EnemyBehavior boss;
    public Slider slider;
    public TMP_Text healthText, bossName;
    public Gradient gradient;
    public Image fill;

    void Update()
    {
        try
        {
            boss = GameObject.FindWithTag("Boss").GetComponent<EnemyBehavior>();
        }
        catch
        {

        }

        if(boss != null)
        {
            setVisiable(true);

            bossName.text = boss.enemy.Name;
            slider.value = boss.currentHealth / boss.enemy.health;
            healthText.text = $"{Mathf.RoundToInt(boss.currentHealth)} / {boss.enemy.health}";
            fill.color = gradient.Evaluate(boss.currentHealth / boss.enemy.health);
        }
        else
        {
            setVisiable(false);
        }
    }

    private void setVisiable(bool Visiable)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(Visiable);
        }
    }
}
