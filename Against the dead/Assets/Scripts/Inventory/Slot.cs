using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : NetworkBehaviour
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
        rar.color = GetRarity(item.rarity);
    }

    Color GetRarity(Item.Rarity rarity)
    {
        switch (rarity)
        {
            case Item.Rarity.Common: return new Color(255, 255, 255);
            case Item.Rarity.Uncommon: return new Color(0, 179, 0);
            case Item.Rarity.Rare: return new Color(0, 0, 230);
            case Item.Rarity.Epic: return new Color(128, 0, 255); // Purple
            case Item.Rarity.Legendary: return new Color(255, 192, 0); // Orange
        }

        return new Color(0, 0, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
