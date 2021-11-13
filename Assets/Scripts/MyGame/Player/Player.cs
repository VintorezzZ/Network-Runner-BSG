using System.Collections;
using MyGame.Managers;
using MyGame.Player;
using UnityEngine;
using Utils;

[RequireComponent(typeof(PickUpHandler), typeof(MoveController))]
public class Player : MonoBehaviour
{
    #region Public Fields

    public bool canMove = true;
    public float score;
    public MoveController moveController;
    public WeaponManager weaponManager;

    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform rayCastPoint;

    #endregion

    #region Private Variables

    private Animator _animator;
    
    [SerializeField] private int startHealth = 3;
    [SerializeField] private int startBullets = 3;
    private int _health;
    private int _bullets;
    private Transform _generatedBullets;
    private PickUpHandler _pickUpHandler;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private bool _canShoot = true;

    #endregion

    public int Ammo
    {
        get => _bullets;

        set
        {
            _bullets = value;
            EventHub.OnBulletsChanged(_bullets);
        }
    }
    
    public int Health
    {
        get => _health;

        set
        {
            _health = value;
            EventHub.OnHealthChanged(_health);
        }
    }
    
    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }
    
    private void Awake()
    {
        CreateBulletsContainer();

        weaponManager = new WeaponManager(gunHolder, rayCastPoint);
        _animator = GetComponentInChildren<Animator>();
        _pickUpHandler = GetComponent<PickUpHandler>();
        _pickUpHandler.Init(this);
        moveController = GetComponent<MoveController>();
        moveController.Init(this);
        EventHub.gameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        canMove = true;
    }

    private void Start()
    {
        canMove = false;
        Health = startHealth;
        Ammo = startBullets;
    }
    
    private void Update()
    {
        if (!canMove)
            return;
        
        if (_health < 0f || transform.position.y < -4)
            StartDeathRoutine();
        
        weaponManager.Tick();
        moveController.Tick();
        
        UpdateScore();
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_canShoot && _bullets > 0)
            {
                StartCoroutine(Shoot());
            }
        }

        ProcessAnimation(moveController.horizontalInput);
    }
    
    private void UpdateScore()
    {
        score += moveController.speed * Time.deltaTime;
    }

    public void CheckForBestScore()
    {
        if (score > PlayerPrefs.GetFloat("bestscore"))
        {
            PlayerPrefs.SetFloat("bestscore", score);
        }
    }
    
    private void CreateBulletsContainer()
    {
        _generatedBullets = new GameObject("generatedBullets").transform;
        _generatedBullets.SetParent(FindObjectOfType<WorldBuilder>().transform);
    }
    
    private void ProcessAnimation(float horizontalInput)
    {
        _animator.SetFloat(Blend, horizontalInput);

        if (horizontalInput > 0.2f || horizontalInput < -0.2f)
        {
            CheckForAwesomeTrigger();
        }
    }
    
    private IEnumerator Shoot()
    {
        weaponManager.Shoot();

        Ammo--;

        _canShoot = false;
        yield return new WaitForSeconds(0.3f);
        _canShoot = true;
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
                _pickUpHandler.AddAwesomeTriggerScore(50);
            }
        }
    }

    private void StartDeathRoutine()
    {
        canMove = false;
        _animator.SetTrigger("dead");
        score = Mathf.RoundToInt(score);
        CheckForBestScore();
        
        EventHub.OnGameOvered();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!canMove)
            return;
        
        CheckForObstacle(other);
    }

    private void CheckForObstacle(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            SoundManager.Instance.PlayHit();
            
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
}
