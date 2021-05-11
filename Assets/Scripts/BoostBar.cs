using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    public void SetStartBooster(int boosterMax)
    {
        slider.maxValue = boosterMax;
        slider.value = slider.maxValue;
    }
    public void SetBooster(int boostFuel)
    {
        slider.value = boostFuel;
    }
}
