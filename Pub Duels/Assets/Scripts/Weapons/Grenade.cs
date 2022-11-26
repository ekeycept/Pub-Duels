using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject effect;
    private Vector3 targetPos;
    private Rigidbody2D rb;
    private Vector3 dir;

    private void Awake()
    {
        targetPos = UtilsClass.GetMouseWorldPosition();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Boom());
    }

    private void Start()
    {
        Vector3 dir = transform.right + Vector3.up;
        rb.AddForce(dir);
    }

    void Update()
    {
        transform.position += targetPos * Time.deltaTime * speed;
        //transform.Translate(Vector2.MoveTowards(transform.position, targetPos, 100f) * speed * Time.deltaTime);
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(lifetime);
        Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
