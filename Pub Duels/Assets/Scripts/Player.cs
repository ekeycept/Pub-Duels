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

    [SerializeField] private FieldOfView fieldOfView;

    void Start()
    {
        Cursor.visible = false;
        controller = GetComponent<CharacterController2D>();
    }
    
    void Update()
    {
        Vector3 targetPos = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDir = (targetPos - transform.position);
        fieldOfView.SetAimDirection(aimDir);

        directionMove = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump"))
            jump = true;
    }

    private void FixedUpdate()
    {
        controller.Move(directionMove * Time.deltaTime, false, jump);
        jump = false;
    }

    public void GetDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
    }    
}
