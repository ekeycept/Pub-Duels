using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int health;

    [SerializeField] private CharacterController2D controller;
    private float directionMove;
    [SerializeField] private float speed = 40f;
    private bool jump = false;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }
    
    void Update()
    {
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
