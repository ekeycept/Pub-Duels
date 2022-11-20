using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int health;

    [SerializeField] private CharacterController2D controller;
    private float directionMove;
    [SerializeField] private float speed = 40f;
    private bool jump = false;
    [SerializeField] private Animator animator;
    private bool isRunning = false;
    public SpriteRenderer sr;

    [SerializeField] private FieldOfView fieldOfView;

    void Start()
    {
        Cursor.visible = false;
        controller = GetComponent<CharacterController2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        Vector3 targetPos = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDir = (targetPos - transform.position);
        fieldOfView.SetAimDirection(aimDir);

        directionMove = Input.GetAxisRaw("Horizontal") * speed;

        if (directionMove != 0)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.H))
            GetDamage(2);
    }

    private void FixedUpdate()
    {
        controller.Move(directionMove * Time.deltaTime, false, jump);
        jump = false;
        animator.SetBool("isJumping", false);
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        StartCoroutine(ColorChange());

        if (health <= 0)
            Destroy(gameObject);
    }

    IEnumerator ColorChange()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(.7f);
        sr.color = Color.white;
    }
    
}

