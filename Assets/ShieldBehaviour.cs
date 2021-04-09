using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    [SerializeField]
    private int _shieldCharges = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShieldHit()
    {
        _shieldCharges -= 1;
        switch (_shieldCharges)
        {
            case 0:
                gameObject.SetActive(false);
                break;
            case 1:
                GetComponent<Renderer>().material.color = new Color(255, 0, 0);
                break;
            case 2:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            //case 3:
            //    GetComponent<Renderer>().material.color = Color.blue;
            //    break;
        }
    }
}
