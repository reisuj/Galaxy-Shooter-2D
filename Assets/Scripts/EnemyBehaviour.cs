using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 5.0f;
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
}
