using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    public float Y => transform.position.y;
    private float _top = 8f;
    private float _bottomBorder = -6f;
    private Player _player;
    private AudioSource _audioSource;
    private Animator _animator;
    // create animator handler

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        
        _animator = this.gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source in Enemy is NULL");
        }
        //null checked player
        //assign the component to anam
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down* _speed * Time.deltaTime);
        if (Y < _bottomBorder)
        {
            RespawnAtTop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            DestroyEnemy();
        }
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            //add 10 to score
            if (_player != null)
            {
                _player.AddScore(10);
            }

            DestroyEnemy();
        }
    }
    private void RespawnAtTop()
    {
        float randomX = Random.Range(-8f, 8f);
        transform.position = new Vector3(randomX,_top,0);
    }

    private void DestroyEnemy()
    {
        //trigger anim
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioSource.Play();
        Destroy(this.gameObject,2.8f);
    }
}
