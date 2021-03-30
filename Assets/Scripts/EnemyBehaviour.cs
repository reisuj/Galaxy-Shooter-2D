using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerLaser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if(other.tag == "Player")
        {
            Debug.Log("Player Hit!!");

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
