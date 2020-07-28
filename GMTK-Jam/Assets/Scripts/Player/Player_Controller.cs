using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]

public class Player_Controller : MonoBehaviour
{
    //External Classes
    AmmoBag ammoBag;
    public StressManager stressManager;
    AudioManager _audioManager;
    private Bullet_Controller _bulletController;

    public bool mayhem = false;
    float mayhemTimer = 0.5f;
    float mayhemGunshotTimer;
    float mayhemRotateTimer;
    Vector3 goal;
    private Quaternion targetRotation;

    //Player Health
    [SerializeField]
    private int _playerHealth = 5;

    //UI Manager
    private UI_Manager _uiManager;

    //Player Movement Variables
    [SerializeField]
    private float _movementSpeed = 10.0f;
    [SerializeField]
    private float _maxVelocityChange = 10.0f;
    //[SerializeField]
    //private float gravity = 14.0f;
    private Rigidbody _playerRb;
    private Vector3 _movementAxis;

    //Player Camera Variables
    [SerializeField]
    private Camera _mainCam = null;
    [SerializeField]
    private GameObject _targetIndicatorPrefab = null;
    [SerializeField]
    private enum CameraDirection { x, z }
    [SerializeField]
    private CameraDirection _cameraDirection = CameraDirection.x;
    [SerializeField]
    private float _cameraHeight = 20f;
    [SerializeField]
    private float _cameraDistance = 7f;

    private GameObject _targetObject;
    //Mouse Cursor Camera Offset Effect Variables
    private Vector3 _playerPosOnScreen;
    private Vector3 _cursorPosition;
    private Vector3 _offsetVector;
    //Plane that represents imaginary floor that will be used to calculate Aim target position
    private Plane _surfacePlane = new Plane();

    private void Start()
    {
        goal = transform.position + new Vector3(Random.insideUnitSphere.x * 5, 0, Random.insideUnitSphere.z * 5);
        mayhemGunshotTimer = Random.Range(0.1f, 1);
        mayhemRotateTimer = Random.Range(0.1f, 0.5f);

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }

        ammoBag = GetComponent<AmmoBag>();
        _audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();        
        _bulletController = GetComponent<Bullet_Controller>();
        _playerRb = GetComponent<Rigidbody>();
        //Instantiate aim target prefab
        if (_targetIndicatorPrefab)
        {
            _targetObject = Instantiate(_targetIndicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        }
        //Hide the cursor
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ammoBag.CanFire())
            {
                if (ammoBag.Fire())
                {
                    _bulletController.Shootbullet();
                    _audioManager.PlayEffect("Pistol");
                }
            }
        }

        //Add stress (Temporary)
        //if (Input.GetMouseButtonDown(1))
        //{
        //    stressManager.AddStress(10);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    stressManager.AddStress(-5);
        //}
    }

    private void FixedUpdate()
    {
        //Setup camera offset
        Vector3 cameraOffset = Vector3.zero;
        if (_cameraDirection == CameraDirection.x)
        {
            cameraOffset = new Vector3(_cameraDistance, _cameraHeight, 0);
        }
        else if (_cameraDirection == CameraDirection.z)
        {
            cameraOffset = new Vector3(0, _cameraHeight, _cameraDistance);
        }

        if (!stressManager.getMayhemBool())
        {
            //Player Movement
            Vector3 targetVelocity = Vector3.zero;
            // Calculate how fast we should be moving
            if (_cameraDirection == CameraDirection.x)
            {
                targetVelocity = new Vector3(Input.GetAxis("Vertical") * (_cameraDistance >= 0 ? -1 : 1), 0, Input.GetAxis("Horizontal") * (_cameraDistance >= 0 ? 1 : -1));
            }
            else if (_cameraDirection == CameraDirection.z)
            {
                targetVelocity = new Vector3(Input.GetAxis("Horizontal") * (_cameraDistance >= 0 ? -1 : 1), 0, Input.GetAxis("Vertical") * (_cameraDistance >= 0 ? -1 : 1));
            }
            targetVelocity *= _movementSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = _playerRb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.y = 0;
            _playerRb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            if (mayhemTimer < 0)
            {
                goal = transform.position + new Vector3(Random.insideUnitSphere.x * 10, 0, Random.insideUnitSphere.z * 10);

                mayhemTimer = 0.25f;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, goal, 0.1f);
                mayhemTimer -= Time.deltaTime;
            }

            if (mayhemRotateTimer < 0)
            {
                targetRotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);
                mayhemRotateTimer = Random.Range(0.1f, 0.5f);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15.0f);
                mayhemRotateTimer -= Time.deltaTime;
            }
            if (mayhemGunshotTimer < 0)
            {
                if (ammoBag.CanFire())
                {
                    if (ammoBag.Fire())
                    {
                        _bulletController.Shootbullet();
                        _audioManager.PlayEffect("Pistol");
                        mayhemGunshotTimer = Random.Range(0.1f, 0.5f);
                    }
                    else
                    {
                        _audioManager.PlayEffect("GunClick");
                        mayhemGunshotTimer = Random.Range(0.1f, 0.5f);
                    }
                }
            }
            else
            {
                mayhemGunshotTimer -= Time.deltaTime;
            }

            //spawnPosition = new Vector3(Random.insideUnitSphere.x * spawnTrigger.radius,
            //transform.position.y, Random.insideUnitSphere.z * spawnTrigger.radius);
        }        
        // We apply gravity manually for more tuning control
        //_playerRb.AddForce(new Vector3(0, -gravity * _playerRb.mass, 0));

        //Mouse cursor offset effect
        _playerPosOnScreen = _mainCam.WorldToViewportPoint(transform.position);
        _cursorPosition = _mainCam.ScreenToViewportPoint(Input.mousePosition);
        _offsetVector = _cursorPosition - _playerPosOnScreen;

        //Camera follow
        _mainCam.transform.position = Vector3.Lerp(_mainCam.transform.position, transform.position + cameraOffset, Time.deltaTime * 7.4f);
        _mainCam.transform.LookAt(transform.position + new Vector3(-_offsetVector.y * 2, 0, _offsetVector.x * 2));

        //Aim target position and rotation
        _targetObject.transform.position = GetAimTargetPos();
        _targetObject.transform.LookAt(new Vector3(transform.position.x, _targetObject.transform.position.y, transform.position.z));

        if (!stressManager.getMayhemBool())
        {
            //Player rotation
            transform.LookAt(new Vector3(_targetObject.transform.position.x, transform.position.y, _targetObject.transform.position.z));
        }
    }

    private Vector3 GetAimTargetPos()
    {
        //Update surface plane
        _surfacePlane.SetNormalAndPosition(Vector3.up, transform.position);
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float enter = 0.0f;

        if (_surfacePlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            return hitPoint;
        }

        //No raycast hit, hide the aim target by moving it far away
        return new Vector3(-5000, -5000, -5000);
    }

    public AmmoBag GetAmmoBag
    {
        get { return ammoBag; }
    }

    public void Damage()
    {
        _playerHealth -= 1;
        _uiManager.UpdateLives(_playerHealth);
        if (_playerHealth < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Destroy(this.gameObject);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "NextStage")
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //    }
    //}
}
