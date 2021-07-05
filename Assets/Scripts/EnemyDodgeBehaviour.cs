using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodgeBehaviour : BaseEnemy
{
    private int _moveDirection;
    private bool _canDodge = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 8.0f);
        Debug.DrawRay(transform.position, Vector3.down * 8.0f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("PlayerLaser") && _canDodge == true)
            {
                _moveDirection = Random.Range(0, 2) == 0 ? -2 : 2;
                transform.position = new Vector3(transform.position.x - _moveDirection, transform.position.y, transform.position.z);
                _canDodge = false;
            }
        }
    }
}
