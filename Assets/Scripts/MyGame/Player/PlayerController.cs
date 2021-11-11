using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using GameManager = Com.MyCompany.MyGame.GameManager;

public class PlayerController : MonoBehaviour
{
    #region Public Fields

    public bool canMove = true;
    public int score;

    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform rayCastPoint;

    #endregion

    #region Private Variables

    private Animator _animator;
    private CharacterController _characterController;
    private GameManager _gm;
    private AudioManager _am;

    private Vector3 _gravity;
    private float gravityAmount = -20;
    [SerializeField] public float speed = 5f;
    [SerializeField] private float strafeSpeed = 6;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private int startHealth = 3;
    [SerializeField] private int startBullets = 3;
    private int _health;
    private int _bullets;
    private Transform _generatedBullets;
    private WeaponManager _weaponManager;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private bool _canShoot = true;
    private float _horizontalInput;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    private bool groundedPlayer;

    #endregion

    public int Ammo
    {
        get => _bullets;

        set
        {
            _bullets += value;
            EventHub.OnBulletsChanged(_bullets);
        }
    }
    
    public int Health
    {
        get => _health;

        set
        {
            _health += value;
            EventHub.OnHealthChanged(_health);
        }
    }
    
    #region Private Methods

    #if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
    #endif

    #endregion

    #region MonoBehaviour Callbacks
    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

#if UNITY_5_4_OR_NEWER
    public void OnDisable()
    {
        // Always call the base to remove callbacks
        //base.OnDisable ();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
#endif
    
    #endregion
    
    private void Awake()
    {
        CreateBulletsContainer();
        SetManagers();
    }

    private void Start()
    {
        LocalPlayerInstance = gameObject;  // убрать 
        
        _characterController = gameObject.GetComponent<CharacterController>();
        _animator = gameObject.GetComponentInChildren<Animator>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        canMove = false;

        Health = startHealth;
        Ammo = startBullets;
    }


    private void Update()
    {
        groundedPlayer = _characterController.isGrounded;
            
        if (transform.position.y < -4)
            StartDeathRoutine();

        if (_health < 1f)
            GameManager.Instance.OnGameOver();

        if (!canMove)
            return;

        _weaponManager.OnUpdate();
        
        ProcessInputs();

        ProcessAnimation(_horizontalInput);

        SpeedControl();
        Move(_horizontalInput);
    }

    private void SetManagers()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _am = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>();
        _weaponManager = new WeaponManager(gunHolder, rayCastPoint);
        _weaponManager.Init();
    }

    private void ProcessInputs()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetMouseButtonDown(0))
        {
            if (_canShoot && _bullets > 0)
            {
                StartCoroutine(Shoot());
            }
        }
        
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _gravity.y += Mathf.Sqrt(0.8f * -3.0f * gravityAmount);
    }

    private void CreateBulletsContainer()
    {
        _generatedBullets = new GameObject("generatedBullets").transform;
        _generatedBullets.SetParent(FindObjectOfType<WorldBuilder>().transform);
    }
    
    private void ProcessShoot()
    {
        _weaponManager.Shoot();
    }

    private void ProcessAnimation(float horizontalInput)
    {
        _animator.SetFloat(Blend, horizontalInput);

        if (horizontalInput > 0.2f || horizontalInput < -0.2f)
        {
            CheckForAwesomeTrigger();
        }
    }


    private void SpeedControl()
    {
        speed += acceleration * Time.deltaTime;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }
    
    private IEnumerator Shoot()
    {
        _weaponManager.Shoot();

        Ammo--;

        _canShoot = false;
        yield return new WaitForSeconds(0.3f);
        _canShoot = true;
    }

    private void Move(float horizontalInput)
    {
        if (groundedPlayer && _gravity.y < 0)
        {
            _gravity.y = 0f;
        }
        
        Vector3 moveDir = transform.right * (horizontalInput * strafeSpeed) + transform.forward * speed;
        _characterController.Move(moveDir * Time.deltaTime);
        
        _gravity.y += gravityAmount * Time.deltaTime;
        _characterController.Move(_gravity * Time.deltaTime);
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
                //StartCoroutine(_ui.ShowAwesomeText());
                AddCoins(50);
            }
        }
    }

    private void AddCoins(int amount)
    {
        score += amount;
    }

    private void StartDeathRoutine()
    {
        canMove = false;
        _animator.SetTrigger("dead");
        _am.PlayLoseSFX();
        
        EventHub.OnGameOvered();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CheckForObstacle(other); 
        CheckForBulletBonus(other);
        CheckForCrossBends(other);
    }

    private void CheckForObstacle(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (Health >= 1)
            {
                Health--;
                
                PoolManager.Return(other.gameObject.GetComponentInParent<PoolItem>());
            }
            else
            {
                StartDeathRoutine();
            }
        }
    }

    private void CheckForBulletBonus(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            _weaponManager.SwitchWeapon("RPG7", 5f);

            Ammo++;

            if (Ammo > 30) 
                Ammo = 30;
            
            PoolManager.Return(other.gameObject.GetComponent<PoolItem>());
        }
    }

    private void CheckForCrossBends(Collider other)
    {
        if (other.gameObject.CompareTag("Cross left"))
        {
            transform.localRotation *= Quaternion.Euler(0, -90, 0);
        }
        else if (other.gameObject.CompareTag("Cross right"))
        {
            transform.localRotation *= Quaternion.Euler(0, 90, 0);
        }
    }
}
