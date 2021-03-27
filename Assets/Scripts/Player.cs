using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalMovement * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalMovement * _speed * Time.deltaTime);

        // Restricts Player's Vetical movement between -1.5 and 3.0on the Y axis.
        transform.position = new Vector3(transform.position.x, (Mathf.Clamp(transform.position.y, -1.5f, 3.0f)), 0);

        // Wraps Player on the X Axis when going past each edge.
        if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        else if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        
    }
}
