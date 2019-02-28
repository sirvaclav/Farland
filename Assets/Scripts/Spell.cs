using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {
    private string _name;
    private int _damage;
    private List<string> _elements = new List<string>(4);

    public string Name { get { return _name; } private set { _name = value; } }
    public int Damage { get { return _damage; } private set { _damage = value; } }
    public List<string> Elements { get { return _elements; } private set { _elements = value; } }

    public Spell(string name, string elements, string damage)
    {
        _name = name;

        string[] split = elements.Split('|');

        foreach(string el in split)
        {
            _elements.Add(el);
        }

        int result = 0;
        int.TryParse(damage, out result);
        _damage = result;
    }
}