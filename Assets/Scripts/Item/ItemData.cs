using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName = "Scriptable Object/ItemData")]

public class ItemData : ScriptableObject
{
    public enum ItemType {melee, Range, Glove, Shoe, Heal}


    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemname;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;

}
