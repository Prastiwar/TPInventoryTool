using System.Collections;
using System.Collections.Generic;
using TP_Inventory;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField] TPInventoryCreator manager;
    [SerializeField] TPSlot slot;
    [SerializeField] TPItem item;

    void Awake()
    {
        manager = FindObjectOfType<TPInventoryCreator>();
        slot.Item = item;
    }
}