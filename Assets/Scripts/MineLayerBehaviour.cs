using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineLayerBehaviour : BaseEnemy
{
    [SerializeField]
    private float _rotation = 1.0f;
    [SerializeField]
    private CircleCollider2D _circleCollider;
    [SerializeField]
    private float _startPositionY;
    [SerializeField]
    private float _startPositionX;
    private int _randomInt;
    protected override void Start()
    {
        SetStartPosition();

        GetReferences();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void CalculateMovement()
    {
        if (_randomInt == 0)
        {
            transform.Rotate(0, 0, -_rotation);
            transform.Translate(Vector3.right * (_enemySpeed / 2) * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(0, 0, _rotation);
            transform.Translate(Vector3.left * (_enemySpeed / 2) * Time.deltaTime, Space.World);
        }
        
        if (transform.position.x < -13.2f || transform.position.x > 13.2f)
        {
            SetStartPosition();
            //Destroy(this.gameObject);
        }
    }

    protected override void EnemyDestroyed()
    {
        _enemySpeed = 0;
        _rotation = 0;
        AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
        Instantiate(_explosionFX, transform.position, Quaternion.identity);
        _circleCollider.enabled = false;
        _spawnManager.EnemyKilled();
        Destroy(this.gameObject, 1.0f);
    }

    private void SetStartPosition()
    {
        _startPositionY = Random.Range(1.0f, 4.5f);
        _randomInt = Random.Range(0, 2);
        if (_randomInt == 0)
        {
            _startPositionX = -13.0f;
        }
        else
        {
            _startPositionX = 13.0f;
        }

        this.transform.position = new Vector3(_startPositionX, _startPositionY, 0);
    }

    private void GetReferences()
    {
        _spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL!");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        _circleCollider = GetComponent<CircleCollider2D>();
        if (_circleCollider == null)
        {
            Debug.LogError("Collider is NULL!");
        }
    }
}
