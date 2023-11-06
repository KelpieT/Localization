using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viter.Dictionary;
using System.Globalization;
using TMPro;

namespace Viter.Localization
{
    public class MainMonoLocalization : MonoBehaviour
    {
        private const string DEFAULT_LANG = "eng";
        private const string ERROR_LOC = "cant find locaization";
        public static event Action OnUpdateLanguage;
        public static MainMonoLocalization instance;
        [SerializeField] private LocalizationDictionariesContainer<string> stringContainer;
        [SerializeField] private LocalizationDictionariesContainer<TMP_FontAsset> tmpFontContainer;
        [SerializeField] private LocalizationDictionariesContainer<Font> fontContainer;

        private string currentLangISO3;

        public LocalizationDictionariesContainer<string> StringContainer { get => stringContainer; set => stringContainer = value; }
        public LocalizationDictionariesContainer<TMP_FontAsset> TmpFontContainer { get => tmpFontContainer; set => tmpFontContainer = value; }
        public LocalizationDictionariesContainer<Font> FontContainer { get => fontContainer; set => fontContainer = value; }

        private void OnEnable()
        {
            instance = this;
            SetDeviceLang();
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

        public TMP_FontAsset GetTmpFont(string key)
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
    }
}
