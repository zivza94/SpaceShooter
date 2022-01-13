using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 19f;
    [SerializeField] private GameObject _explotionPrefab;
    private SpawnManager _spawnMamager;

    void Start()
    {
        _spawnMamager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnMamager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed *Time.deltaTime);
        //rotate object on the z axis
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            //add 10 to score
            
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        Instantiate(_explotionPrefab, transform.position, Quaternion.identity);
        _spawnMamager.StartSpawning();
        Destroy(this.gameObject,0.25f);
    }
}
