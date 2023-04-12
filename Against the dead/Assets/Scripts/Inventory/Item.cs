using System;
using Unity;
using Mirror;
using UnityEngine;
[CreateAssetMenu(fileName = "New item", menuName = "Create new Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Ammunition,
        Weapon,
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public int id;
    
    public string name;
    [TextArea(3, 3)] public string description;
    
    public ItemType type;
    public Rarity rarity;
    public int maxStack;
    public float weight;
    public int baseValue;

    public GameObject prefab;
    public Texture icon;
    
    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case Rarity.Common: return new Color(255, 255, 255);
            case Rarity.Uncommon: return new Color(0, 179, 0);
            case Rarity.Rare: return new Color(0, 0, 230);
            case Rarity.Epic: return new Color(128, 0, 255); // Purple
            case Rarity.Legendary: return new Color(255, 192, 0); // Orange
        }

        return new Color(0, 0, 0, 0);
    }
    
    public string GetRarityText()
    {
        switch (rarity)
        {
            case Rarity.Common: return "Common";
            case Rarity.Uncommon: return "Uncommon";
            case Rarity.Rare: return "Rare";
            case Rarity.Epic: return "Epic";
            case Rarity.Legendary: return "Legendary";
        }

        return "";
    }

    public string GetTypeText()
    {
        switch (type)
        {
            case ItemType.Weapon: return "Weapon";
            case ItemType.Ammunition: return "Ammunition";
        }

        return "";
    }
}