using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedHealthBar : MonoBehaviour
{
    public Slider slider;
    private float decreaseSpeed = 0.1f;
    private readonly WaitForSeconds _delay = new WaitForSeconds(0.05f);
    private Coroutine _delayedHealthCoroutine;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        if (_delayedHealthCoroutine != null)
        {
            StopCoroutine(_delayedHealthCoroutine);
        }
        _delayedHealthCoroutine = StartCoroutine(DelayedHealth(health));
    }

    IEnumerator DelayedHealth(float targetHealth)
    {
        while (slider.value > targetHealth)
        {
            slider.value -= decreaseSpeed;
            yield return _delay;
        }
        slider.value = targetHealth;
        _delayedHealthCoroutine = null;
    }
}