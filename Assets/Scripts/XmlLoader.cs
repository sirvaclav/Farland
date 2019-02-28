using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XmlLoader{

    private string _itemsFileName = "items.xml";
    private string _spellsFileName = "spells.xml";
    System.Random random = new System.Random();

    /// <summary>
    /// Metoda pro nalezení informací o itemu v xml
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public Item LoadItemFromXml(string type, int index)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, _itemsFileName);
        
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode node = doc.DocumentElement.SelectSingleNode("/items/" + type);
            node = node.ChildNodes[index];

            string itemName = node.ChildNodes[0].InnerText;
            string itemDesc = node.ChildNodes[1].InnerText;
            int itemDefHeal = 0;
            int itemBonHp = 0;
            int itemBonStam = 0;
            int itemBonDmg = 0;

            foreach (XmlNode nodes in node.ChildNodes[2])
            {
                int value = 0; 
                int.TryParse(nodes.InnerText, out value);
                                
                switch(nodes.Name)
                {
                    case "damagereduction": itemDefHeal = value; break;
                    case "health": itemBonHp = value; break;
                    case "stamina": itemBonStam = value; break;
                    case "damage": itemBonDmg = value; break;
                    default: break;
                }
            }

            Item item = new Item
            {
                Name = itemName,
                Description = itemDesc,
                Category = node.Name,
                BonusHealth = itemBonHp,
                BonusStamina = itemBonStam,
                BonusDamage = itemBonDmg,
                DefendHeal = itemDefHeal
            };

            return item;
        }

        else return null;
    }

    /// <summary>
    /// Metoda pro nalezení náhodného itemu
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string[] LoadRandomItemIndexFromXml(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string[] item = new string[2];
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode node = doc.DocumentElement.SelectSingleNode("/items");

            int index = random.Next(node.ChildNodes.Count);

            node = node.ChildNodes[index];

            item[1] = node.Name;

            index = random.Next(node.ChildNodes.Count);

            item[0] = node.ChildNodes[index].InnerText;
            
            return item;
        }

        else return null;
    }

    /// <summary>
    /// Metoda pro nalezení informací o kouzle v xml
    /// </summary>
    /// <param name="spellBase"></param>
    /// <returns></returns>
    public List<Spell> LoadSpellsFromXML(string spellBase)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, _spellsFileName);

        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode node = doc.DocumentElement.SelectSingleNode("/spells/" + spellBase + "Base");

            List<Spell> spells = new List<Spell>();
            
            foreach (XmlNode spell in node.ChildNodes)
            {
                Spell _spell = new Spell(
                    spell.ChildNodes[0].InnerText,
                    spell.ChildNodes[1].InnerText,
                    spell.ChildNodes[2].InnerText
                    );

                spells.Add(_spell);
            }

            return spells;
        }
        else return null;
    }
}
