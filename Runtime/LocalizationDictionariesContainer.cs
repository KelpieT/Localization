using System;
using System.Collections.Generic;
using UnityEngine;
using Viter.Dictionary;

namespace Viter.Localization
{
    [Serializable]
    public class LocalizationDictionariesContainer<TValue>
    {
        [SerializeField] private SerializableDicrionary<string, SerializableDicrionary<string, TValue>> allDicts;
        private SerializableDicrionary<string, TValue> currentDict;

        public SerializableDicrionary<string, TValue> CurrentDict { get => currentDict; }
        public SerializableDicrionary<string, SerializableDicrionary<string, TValue>> AllDicts { get => allDicts; }

        public string SetLangDict(string langISO3, string defaultLang)
        {
            if (allDicts.Dict.ContainsKey(langISO3))
            {
                currentDict = allDicts.Dict[langISO3];
                return langISO3;
            }
            else if (allDicts.Dict.ContainsKey(defaultLang))
            {
                currentDict = allDicts.Dict[defaultLang];
                return defaultLang;
            }
            else
            {
                Debug.LogError("cant find loc key");
                return "";
            }
        }
#if UNITY_EDITOR
        public void SetAllDicts(SerializableDicrionary<string, SerializableDicrionary<string, TValue>> allDicts)
        {
            this.allDicts = allDicts;
        }
#endif

    }
}
