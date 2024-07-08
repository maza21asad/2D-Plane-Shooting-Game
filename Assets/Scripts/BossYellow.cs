using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossYellow : MonoBehaviour
{
    public Transform[] gunPoint;
    public GameObject bossBullet;
    public GameObject enemyFlash;
    public GameObject enemyExplosionPrefab;
    public HealthBar healthBar;
    public GameObject damageEffect;
    public float speed = 1f;
    public float health = 50f;

    public AudioClip bulletSound;
    public AudioClip damageSound;
    public AudioClip explosionSound;
    public AudioSource audioSource;

    float barSize = 1f;
    float damage = 0;

    public float enemyBulletSpawnTime = 0.5f;

    private bool hasReachedTargetPosition = false;
    private Animator animator;

    public GameObject laserPrefab;
    private List<GameObject> laserBeams = new List<GameObject>();

    public float laserDuration = 5f;
    private bool isFiringLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyFlash.SetActive(false);
        StartCoroutine(BossShoot());
        damage = barSize / health;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasReachedTargetPosition)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);

            if (transform.position.y <= 4)
            {
                transform.position = new Vector2(0, 4);
                hasReachedTargetPosition = true;
                EnableAnimation();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position, 0.1f);
            DamageHealthBar();
            Destroy(collision.gameObject);
            GameObject damageVFX = Instantiate(damageEffect, collision.transform.position, Quaternion.identity);
            Destroy(damageVFX, 0.05f);
            if (health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.5f);
                Destroy(gameObject);
                GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.4f);
                DisableLaserBeams();
            }
        }
    }
    void DamageHealthBar()
    {
        if (health > 0)
        {
            health -= 1;
            barSize = barSize - damage;
            healthBar.SetSize(barSize);
        }
    }

    void BossFire()
    {
        for (int i = 0; i < gunPoint.Length -2; i++)
        {
            Instantiate(bossBullet, gunPoint[i].position, Quaternion.identity);
        }
    }

    void BossLaser()
    {
        for (int i = gunPoint.Length - 2; i < gunPoint.Length; i++)
        {
            StartCoroutine(FireLaserFromGunPoint(gunPoint[i]));
        }
    }
    IEnumerator BossShoot()
    {
        while (true)
        {
            if (isFiringLaser)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(enemyBulletSpawnTime);
                BossFire();
                audioSource.PlayOneShot(bulletSound, 0.5f);
                enemyFlash.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                enemyFlash.SetActive(false);

                if (health <= 25)
                {
                    StartCoroutine(FireLaserBeam());
                }
            }
        } 
    }

    IEnumerator FireLaserBeam()
    {
        isFiringLaser = true;
        BossLaser();
        yield return new WaitForSeconds(laserDuration);
        isFiringLaser = false;
    }

    IEnumerator FireLaserFromGunPoint(Transform gunPoint)
    {
        GameObject laser = Instantiate(laserPrefab, gunPoint.position, Quaternion.identity);
        laserBeams.Add(laser);
        LineRenderer laserRenderer = laser.GetComponent<LineRenderer>();
        BoxCollider2D laserCollider = laser.GetComponent<BoxCollider2D>();
        laserRenderer.enabled = true;

        //Vector3 startPosition = gunPoint.position;

        while (isFiringLaser)
        {
            if (laserRenderer != null)
            {
                Vector3 startPosition = gunPoint.position;
                RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down);

                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    laserRenderer.SetPosition(0, startPosition);
                    laserRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    laserRenderer.SetPosition(0, startPosition);
                    laserRenderer.SetPosition(1, startPosition + Vector3.down * 100f);
                }
            }
            yield return null;
        }
        if (laserRenderer != null)
        {
            laserRenderer.enabled = false;
        }
    }

    void DisableLaserBeams()
    {
        foreach (var laser in laserBeams)
        {
            if (laser != null)
            {
                Destroy(laser);
            }
        }
        laserBeams.Clear();
    }

    void EnableAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("StartAnimation");
        }
    }
}
