using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;

    private Animator _anim;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL!");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL!");
        }


    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -3.5f)
        {
            float newX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(newX, 10.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerLaser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(Random.Range(5, 11));
            }
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject);
        }
        else if(other.tag == "Player")
        {            
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject);
        }
    }
}
