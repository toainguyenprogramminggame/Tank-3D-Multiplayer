using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float maxHealth, float curHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = curHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public void SetHealth(float newHealth)
    {
        slider.value = newHealth;
    }
}
