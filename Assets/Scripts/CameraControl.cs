using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Animator _anim;   

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL!");
        }
    }

    public void CamShake()
    {
        _anim.SetTrigger("Shake");
    }
}
