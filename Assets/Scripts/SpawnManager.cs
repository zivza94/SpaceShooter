using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private List<GameObject> _powerupPrefabs;
    [SerializeField]
    private GameObject _powerupContainer;

    private bool _spawning = true;
    
    
    [SerializeField]
    private float _top = 7.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StopSpawning()
    {
        _spawning = false;
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_spawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 pos = new Vector3(randomX, _top, 0); 
            Instantiate(_enemyPrefab,pos,Quaternion.identity, _enemyContainer.transform);
            yield return new WaitForSeconds(5f);
        }
    }
    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_spawning)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 pos = new Vector3(randomX, _top, 0);

            int randomPowerUp = Random.Range(0, _powerupPrefabs.Count);
            Instantiate(_powerupPrefabs[randomPowerUp], pos, Quaternion.identity, _powerupContainer.transform);
            var waitTime = Random.Range(3f, 7f);
            yield return new WaitForSeconds(waitTime);
        }

    }
    
}
