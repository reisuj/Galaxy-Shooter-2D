using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyXPowerup : BaseEnemy
{
    private bool _canShoot = true;

    protected override void Start()
    {
        base.Start();
    }
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 8.0f);
        Debug.DrawRay(transform.position, Vector3.down * 8.0f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Powerup" && _canShoot == true && hit.collider.name != "Negative_Powerup")
            {
                AudioSource.PlayClipAtPoint(_laserAudio, transform.position, 1.0f);
                Instantiate(_enemyLaser, transform.position, Quaternion.identity);
                _canShoot = false;
            }
        }
    }
}
