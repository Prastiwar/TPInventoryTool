using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TPSound
{
    public enum AudioTypeEnum
    {
        GetItemSound,
        RemoveItemSound,
        UseItemSound
    }
    public AudioClip AudioClip;
    public AudioTypeEnum AudioType;
}
