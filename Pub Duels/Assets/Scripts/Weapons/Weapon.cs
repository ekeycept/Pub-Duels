using Unity.Netcode;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    [SerializeField] protected int MaxShotsNumber = 6;
    [SerializeField] protected float offset;
    [SerializeField] protected float timeForReloading;
    [SerializeField] protected float startTimeForReloading;
    [SerializeField] protected float timeBtwShots;
    [SerializeField] protected float startTimeBtwShots;
    [SerializeField] protected GameObject Bullet;
    [SerializeField] protected GameObject Player;
    [SerializeField] protected Transform shotPoint;
    [SerializeField] protected AudioSource ShotSound;
    [SerializeField] protected Animator animator;
    protected float ShotsNumber = 0;
    protected bool facingright = true;
    protected SpriteRenderer mySpriteRenderer;
    private bool IsWeaponOwner;
    /*    [SerializeField] private Joystick joystickFire;*/

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (gameObject != null)
        {
            IsWeaponOwner = transform.parent.gameObject.GetComponent<NetworkObject>().IsOwner;
            if (!IsWeaponOwner) return;
            RotateWeapon();
            Shoot();
            FlipWeapon();
        }
    }


    protected void RotateWeapon()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        /*        Vector3 direction = new Vector3(joystickFire.Horizontal, -joystickFire.Vertical);
                Vector3 aa = new Vector3(0, 0, 0);
                Vector3 diff = aa - direction;
                diff.Normalize();
                float rotation = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotation + 90);*/

    }

    protected virtual void Shoot()
    {
        if (timeBtwShots <= 0 && timeForReloading <= 0)
        {
            if (Input.GetMouseButton(0) && ShotsNumber < MaxShotsNumber)
            {
                BulletSpawnServerRpc();
                timeBtwShots = startTimeBtwShots;
                ShotSound.Play();
                ShotsNumber++;
                animator.SetTrigger("Shooting");
            }
            else if (ShotsNumber >= MaxShotsNumber || Input.GetKeyDown("r"))
            {
                Reload();
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
            timeForReloading -= Time.deltaTime;
        }

        /*        if (timeBtwShots <= 0 && timeForReloading <= 0)
                {
                    if ((joystickFire.Horizontal != 0 || joystickFire.Vertical != 0) && ShotsNumber < MaxShotsNumber)
                    {
                        Instantiate(Bullet, shotPoint.position, transform.rotation);
                        timeBtwShots = startTimeBtwShots;
                        ShotSound.Play();
                        ShotsNumber++;
                    }
                    else if (ShotsNumber >= MaxShotsNumber || Input.GetKeyDown("r"))
                    {
                        Reload();
                    }
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                    timeForReloading -= Time.deltaTime;
                }*/
    }

    protected void Reload()
    {
        ShotsNumber = 0;
        Debug.Log("Reloading...");
        timeForReloading = startTimeForReloading;
    }
    protected void FlipWeapon()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!facingright && mousePos.x > transform.position.x || facingright && mousePos.x < transform.position.x)
        {
            facingright = !facingright;
            Vector3 theScale = transform.localScale;
            theScale.y *= -1;
            transform.localScale = theScale;
        }

        if (Player.GetComponent<SpriteRenderer>().flipX == true)
        {
            Vector3 theScale = transform.localScale;
            theScale.y *= -1;
            transform.localScale = theScale;
        }

        /*        if (!facingright && joystickFire.Horizontal > 0 || facingright && joystickFire.Horizontal < 0)
                {
                    facingright = !facingright;
                    mySpriteRenderer.flipY = !mySpriteRenderer.flipY;
                }*/
    }

    [ServerRpc]
    private void BulletSpawnServerRpc()
    {
        GameObject bullet = Instantiate(Bullet, shotPoint.position, transform.rotation);
        bullet.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log("Bullet has been instatiate!");
    }

    [ServerRpc]
    private void RotateWeaponServerRpc()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }
}
