using System.Collections.Generic;
using UnityEngine;

namespace VRKeyboard.Runtime
{
	// paths: KeyboardKeysConfigEditor, KeyboardKeysConfig.uxml

	[CreateAssetMenu(fileName = "KeyboardKeysConfig", menuName = "VRKeyboard/KeyboardKeysConfig")]
    public class KeyboardKeysConfig : ScriptableObject
    {
        public class KeyData
        {
            public string Content { get; set; }
        }

        public List<List<KeyData>> Keys = new List<List<KeyData>>();
        // additional array for the new keys?
        public int TestInt = 0;

        public void AddKey()
        {
            if (Keys.Count <= 0)
            {
                Keys.Add(new List<KeyData>());
            }

            Keys[Keys.Count - 1].Add(new KeyData());
        }

        public void Clear()
        {
            Keys = new List<List<KeyData>>();
        }
    }
}
