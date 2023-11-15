using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public BossBehavior bossObject;
    public Slider slider;
    public TMP_Text healthText, bossName;
    public Gradient gradient;
    public Image fill;


    void Update()
    {
        try
        {
            bossObject = GameObject.FindWithTag("Boss").GetComponent<BossBehavior>();
        }
        catch
        {

        }

        if(bossObject != null)
        {
            setVisiable(true);

            bossName.text = bossObject.enemy.Name;
            slider.value = bossObject.currentHealth / bossObject.enemy.health;
            healthText.text = $"{Mathf.RoundToInt(bossObject.currentHealth)} / {bossObject.enemy.health}";
            fill.color = gradient.Evaluate(bossObject.currentHealth / bossObject.enemy.health);
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
