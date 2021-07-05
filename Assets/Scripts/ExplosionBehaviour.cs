using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 2.75f);
    }
}
