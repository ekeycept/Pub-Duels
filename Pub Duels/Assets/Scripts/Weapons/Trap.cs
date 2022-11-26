using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private bool isClosed;
    private float trapTime = 5f;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && isClosed == false)
        {
            collision.gameObject.GetComponent<Player>().GetTrapped(trapTime);
            isClosed = true;
            animator.SetBool("isClosed", true);
        }

    }
}
