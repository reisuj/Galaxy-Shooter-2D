using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Transform _enemyTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void TargetClosestEnemy()
    //{
    //    GameObject[] enemies;

    //    enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject closestEnemy = null;
    //    float distance = Mathf.Infinity;
        
    //    foreach (GameObject enemy in enemies)
    //    {
    //        Vector3 difference = enemy.transform.position - this.transform.position;
    //        float currentDist = difference.sqrMagnitude;
    //        if (currentDist < distance)
    //        {
    //            closestEnemy = enemy;
    //            distance = currentDist;
    //        }
    //    }
    //}
}
