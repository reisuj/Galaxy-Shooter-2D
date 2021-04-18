using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
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
            if (_uiManager != null)
            {
                _uiManager.PlayerScored();
            }            
            Destroy(this.gameObject);
        }
        else if(other.tag == "Player")
        {            
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
