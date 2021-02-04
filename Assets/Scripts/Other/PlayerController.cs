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
    private CharacterController characterController;
    private GameManager gm;
    private UI_manager ui;
    private AudioManager am;

    [SerializeField] public static float speed = 5f;               // уточнить
    [SerializeField] private float strafeSpeed = 6;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float maxSpeed = 20;
    
    public event Action onGameOver;

    [SerializeField] private int health = 3;
    [SerializeField] private int bulletAmount = 3;
    private bool canShoot = true;

    private Transform generatedBullets;
    
    private static readonly int Blend = Animator.StringToHash("Blend");

    private WeaponManager weaponManager;
    private Transform gunHolder;

    private void Awake()
    {
        CreateBulletsContainer();

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ui = GameObject.FindGameObjectWithTag("UI_manager").GetComponent<UI_manager>();
        am = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>();

        gunHolder = GameObject.Find("GunHolder").transform;
        
        weaponManager = new WeaponManager(gunHolder);
        weaponManager.Init();
    }

    private void Start()
    {
        onGameOver += gm.OnGameOver;
        onGameOver += StartDeathRoutine;
        characterController = gameObject.GetComponent<CharacterController>();
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
        
        weaponManager.OnUpdate();
        
        float horizontalInput = Input.GetAxis("Horizontal");

        ProcessAnimation(horizontalInput);
        ProcessShoot();

        SpeedControl();

        Move(horizontalInput);
    }

    private void ProcessShoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (canShoot && bulletAmount > 0)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private void ProcessAnimation(float horizontalInput)
    {
        animator.SetFloat(Blend, horizontalInput);
        
        CheckForAwesomeTrigger();
    }


    private void CreateBulletsContainer()
    {
        generatedBullets = new GameObject("generatedBullets").transform;
        generatedBullets.SetParent(FindObjectOfType<WorldBuilder>().transform);
    }

    private void SpeedControl()
    {
        speed += acceleration * Time.deltaTime;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }

    private IEnumerator Shoot()
    {
        //InstantiateBullet();

        weaponManager.Shoot();
        
        bulletAmount--;
        ui.UpdateBulletstext(bulletAmount);
        
        canShoot = false;
        yield return new WaitForSeconds(0.3f);
        //yield return null;
        canShoot = true;
    }

    // private void InstantiateBullet()
    // {
    //     Bullet bulletScript = PoolManager.Get(PoolType.Bullets).GetComponent<Bullet>();
    //     bulletScript.playerVelocity = speed;
    //     
    //     GameObject bullet = bulletScript.gameObject;
    //     SetBulletSettings(bullet);
    // }

    // private void SetBulletSettings(GameObject bullet)
    // {
    //     bullet.SetActive(true);
    //     bullet.transform.SetParent(generatedBullets);
    //     bullet.transform.position = gameObject.transform.position + transform.forward;
    //     bullet.transform.rotation = gameObject.transform.rotation;
    // }

    private void Move(float horizontalInput)
    {
        Vector3 moveDir = transform.right * horizontalInput * strafeSpeed + transform.forward * speed;
        characterController.Move(moveDir * Time.deltaTime);

        gravity.y += gravityAmount * Time.deltaTime;
        characterController.Move(gravity * Time.deltaTime);
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (health > 0)
            {
                health--;
                ui.UpdateHealttext(health);
                PoolManager.Return(other.gameObject.GetComponent<PoolItem>());
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
            weaponManager.SwitchWeapon("RPG7", 5f);

            bulletAmount++;
            
            // if (bulletAmount > 30) 
            //     bulletAmount = 30;
            
            ui.UpdateBulletstext(bulletAmount);
            PoolManager.Return(other.gameObject.GetComponent<PoolItem>());
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
