using TP_Inventory;
using UnityEngine;
using UnityEngine.UI;

public class ExampleInventoryScript : MonoBehaviour
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

        stats[0].AfterChange = UpdateFirstText;
        stats[1].AfterChange = UpdateSecondText;
    }

    void UpdateFirstText()
    {
        texts[0].text = stats[0].name + ": " + stats[0].Value.ToString();
    }
    void UpdateSecondText()
    {
        texts[1].text = stats[1].name + ": " + stats[1].Value.ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        if (items[0].OnSlot != null)
            Debug.Log(items[0].OnSlot);
    }
}