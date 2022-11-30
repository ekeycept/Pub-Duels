using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.Netcode;

public class Player : NetworkBehaviour, IDamageable
{
    [SerializeField] private int health;

    [SerializeField] private CharacterController2D controller;
    private float directionMove;
    [SerializeField] private float speed = 40f;
    private bool jump = false;
    [SerializeField] private Animator animator;
    public SpriteRenderer sr;
    [SerializeField] private GameObject effect;

    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private GameObject grenade;
    [SerializeField] private Transform grenadePos;
    [SerializeField] private float grenadeSpeed;
    [SerializeField] private GameObject Gun;

    private bool isMoveable = true;

    void Start()
    {
        if (!IsOwner)
        {
            return;
        }
            SpawnGunServerRpc();
        Cursor.visible = false;
        controller = GetComponent<CharacterController2D>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (!IsOwner) return;
        Vector3 targetPos = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDir = (targetPos - transform.position);
        fieldOfView.SetAimDirection(aimDir);

        if (isMoveable)
        {
            directionMove = Input.GetAxisRaw("Horizontal") * speed;
        }

        if (directionMove != 0)
        {
            animator.SetBool("isRunning", true);
        }

        else
            animator.SetBool("isRunning", false);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.H))
            GetDamage(2);

        if (Input.GetMouseButtonDown(1))
            Instantiate(grenade, grenadePos.position, transform.rotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        controller.Move(directionMove * Time.deltaTime, false, jump);
        jump = false;
        animator.SetBool("isJumping", false);
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        StartCoroutine(ColorChange());

        if (health <= 0)
        {
            if (!IsOwner) return;
            DestroyPlayerServerRpc();
        }
    }

    IEnumerator ColorChange()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(.7f);
        sr.color = Color.white;
    }

    public void GetTrapped(float trapTime)
    {
       //transform.position = trapPos.position;
        StartCoroutine(Trap(trapTime));
    }

    IEnumerator Trap(float trapTime)
    {
        isMoveable = false;
        directionMove = 0;
        yield return new WaitForSeconds(trapTime);
        isMoveable = true;
    }
    /*    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
                Instantiate(effect, transform.position, transform.rotation);
        }*/

    [ServerRpc]
    private void DestroyPlayerServerRpc()
    {
        GetComponent<NetworkObject>().Despawn(true);
        Destroy(gameObject);
    }

    [ServerRpc]
    private void SpawnGunServerRpc()
    {
        GameObject gun = Instantiate(Gun);
        gun.GetComponent<NetworkObject>().Spawn(true);
        gun.transform.SetParent(gameObject.transform);
        Debug.Log("Gun has been instatiate!");
    }
}

