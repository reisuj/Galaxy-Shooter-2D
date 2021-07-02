using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    public void SetBosshealth(int bossHealthMax)
    {
        slider.maxValue = bossHealthMax;
        slider.value = slider.maxValue;
    }
    public void DamageBoss(int bossHealth)
    {
        slider.value = bossHealth;
    }
}
