using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineLayerBehaviour : BaseEnemy
{
    [SerializeField]
    private float _rotation = 3.0f;
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    private CircleCollider2D _circleCollider;

    protected override void Start()
    {
        base.Start();

        _circleCollider = GetComponent<CircleCollider2D>();
        if (_circleCollider == null)
        {
            Debug.LogError("Collider is NULL!");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.Rotate(0, 0, _rotation);
        transform.Translate(Vector3.down * _rotateSpeed * Time.deltaTime);
    }

    protected override void EnemyDestroyed()
    {
        _enemySpeed = 0;
        _rotation = 0;
        _rotateSpeed = 0;
        AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
        Instantiate(_explosionFX, transform.position, Quaternion.identity);
        _circleCollider.enabled = false;
        _spawnManager.EnemyKilled();
        Destroy(this.gameObject, 1.0f);
    }
}
