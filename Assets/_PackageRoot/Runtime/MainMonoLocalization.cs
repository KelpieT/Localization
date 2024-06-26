using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Viter.Dictionary;

namespace Viter.Localization
{
    public class MainMonoLocalization : MonoBehaviour
    {
        public const string DEFAULT_LANG = "eng";
        public const string DEFAULT_FONT = "defaultFont";
        private const string ERROR_LOC = "cant find locaization";
        public static event Action OnUpdateLanguage;
        public static MainMonoLocalization instance;
        [SerializeField] private LocalizationDictionariesContainer<string> stringContainer;
        [SerializeField] private LocalizationDictionariesContainer<TmpData> tmpFontContainer;
        [SerializeField] private LocalizationDictionariesContainer<Font> fontContainer;

        private string currentLangISO3;

        public LocalizationDictionariesContainer<string> StringContainer { get => stringContainer; set => stringContainer = value; }
        public LocalizationDictionariesContainer<TmpData> TmpFontContainer { get => tmpFontContainer; set => tmpFontContainer = value; }
        public LocalizationDictionariesContainer<Font> FontContainer { get => fontContainer; set => fontContainer = value; }

        private void Awake()
        {
            instance = this;
        }

        public void SetLang(string langISO3)
        {
            currentLangISO3 = stringContainer.SetLangDict(langISO3, DEFAULT_LANG);
            tmpFontContainer.SetLangDict(langISO3, DEFAULT_LANG);
            fontContainer.SetLangDict(langISO3, DEFAULT_LANG);

            OnUpdateLanguage?.Invoke();
        }

        public void SetDeviceLang()
        {
            CultureInfo current = CultureInfo.CurrentCulture;
            string langISO3 = current.ThreeLetterISOLanguageName;
            SetLang(langISO3);
        }

        public string GetString(string key)
        {
            if (stringContainer.CurrentDict.Dict.ContainsKey(key))
            {
                return stringContainer.CurrentDict.Dict[key];
            }
            else
            {
                return ERROR_LOC;
            }
        }

        public TmpData GetTmpFont(string key)
        {
            if (tmpFontContainer.CurrentDict.Dict.ContainsKey(key))
            {
                return tmpFontContainer.CurrentDict.Dict[key];
            }
            else
            {
                return default;
            }
        }

        public Font GetFont(string key)
        {
            if (fontContainer.CurrentDict.Dict.ContainsKey(key))
            {
                return fontContainer.CurrentDict.Dict[key];
            }
            else
            {
                return default;
            }
        }

        public List<string> GetAllLanguages()
        {
            int count = stringContainer.AllDicts.Dict.Count;
            List<string> langs = new(count);
            foreach (var item in stringContainer.AllDicts.Dict)
            {
                langs.Add(item.Key);
            }
            return langs;
        }

        public string GetCurrentLanguage()
        {
            return currentLangISO3;
        }
    }
}
