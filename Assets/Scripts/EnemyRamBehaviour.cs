using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRamBehaviour : BaseEnemy
{
    [SerializeField]
    private float _rotation = 2.5f;
    [SerializeField]
    private float _distance;
    private CircleCollider2D _circleCollider;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _circleCollider = GetComponent<CircleCollider2D>();
        if (_circleCollider == null)
        {
            Debug.LogError("Collider is NULL!");
        }
    }

    protected override void Update()
    {
        base.Update();
        transform.Rotate(0, 0, _rotation);
        RammingCheck();
    }

    private void RammingCheck()
    {
        _distance = Vector2.Distance(_player.transform.position, this.transform.position);
        if (_distance <= 4.0f)
        {
            Vector3 dir = this.transform.position - _player.transform.position;
            dir = dir.normalized;
            this.transform.position -= dir * Time.deltaTime * (_enemySpeed * 2);
        }
        else if (_distance < .3f)
        {
            _circleCollider.enabled = false;
            EnemyDestroyed();
        }
    }
}

