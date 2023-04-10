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
}