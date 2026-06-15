using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedHealthBar : MonoBehaviour
{
    public Slider slider;
    private float decreaseSpeed = 0.1f;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        StartCoroutine(DelayedHealth(health));
    }

    IEnumerator DelayedHealth(float targetHealth)
    {
        while (slider.value > targetHealth)
        {
            slider.value -= decreaseSpeed;
            yield return new WaitForSeconds(0.05f);
        }
        slider.value = targetHealth;
    }
}