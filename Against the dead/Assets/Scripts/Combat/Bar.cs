using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{

    public Slider slider;

    public void SetMax(int value)
    {
        slider.maxValue = value;
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }
}
