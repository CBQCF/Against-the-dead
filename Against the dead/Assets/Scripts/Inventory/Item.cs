using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Gameplay only")] 
    public GameObject prefab;
    public ItemType type;
    public ActionType actionType;
    public Vector3Int range = new Vector3Int(3, 3, 3);

    [Header("UI only")] 
    public bool stackable = true;

    [Header("Both")] 
    public Sprite image;
    
        
    public enum ActionType
    {
        Dig,
        Mine,
        Shoot,
        Fight,
    }
        
    public enum ItemType
    {
        Ammunition,
        Weapon,
        Tool,
        BuildingBlock,
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }
}