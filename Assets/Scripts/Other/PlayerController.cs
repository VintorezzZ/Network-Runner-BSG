using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerController : MonoBehaviour
{
    public static bool canMove = true;
    
    private bool isStrafing;

    private float gravityAmount = -20;
    private Vector3 gravity;

    private Animator animator;
    private CharacterController controller;
    private GameManager gm;
    private UI_manager ui;
    private AudioManager am;

    [SerializeField] public float speed = 5f;
    [SerializeField] private float strafeSpeed = 6;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float maxSpeed = 20;
    
    public Action onGameOver;

    private int health = 3;
    private int bulletAmount = 3;
    private bool canShoot = true;
    [SerializeField] GameObject bulletPrefab;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ui = GameObject.FindGameObjectWithTag("UI_manager").GetComponent<UI_manager>();
        am = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>();
    }

    private void Start()
    {
        onGameOver += gm.OnGameOver;
        onGameOver += StartDeathRoutine;
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();
        
        ui.UpdateBulletstext(bulletAmount);
        ui.UpdateHealttext(health);
        
        canMove = false;
    }

    private void Update()
    {
        if (transform.position.y < -4)
        {
            onGameOver?.Invoke();
        }
        
        if (!canMove)
            return;
        
        if (Input.GetKeyDown(KeyCode.A) | Input.GetKeyDown(KeyCode.LeftArrow) | SwipeManager.swipeLeft)
        {
            CheckForAwesomeTrigger();

            if(!isStrafing)
                StartCoroutine(Strafe());
        }
        else if (Input.GetKeyDown(KeyCode.D) | Input.GetKeyDown(KeyCode.RightArrow) | SwipeManager.swipeRight)
        {
            CheckForAwesomeTrigger();

            if(!isStrafing)
                StartCoroutine(Strafe());
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot && bulletAmount > 0) 
            {
                StartCoroutine(Shoot());
            }
            
        }
        
        speed += acceleration * Time.deltaTime;
        if (speed > maxSpeed)
            speed = maxSpeed;
        
        Move();
    }

    private IEnumerator Shoot()
    {
        Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);
        bulletAmount--;
        ui.UpdateBulletstext(bulletAmount);
        
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");

        Vector3 moveDir = transform.right * x * strafeSpeed + transform.forward * speed;
        controller.Move(moveDir * Time.deltaTime);

        gravity.y += gravityAmount * Time.deltaTime;
        controller.Move(gravity * Time.deltaTime);
    }

    private void CheckForAwesomeTrigger()
    {
        RaycastHit[] hits;
        Ray ray = new Ray(transform.position, Vector3.forward);
        //Debug.DrawRay(transform.position, Vector3.forward * 3, Color.red, 1);
        hits = Physics.RaycastAll(ray, 3);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("AwesomeTrigger"))
            {
                StartCoroutine(ui.ShowAwesomeText());
                AddCoins(50);
            }
        }
    }

    private void AddCoins(int amount)
    {
        gm.score += amount;
    }

    private void StartDeathRoutine()
    {
        canMove = false;
    }

    IEnumerator Strafe()
    {
        isStrafing = true;
        am.PlayTransitionSFX();
        animator.SetBool("isStrafing", true);
        //Invoke("AnimDelay", 0.3f);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isStrafing", false);
        isStrafing = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (health > 0)
            {
                health--;
                ui.UpdateHealttext(health);
                Destroy(other.gameObject);
            }
            else
            {
                onGameOver?.Invoke();
                animator.SetTrigger("dead");
                am.PlayLoseSFX();
            }
            
        }

        if (other.CompareTag("Bullet"))
        {
            bulletAmount++;
            
            if (bulletAmount > 30) 
                bulletAmount = 30;
            
            ui.UpdateBulletstext(bulletAmount);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Cross left"))
        {
            transform.localRotation *= Quaternion.Euler(0,  -90, 0);
        }
        else if (other.gameObject.CompareTag("Cross right"))
        {
            transform.localRotation *= Quaternion.Euler(0,   90, 0);
        }
    }
}
