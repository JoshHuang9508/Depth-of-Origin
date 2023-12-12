using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Behavior : MonoBehaviour
{

    [Header("Object Reference")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    public Rigidbody2D currentRb;
    [SerializeField] private GameObject damageText;
    [SerializeField] private GameObject itemDropper;

    [Header("Audio")]
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deadSound;

    [Header("Object Reference")]
    [SerializeField] private EnemyBehavior enemybehavior;

    [Header("Dynamic Data")]
    public EnemySO enemy;
    [SerializeField] private GameObject target;
    public float currentHealth;
    [SerializeField] private Vector2 currentPos, targetPos, diraction;

    int behaviorType = 1;

    private void Start()
    {
        
    }
    private void Update()
    {
        switch(behaviorType)
        {
            case 1:

                enemybehavior.movementDisableTimer = enemybehavior.movementDisableTimer < 5 ? 1000 : 0;
                enemybehavior.currentRb.bodyType = RigidbodyType2D.Static;
                enemybehavior.enemy.attackType = EnemySO.AttackType.Sniper;
                enemybehavior.enemy.attackField = 100;
                enemybehavior.enemy.chaseField = 100;
                enemybehavior.enemy.attackSpeed = 1f;
                enemybehavior.enemy.attackDamage = 1500;
                break;
            case 2:
                break;
        }
    }
}
