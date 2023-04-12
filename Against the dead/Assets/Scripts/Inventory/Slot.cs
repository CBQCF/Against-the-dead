using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public int Amount;

    private RawImage icon;
    private Image rar;
    private TextMeshProUGUI txt_amount;

    public void SetStats()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);   
        }

        icon = GetComponentInChildren<RawImage>();
        txt_amount = GetComponentInChildren<TextMeshProUGUI>();
        rar = GetComponentInChildren<Image>();
        
        icon.texture = item.icon;
        txt_amount.text = $"{Amount}x";
        rar.color = item.GetRarityColor();
    }
}
