using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;

    private void Start()
    {
        Invoke("DestroyBulletServerRpc", lifetime);
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.position, distance, whatIsSolid);
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
        GetComponent<NetworkObject>().Despawn(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable == null)
                DestroyBulletServerRpc();
            else
                damageable.GetDamage(damage);
        }
        else
            DestroyBulletServerRpc();
    }

    [ServerRpc]
    private void DestroyBulletServerRpc()
    {
        Destroy(gameObject);
        GetComponent<NetworkObject>().Despawn(true);
    }
}