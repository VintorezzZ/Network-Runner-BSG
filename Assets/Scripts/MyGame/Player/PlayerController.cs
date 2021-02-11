using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using GameManager = Com.MyCompany.MyGame.GameManager;

public class PlayerController : MonoBehaviourPun
{
    #region Public Fields

    public bool canMove = true;

    [SerializeField] private Transform gunHolder; 
    [SerializeField] private Transform rayCastPoint;

    public event Action onGameOver;

    #endregion

    #region Private Variables
    
    private Animator _animator;
    private CharacterController _characterController;
    private GameManager _gm;
    private UI_manager _ui;
    private AudioManager _am;

    private Vector3 _gravity;
    private float gravityAmount = -20;
    [SerializeField] public float speed = 5f;
    [SerializeField] private float strafeSpeed = 6;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private int health = 3;
    [SerializeField] private int bulletAmount = 3;
    private Transform _generatedBullets;
    private WeaponManager _weaponManager;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private bool _canShoot = true;
    private float _horizontalInput;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    #endregion

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
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;  // убрать 
            RoomController.instance.Init(this);
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        //DontDestroyOnLoad(this.gameObject);
        
        onGameOver += _gm.OnGameOver;
        onGameOver += StartDeathRoutine;
        _characterController = gameObject.GetComponent<CharacterController>();
        _animator = gameObject.GetComponentInChildren<Animator>();

        _ui.UpdateBulletsText(bulletAmount);
        _ui.UpdateHealttext(health);

#if UNITY_5_4_OR_NEWER
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        
        canMove = false;
    }


    private void Update()
    {
        if (photonView.IsMine)
        {
            if (transform.position.y < -4)
                onGameOver?.Invoke();

            if (health < 1f)
                GameManager.instance.LeaveRoom();

            if (!canMove)
                return;

            _weaponManager.OnUpdate();
        
            ProcessInputs();

            ProcessAnimation(_horizontalInput);

            SpeedControl();
            Move(_horizontalInput);
        }
    }

    private void ProcessInputs()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetMouseButtonDown(0))
        {
            if (_canShoot && bulletAmount > 0)
            {
                StartCoroutine(Shoot());
                photonView.RPC("ProcessShoot", RpcTarget.Others);
            }
        }
    }

    private void SetManagers()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _ui = GameObject.FindGameObjectWithTag("UI_manager").GetComponent<UI_manager>();
        _am = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>();
        _weaponManager = new WeaponManager(gunHolder, rayCastPoint);
        _weaponManager.Init();
    }

    private void CreateBulletsContainer()
    {
        _generatedBullets = new GameObject("generatedBullets").transform;
        _generatedBullets.SetParent(FindObjectOfType<WorldBuilder>().transform);
    }

    [PunRPC]
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
        
        bulletAmount--;
        _ui.UpdateBulletsText(bulletAmount);
        
        _canShoot = false;
        yield return new WaitForSeconds(0.3f);
        _canShoot = true;
    }

    private void Move(float horizontalInput)
    {
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
                StartCoroutine(_ui.ShowAwesomeText());
                AddCoins(50);
            }
        }
    }

    private void AddCoins(int amount)
    {
        _gm.score += amount;
    }

    private void StartDeathRoutine()
    {
        canMove = false;
        _animator.SetTrigger("dead");
        _am.PlayLoseSFX();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            CheckForObstacle(other);
            CheckForBulletBonus(other);
            CheckForCrossBends(other);
        }
    }

    private void CheckForObstacle(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (health >= 1)
            {
                health--;
                _ui.UpdateHealttext(health);
                PoolManager.Return(other.gameObject.GetComponentInParent<PoolItem>());
            }
            else
            {
                onGameOver?.Invoke();
            }
        }
    }

    private void CheckForBulletBonus(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            _weaponManager.SwitchWeapon("RPG7", 5f);

            bulletAmount++;

            if (bulletAmount > 30) 
                bulletAmount = 30;

            _ui.UpdateBulletsText(bulletAmount);
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
