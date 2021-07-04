using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissileBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _missileSpeed = 6;
    [SerializeField]
    private float _posToFindTarget = 1.0f;
    private bool _targetSet = false;
    [SerializeField]
    private GameObject _player;
    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 1.0f)
        {
            transform.Translate(Vector3.down * _missileSpeed * Time.deltaTime);
        }
        else
        {
            AcquireTarget();
            MoveToTargetPosition();
            return;
        }
        
    }

    private void AcquireTarget()
    {
        if (_targetSet == false)
        {
            //_target.position = _player.transform.position;
            _targetSet = true;
        }
    }

    private void MoveToTargetPosition()
    {
        if (_targetSet == true && _player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _missileSpeed * Time.deltaTime);

            Vector2 direction = (transform.position - _player.transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var offset = 270f;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
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
