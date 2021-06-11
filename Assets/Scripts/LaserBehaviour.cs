using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 7.0f;

    private Vector3 _direction;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y > 10.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }            
            Destroy(this.gameObject);
        }
    }

    public void FireUp()
    {
        _direction = Vector3.up;
    }
}
