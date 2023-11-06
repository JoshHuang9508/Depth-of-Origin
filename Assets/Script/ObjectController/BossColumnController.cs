using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossColumnController : MonoBehaviour
{
    [SerializeField] public System.Action shieldBreak;
    [SerializeField] public static int totalCoulumnAmount = 6;

    public void Reset()
    {
        totalCoulumnAmount = 6;
    }

    private void OnDisable()
    {
        totalCoulumnAmount--;
        if(totalCoulumnAmount <= 0)
        {
            shieldBreak.Invoke();
        }
    }
}
