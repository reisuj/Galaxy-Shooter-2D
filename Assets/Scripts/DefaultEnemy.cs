using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : BaseEnemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _enemySpeed = 3.0f;
    }
}
