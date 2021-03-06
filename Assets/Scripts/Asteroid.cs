using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private AudioClip _explosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 2, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 0.1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerLaser")
        {
            AudioSource.PlayClipAtPoint(_explosionAudio, new Vector3(0, 0, -10), 1.0f);
            Destroy(collision.gameObject);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 1.0f);
        }
    }
}
