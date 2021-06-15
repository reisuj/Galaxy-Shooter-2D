using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseScan : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("ReverseScan collider hit by " + collision.name);
            EnemyBehaviour _enemyBehaviour = transform.parent.GetComponent<EnemyBehaviour>();
            //_enemyBehaviour.FireBack();
        }
    }
}
