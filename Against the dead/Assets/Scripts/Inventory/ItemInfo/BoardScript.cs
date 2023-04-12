using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour
{
    [SerializeField] private RawImage gIcon;
    [SerializeField] private TextMeshProUGUI gName;
    [SerializeField] private TextMeshProUGUI gType;
    [SerializeField] private TextMeshProUGUI gQuantity;
    [SerializeField] private TextMeshProUGUI gWeight;
    [SerializeField] private TextMeshProUGUI gDescription;
    [SerializeField] private Image gRarityImg;
    [SerializeField] private TextMeshProUGUI gRarityText;

    public Slot slot;
    private Item _item;
    private int _amount;

    private void OnEnable()
    {
        _item = slot.item;
        _amount = slot.Amount;
        Setup();
    }

    private void Setup()
    {
        gIcon.texture = _item.icon;
        gName.text = _item.name;
        gType.text = _item.GetTypeText();
        gQuantity.text = $"{_amount}/{_item.maxStack}";
        gWeight.text = $"{_item.weight * _amount}kg";
        gDescription.text = _item.description;
        gRarityImg.color = _item.GetRarityColor();
        gRarityText.color = gRarityImg.color;
        gRarityText.text = _item.GetRarityText();
    }
}
