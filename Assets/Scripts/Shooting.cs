using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject playerBullet;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public GameObject flash;
    public float bulletSpawnTime = 0.5f;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        flash.SetActive(false);
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
            //Instantiate(playerBullet, spawnPointLeft.position, Quaternion.identity);
            //Instantiate(playerBullet, spawnPointRight.position, Quaternion.identity);
        //}
    }

    void Fire()
    {
        Instantiate(playerBullet, spawnPointLeft.position, Quaternion.identity);
        Instantiate(playerBullet, spawnPointRight.position, Quaternion.identity);
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(bulletSpawnTime);
            Fire();
            audioSource.Play();
            flash.SetActive(true);
            yield return new WaitForSeconds(0.04f);
            flash.SetActive(false);
        }
        //StartCoroutine(Shoot());
    }
}
