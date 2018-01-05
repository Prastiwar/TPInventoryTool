using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TP_Inventory;
using UnityEngine;

namespace TP_InventoryEditor
{
    [RequireComponent(typeof(TPInventoryCreator))]
    public class TPInventoryPersistance : MonoBehaviour
    {
        public TPInventoryData inventoryData;

        TPInventoryCreator TPInventoryCreator;
        string saveName = "inventory";
        string extenstionName = "TP_Save";

        void OnValidate()
        {
            if (TPInventoryCreator == null) TPInventoryCreator = GetComponent<TPInventoryCreator>();
        }

        public void Save()
        {
            string path = Application.persistentDataPath + "/" + saveName + "." + extenstionName;
            BinaryFormatter bf = new BinaryFormatter();
            List<System.Object> objects = new List<System.Object>();
            FileStream file = File.Create(path);

            // Index of 0 to statLength (- 1)
            int statLength = inventoryData.Stats.Count;
            for (int i = 0; i < statLength; i++)
            {
                objects.Add(inventoryData.Stats[i].Save());
            }

            // Index of statLength
            int slotsLength = TPInventoryCreator.Slots.Count;
            for (int i = 0; i < slotsLength; i++)
            {
                objects.Add(TPInventoryCreator.Slots[i].Save());
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
            int statLength = inventoryData.Stats.Count;
            for (int i = 0; i < statLength; i++)
            {
                inventoryData.Stats[i].Load((float)objects[i]);
            }

            // Index of statLength to slotLength
            int slotsLength = TPInventoryCreator.Slots.Count;
            for (int i = 0; i < slotsLength; i++)
            {
                TPInventoryCreator.Slots[i].Load((int)objects[statLength + i]);
            }


            file.Close();
        }
    }
}