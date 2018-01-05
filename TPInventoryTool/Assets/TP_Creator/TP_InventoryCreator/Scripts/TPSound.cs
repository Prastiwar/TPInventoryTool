using UnityEngine;

[System.Serializable]
public struct TPSound
{
    public enum AudioTypeEnum
    {
        GetItem,
        RemoveItem,
        WearItem,
        MoveItem,
        UseItem
    }
    public AudioClip AudioClip;
    public AudioTypeEnum AudioType;
}
