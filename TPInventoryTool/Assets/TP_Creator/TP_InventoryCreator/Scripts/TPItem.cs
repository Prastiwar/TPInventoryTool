using UnityEngine;

namespace TP.Inventory
{
    public class TPItem : ScriptableObject
    {
        [SerializeField] int _ID;
        public int ID { get { return _ID; } }

        public Sprite Sprite;
        public string Name;
        public string Description;
        public float Worth;
        public TPType Type;
        public TPModifier[] Modifiers;
        public TPSound[] Sounds;
        public TPSlot OnSlot
        {
            get { return _OnSlot; }
            set
            {
                if (_OnSlot == null)
                    PlayGetItem();
                _OnSlot = value;
            }
        }

        [SerializeField] TPSlot _OnSlot;

        void PlayGetItem()
        {
            AudioSource source = FindObjectOfType<AudioSource>();
            if (source == null)
            {
                Debug.LogError("There is no Audio Source in scene");
                return;
            }

            int length = Sounds.Length;
            for (int i = 0; i < length; i++)
            {
                if (Sounds[i].AudioType == TPSound.AudioTypeEnum.GetItem)
                {
                    source.PlayOneShot(Sounds[i].AudioClip);
                    return;
                }
            }
        }
    }

}