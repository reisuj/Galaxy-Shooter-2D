using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : BaseEnemy
{    
    private float _revFireTime = 0.0f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 5.0f);
        Debug.DrawRay(transform.position, Vector3.up * 5.0f, Color.red);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
            float _fireDelay = Random.Range(3.0f, 5.0f);
            if (hit.collider.CompareTag("Player") && Time.time > _revFireTime)
            {
                _revFireTime = Time.time + _fireDelay;
                AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
                Instantiate(_enemyLaser, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180.0f));
            }
        }
    }
}
