using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject explosion;
    public PlayerHealthBar playerHealthbar;
    public GameObject playerDamageEffect;
    public CoinCount coinCountScript;
    public GameController gameController;
    //public float speed = 10f;
    //public float paddingX = 0.9f;
    //public float paddingY = 0.6f;
    /*float minX;
    float maxX;
    float minY;
    float maxY;*/

    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip damageSoundByRock;
    public AudioClip explosionSound;
    public AudioClip coinSound;

    public float health = 20f;
    //public float enemyBulletPower = 4f;
    float barFillAmount = 1f;
    float damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        //FindBoundaries();
        damage = barFillAmount / health;
    }

    /*void FindBoundaries ()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingX;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingX;

        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingY;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingY;
    }*/

    // Update is called once per frame
    void Update()
    {        
        //float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        //float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        //float newXpos = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
        //float newYpos = Mathf.Clamp(transform.position.y + deltaY, minY, maxY);

        //transform.position = new Vector2(newXpos, newYpos);

        if (Input.GetMouseButton(0))
        {
            Vector2 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = Vector2.Lerp(transform.position, newPos, 10 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            audioSource.PlayOneShot(damageSound, 0.5f);
            TakeDamage(1); // Deal 1 damage for enemy bullets
            Destroy(collision.gameObject);
            GameObject playerDamageVFX = Instantiate(playerDamageEffect, collision.transform.position, Quaternion.identity);
            Destroy(playerDamageVFX, 0.05f);
        }
        else if (collision.gameObject.tag == "Coin")
        {
            audioSource.PlayOneShot(coinSound, 0.5f);
            Destroy(collision.gameObject);
            coinCountScript.AddCount();
        }
        else if (collision.gameObject.tag == "Rock")
        {
            audioSource.PlayOneShot(damageSoundByRock, 0.5f);
            TakeDamage(4); // Deal 4 damage for rocks
            Destroy(collision.gameObject);
            GameObject playerDamageVFX = Instantiate(playerDamageEffect, collision.transform.position, Quaternion.identity);
            Destroy(playerDamageVFX, 0.05f);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            ExplodeAndGameOver();
        }
        else if (collision.gameObject.tag == "Laser")
        {
            // The laser beam has hit the player, but no damage is taken
            Debug.Log("Laser beam hit the player!");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        barFillAmount = barFillAmount - (damage * damageAmount);
        playerHealthbar.SetAmount(barFillAmount);

        if (health <= 0)
        {
            ExplodeAndGameOver();
        }
    }

    private void ExplodeAndGameOver()
    {
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.5f);
        gameController.GameOver();
        Destroy(gameObject);
        GameObject blast = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(blast, 2f);
    }
}
