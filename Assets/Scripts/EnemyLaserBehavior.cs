using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserBehavior : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 7.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime, Space.World);
        if (transform.position.y < -7.0f || transform.position.y > 10.0f)
        {            
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
