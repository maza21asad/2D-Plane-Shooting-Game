using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform[] gunPoint;
    public GameObject enemyBullet;
    public GameObject enemyFlash;
    public GameObject enemyExplosionPrefab;
    public HealthBar healthBar;
    public GameObject damageEffect;
    public GameObject coinPrefab;
    public float speed = 1f;
    public float health = 10f;
    public float bulletPower = 1f;

    public AudioClip bulletSound;
    public AudioClip damageSound;
    public AudioClip explosionSound;
    public AudioSource audioSource;

    float barSize = 1f;
    float damage = 0;

    public float enemyBulletSpawnTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        enemyFlash.SetActive(false);
        StartCoroutine(EnemyShoot());
        damage = barSize / health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            //audioSource.PlayOneShot(damageSound);
            AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position, 0.1f);
            DamageHealthBar();
            Destroy(collision.gameObject);
            GameObject damageVFX = Instantiate(damageEffect, collision.transform.position, Quaternion.identity);
            Destroy(damageVFX, 0.05f);
            if (health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.5f);
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.4f);
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            ZeroHealthBar();
            if (health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.5f);
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.4f);
            }
        }
    }

    void DamageHealthBar()
    {
        if(health>0)
        {
            health -= bulletPower;
            barSize = barSize - damage;
            healthBar.SetSize(barSize);
        }
    }

    void ZeroHealthBar()
    {
        if (health > 0)
        {
            /*health -= health;
            barSize = barSize - barSize;
            healthBar.SetSize(barSize);*/
            health = 0;
            barSize = 0;
            healthBar.SetSize(barSize);
        }
    }

    void EnemyFire()
    {
        for(int i =0; i < gunPoint.Length; i++)
        {
            Instantiate(enemyBullet, gunPoint[i].position, Quaternion.identity);
        }
    }
    IEnumerator EnemyShoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(enemyBulletSpawnTime);
            EnemyFire();
            audioSource.PlayOneShot(bulletSound, 0.5f);
            enemyFlash.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            enemyFlash.SetActive(false);
        }
    }
}
