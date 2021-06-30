using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private float _destinationY = 3.5f;
    private float _speed = 1.5f;
    [SerializeField]
    private int _bossHealth;

    [SerializeField]
    private GameObject _explosion1;
    [SerializeField]
    private GameObject _explosion2;
    // Start is called before the first frame update
    void Start()
    {
        _bossHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToStart();
    }

    private void MoveToStart()
    {
        if (transform.position.y > _destinationY)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, _destinationY, 0);
        }
    }
    private void HitByPlayer(int damage)
    {
        _bossHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerLaser")
        {
            GameObject newExplosion = Instantiate(_explosion1, other.transform.position, Quaternion.identity);
            newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Destroy(other.gameObject);
            HitByPlayer(5);
        }
        else if (other.tag == "HomingMissile")
        {
            GameObject newExplosion = Instantiate(_explosion2, other.transform.position, Quaternion.identity);
            newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Destroy(other.gameObject);
            HitByPlayer(10);
        }
    }


}
