using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TP_Inventory_Item;
using TP_Inventory_Slot;
using UnityEngine;

public class InventorySaveLoad : MonoBehaviour
{
    [Header("Put there everything you want to save/load")]
    public Item[] Items;
    public Stat[] Stats;
    [HideInInspector] public List<Slot> Slots = new List<Slot>();
    string saveName = "inventory";
    string extenstionName = "TP_Save";

    public void Save()
    {
        string path = Application.persistentDataPath + "/" + saveName + "." + extenstionName;
        BinaryFormatter bf = new BinaryFormatter();
        List<System.Object> objects = new List<System.Object>();
        FileStream file = File.Create(path);

        // Index of 0 to statLength (- 1)
        int statLength = Stats.Length;
        for (int i = 0; i < statLength; i++)
        {
            objects.Add(Stats[i].Save());
        }

        // Index of statLength
        int slotsLength = Slots.Count;
        for (int i = 0; i < slotsLength; i++)
        {
            objects.Add(Slots[i].Save());
        }

        bf.Serialize(file, objects);
        file.Close();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/" + saveName + "." + extenstionName;
        if (!File.Exists(path))
            return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        object serializedObject = bf.Deserialize(file);
        List<System.Object> objects = serializedObject as List<System.Object>;

        // Index of 0 to statLength(- 1)
        int statLength = Stats.Length;
        for (int i = 0; i < statLength; i++)
        {
            Stats[i].Load((float)objects[i]);
        }

        // Index of statLength to slotLength
        int slotsLength = Slots.Count;
        for (int i = 0; i < slotsLength; i++)
        {
            Slots[i].Load((int)objects[statLength + i]);
        }


        file.Close();
    }
}
