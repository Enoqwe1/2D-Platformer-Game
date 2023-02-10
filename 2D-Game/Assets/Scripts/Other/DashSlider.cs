using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSlider : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GameObject.Find("UICanvas/DashCoolDown").GetComponent<Slider>();
    }
    private void Start()
    {
        slider.maxValue = 1.6f;
        slider.minValue = 0.0f;
        slider.value = 1.6f;
    }
    public void Dashing()
    {
        slider.value = 0.0f;
    }
    private void FixedUpdate()
    {
        if (slider.value < slider.maxValue)
        {
            slider.value += Time.deltaTime;
        }
    }

}
