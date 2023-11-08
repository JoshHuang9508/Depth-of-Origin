using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public Vector3 moveVec;
    public Rigidbody2D currentRb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        currentRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(MoveRoutine());
    }

    void Update()
    {
        currentRb.velocity = moveVec;

        animator.SetFloat("Horizontal", moveVec.x);
        animator.SetFloat("Vertical", moveVec.y);
        spriteRenderer.flipX = moveVec.x < 0;
    }

    private void Move()
    {
        int horizontalVec = Random.Range(-3, 3);
        int verticalVec = Random.Range(-3, 3);

        moveVec = new Vector3(horizontalVec, verticalVec, 0);
    }

    private void Stop()
    {
        moveVec = new Vector3(0, 0, 0);
    }

    private IEnumerator MoveRoutine()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        Move();
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        Stop();
        StartCoroutine(MoveRoutine());
    }
}
