using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    public void SetMaxHealth(float health)
    {
        totalHealthBar.fillAmount = health / 100;
    }
    public void SetHealth(float health)
    {

        currentHealthBar.fillAmount = health / 100;
    }
}
