using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// třída využívaná XMLLoaderem
/// </summary>
public class ItemFind
{
    private int _index;
    private string _category;

    public int Index { get { return _index; } set { _index = value; } }
    public string Category { get { return _category; } set { _category = value; } }
}

public class Item
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description {  get; set;}
    public int DefendHeal { get; set ;}
    public int BonusHealth { get; set;}
    public int BonusStamina { get; set;}
    public int BonusDamage { get; set;}
}
