using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehaviour : MonoBehaviour
{
    private float _speed;
    private float _destinationY;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _explosionAudio;


    // Start is called before the first frame update
    void Start()
    {
        _destinationY = Random.Range(-5.0f, 0.0f);
        _speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        MineMoveMent();
    }

    private void MineMoveMent()
    {
        if (transform.position.y > _destinationY)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, _destinationY, 0);
            StartCoroutine(StartSelfDestruct());
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
            DestroyMine();
        }
    }

    private void DestroyMine()
    {
        AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator StartSelfDestruct()
    {
        float _selfDestructTimer = Random.Range(10.0f, 20.0f);
        yield return new WaitForSeconds(_selfDestructTimer);
        DestroyMine();
    }
}
