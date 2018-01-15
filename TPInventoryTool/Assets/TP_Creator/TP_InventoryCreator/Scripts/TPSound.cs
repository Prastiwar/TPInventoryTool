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
        UseItem,
        Failure
    }
    public AudioClip AudioClip;
    public AudioTypeEnum AudioType;
}
