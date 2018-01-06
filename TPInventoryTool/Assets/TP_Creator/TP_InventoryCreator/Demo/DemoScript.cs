using System.Collections;
using System.Collections.Generic;
using TP_Inventory;
using UnityEngine;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour
{
    [SerializeField] TPInventoryCreator manager;
    [SerializeField] TPSlot[] slots;
    [SerializeField] TPItem[] items;
    [SerializeField] Text[] texts;
    [SerializeField] TPStat[] stats;

    void Awake()
    {
        int length = items.Length;
        for (int i = 0; i < length; i++)
        {
            if (i != slots.Length)
                slots[i].Item = items[i];
        }

        texts[0].text = stats[0].name + ": " + stats[0].Value.ToString();
        texts[1].text = stats[1].name + ": " + stats[1].Value.ToString();
    }

    void Update()
    {
        if (stats[0].HasChanged)
        {
            texts[0].text = stats[0].name + ": " + stats[0].Value.ToString();
        }

        if (stats[1].HasChanged)
        {
            texts[1].text = stats[1].name + ": " + stats[1].Value.ToString();
        }
    }
}