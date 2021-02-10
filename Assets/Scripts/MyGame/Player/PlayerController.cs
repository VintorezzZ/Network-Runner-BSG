using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using GameManager = Com.MyCompany.MyGame.GameManager;
using Object = UnityEngine.Object;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    #region Public Fields

    public bool canMove = true;
    public event Action onGameOver;

    #endregion

    #region Private Variables

    private Animator animator;
    private CharacterController characterController;
    private GameManager gm;
    private UI_manager ui;
    private AudioManager am;

    private Vector3 gravity;
    private float gravityAmount = -20;
    [SerializeField] public float speed = 5f;
    [SerializeField] private float strafeSpeed = 6;
    [SerializeField] private float acceleration = 1;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private int health = 3;
    [SerializeField] private int bulletAmount = 3;
    private Transform generatedBullets;
    private WeaponManager weaponManager;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private bool canShoot = true;
    private bool isFiring = false;
    private float horizontalInput;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    
    private PhotonView _photonView;
    
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

    #if !UNITY_5_4_OR_NEWER
    /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
    void OnLevelWasLoaded(int level)
    {
    this.CalledOnLevelWasLoaded(level);
    }
    #endif


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
    
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // if (stream.IsWriting)
        // {
        //     // We own this player: send the others our data
        //     stream.SendNext(isFiring);
        //     stream.SendNext(health);
        // }
        // else
        // {
        //     // Network player, receive data
        //     this.isFiring = (bool)stream.ReceiveNext();
        //     this.health = (int)stream.ReceiveNext();
        // }
    }

    #endregion
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();

        CreateBulletsContainer();
        SetManagers();
        
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (_photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;  // убрать 
            RoomController.instance.myPlayer = this;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        onGameOver += gm.OnGameOver;
        onGameOver += StartDeathRoutine;
        characterController = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();
        
        ui.UpdateBulletsText(bulletAmount);
        ui.UpdateHealttext(health);

#if UNITY_5_4_OR_NEWER
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        
        canMove = false;
    }


    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (transform.position.y < -4)
                onGameOver?.Invoke();

            if (health < 1f)
                GameManager.instance.LeaveRoom();

            if (!canMove)
                return;

            weaponManager.OnUpdate();
        
            ProcessInputs();

            ProcessAnimation(horizontalInput);

            SpeedControl();
            Move(horizontalInput);
        }
    }

    private void ProcessInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot && bulletAmount > 0)
            {
                //isFiring = true;
                
                _photonView.RPC("ProcessShoot", RpcTarget.AllViaServer);
            }
        }
    }

    private void SetManagers()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ui = GameObject.FindGameObjectWithTag("UI_manager").GetComponent<UI_manager>();
        am = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>();
        weaponManager = new WeaponManager(
            GameObject.Find("GunHolder").transform, 
            GameObject.Find("RayCastPoint").transform);
        weaponManager.Init();
    }

    private void CreateBulletsContainer()
    {
        generatedBullets = new GameObject("generatedBullets").transform;
        generatedBullets.SetParent(FindObjectOfType<WorldBuilder>().transform);
    }

    [PunRPC]
    private void ProcessShoot()
    {
        // if (isFiring)
        // {
        //     isFiring = false;
        //     StartCoroutine(Shoot());
        // }
        
        StartCoroutine(Shoot());
        
    }

    private void ProcessAnimation(float horizontalInput)
    {
        animator.SetFloat(Blend, horizontalInput);
        
        CheckForAwesomeTrigger();
    }


    private void SpeedControl()
    {
        speed += acceleration * Time.deltaTime;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }

    private IEnumerator Shoot()
    {
        weaponManager.Shoot();
        
        bulletAmount--;
        ui.UpdateBulletsText(bulletAmount);
        
        canShoot = false;
        yield return new WaitForSeconds(0.3f);
        canShoot = true;
    }

    private void Move(float horizontalInput)
    {
        Vector3 moveDir = transform.right * (horizontalInput * strafeSpeed) + transform.forward * speed;
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
        animator.SetTrigger("dead");
        am.PlayLoseSFX();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_photonView.IsMine)
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
                ui.UpdateHealttext(health);
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
            weaponManager.SwitchWeapon("RPG7", 5f);

            bulletAmount++;

            if (bulletAmount > 30) 
                bulletAmount = 30;

            ui.UpdateBulletsText(bulletAmount);
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
