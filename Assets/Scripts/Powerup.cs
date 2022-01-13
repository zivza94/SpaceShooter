using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5;
    //private float _top = 8f;
    private float _bottomBorder = -6f;

    [SerializeField] private PowerupType _powerupType;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _bottomBorder)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupType)
                {
                    case PowerupType.TripleShot:
                        player.TripleShotActive();
                        break;
                    case PowerupType.Speed:
                        player.BoostSpeedActive();
                        break;
                    case PowerupType.Shield:
                        player.ShieldActive();
                        break;

                }
            }
            Destroy(this.gameObject);
        }
    }
}

public enum PowerupType
{
    TripleShot = 0,
    Speed = 1,
    Shield = 2

}
