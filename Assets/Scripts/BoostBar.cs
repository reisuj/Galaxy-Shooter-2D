using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void SetBooster(int boostFuel)
    {
        slider.value = boostFuel;
    }
}
