using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] float paddingTop = 5f;
    [SerializeField] int health = 300;
    [SerializeField] AudioClip destroySfx;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileFiringPeriod = 20f;

     
    private bool isFiring = false;
    private float xMin,xMax,yMin,yMax;


    void Start()
    {
        SetUpBoundaries();
        FindObjectOfType<GameSession>().SetHealth(health);
    }

    void Update()
    {
        Move();
        Shoot();
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Layer collision matrix fix coliisions
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        FindObjectOfType<GameSession>().HealthValue(damageDealer.GetDamage());

        damageDealer.Hit();
        if(health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        
        //this clip will play at the camera position contrary to the others, because why not?
        AudioSource.PlayClipAtPoint(destroySfx, Camera.main.transform.position, 0.6f);
        Destroy(gameObject);
        FindObjectOfType<Level>().LoadGameOver();

    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && !isFiring)
        {
            isFiring = true;
            StartCoroutine(FireContinuously());
        }else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(FireContinuously());
            isFiring = false;
        }
    }

    IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            yield return new WaitForSeconds(projectileFiringPeriod * Time.deltaTime);
        }
    }
   
    private void Move()
    {
        //to move frame rate independent we use Time.deltaTime
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpBoundaries()
    {
        //ViewportToWorldPoint converts the position of something relative to camera view
        //world space boundaries betweent 0,1 for x and y coordinates
        Camera gameCamera = Camera.main;
        //using padding variables to limit players movement
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingTop;
    }
}
