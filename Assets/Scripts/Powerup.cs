using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _powerupAudio;
    [SerializeField]
    private int ammoRefillAmount = 15;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < -3.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldPickedUp();
                        break;
                    case 3:                        
                        player.AmmoCollected(ammoRefillAmount);
                        break;
                    case 4:
                        player.HealthCollected();
                        break;
                    case 5:
                        player.MultiShotActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }                
            }
            AudioSource.PlayClipAtPoint(_powerupAudio, new Vector3(0, 0, -10), 1.0f);
            Destroy(this.gameObject);
        }
    }
}
