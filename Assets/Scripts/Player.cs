using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public or private reference
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField]
    private int _lives = 3;

    #region screen borders
    [SerializeField]
    private float _topBorder = 0f;
    [SerializeField]
    private float _bottomBorder = -3f;
    [SerializeField]
    private float _rightBorder = 11f;
    [SerializeField]
    private float _leftBorder = -11f;
    #endregion
    
    private float X => transform.position.x;
    private float Y => transform.position.y;

    #region fire details
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserAudioClip;
    
    private Vector3 _offset = new Vector3(0,1.05f);
    [SerializeField]
    private float _fireRate = 0.5f;

    private float _canFire = -1.0f;
    #endregion

    #region Triple shot powerup
    [SerializeField]
    private bool _isTripleShotActive = false;
    //private bool _isSpeedBoostActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    #endregion

    #region Shield PowerUp
    [SerializeField] private GameObject _shieldVisulaizer;
    private bool _isShieldActive = false;


    #endregion


    //audio clip
    private AudioSource _audioSource;
    #region Hurt Visulaizers
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    #endregion

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    [SerializeField] private int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source in player is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
    }

    private void CalculateMovement()
    {
        Vector3 direction = new Vector3(GetHorizontalAxis(), GetVerticalAxis(), 0);
        transform.Translate(direction * _speed * Time.deltaTime);


        // if exit the boundary teleport to the other side 
        transform.position = new Vector3(X,Mathf.Clamp(Y, _bottomBorder,_topBorder));

        if (X > _rightBorder || X < _leftBorder)
        {
            transform.position = new Vector3(-X, Y, 0);
        }
    }

    private void Shoot()
    {
        _canFire = Time.time + _fireRate;
        
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }
        
        _audioSource.Play();
    }
    #region Helpers
    private float GetHorizontalAxis()
    {
        return Input.GetAxis("Horizontal");
    }
    private float GetVerticalAxis()
    {
        return Input.GetAxis("Vertical");
    }
    #endregion


    public void Damage()
    {
        if (_isShieldActive)
        {
            SetEnableShield(false);
            return;
        }
        _lives--;
        Hurt();
        _uiManager.UpdateLives(_lives);
        if (_lives <= 0)
        {
            _spawnManager.StopSpawning();
            Destroy(this.gameObject);
        }
    }

    #region Triple Shot
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }
    #endregion
    #region Boost Speed
    public void BoostSpeedActive()
    {
        //_isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(BoostSpeedPowerDownRoutine());
    }
    private IEnumerator BoostSpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _speed /= _speedMultiplier;
        //_isSpeedBoostActive = false;
        
    }
    #endregion

    #region Shield PowerUp
    public void ShieldActive()
    {
        SetEnableShield(true);
        StartCoroutine(ShieldPowerDownRoutine());
    }
    private IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(10f);
        SetEnableShield(false);
    }

    private void SetEnableShield(bool enable)
    {
        _isShieldActive = enable;
        _shieldVisulaizer.SetActive(enable);
    }



    #endregion

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void Hurt()
    {
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
    }
}
