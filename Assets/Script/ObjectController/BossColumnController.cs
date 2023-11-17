using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class BossColumnController : MonoBehaviour
{
    [Header("Settings")]
    public int totalColumnAmount;

    [Header("Status")]
    public static int currentCoulumnAmount;

    public Action shieldBreak;

    public void Reset()
    {
        currentCoulumnAmount = totalColumnAmount;
    }

    private void Start()
    {
        Reset();
    }

    private void OnDisable()
    {
        currentCoulumnAmount--;
        if(currentCoulumnAmount <= 0)
        {
            shieldBreak.Invoke();
        }
    }
}
