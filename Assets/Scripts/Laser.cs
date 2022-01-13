using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 8f;

    [SerializeField] 
    private float _topBorder = 8f;

    public float Y => transform.position.y;


    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (Y > _topBorder)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
