using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Viter.Localization
{
    public class LanguageSelectionContent : MonoBehaviour
    {
        public const string LANG_KEY = "lang_";
        [SerializeField] private LanguageSelectionButton languageSelectionButtonPrefab;
        [SerializeField] private Transform parentForButtons;
        private List<LanguageSelectionButton> languageSelectionButtons = new();
        
        private void Start()
        {
            CreateContent();
        }

        private void CreateContent()
        {
            List<string> allLangs = MainMonoLocalization.instance.GetAllLanguages();
            string currentLang = MainMonoLocalization.instance.GetCurrentLanguage();
            for (int i = 0; i < allLangs.Count; i++)
            {
                LanguageSelectionButton languageSelectionButton = Instantiate(languageSelectionButtonPrefab, parentForButtons);
                string label = MainMonoLocalization.instance.GetString($"{LANG_KEY}{allLangs[i]}");
                TMP_FontAsset fontAsset = MainMonoLocalization.instance.TmpFontContainer
                    .AllDicts.Dict[allLangs[i]]
                    .Dict[MainMonoLocalization.DEFAULT_FONT];
                languageSelectionButton.Init(allLangs[i], label, fontAsset);
                languageSelectionButton.OnLanguageSelect += LanguageSelect;
                languageSelectionButtons.Add(languageSelectionButton);
            }
        }

        private void LanguageSelect(string lang)
        {
            MainMonoLocalization.instance.SetLang(lang);
            UpdateContent(lang);
        }

        private void UpdateContent(string lang)
        {
            for (int i = 0; i < languageSelectionButtons.Count; i++)
            {
                languageSelectionButtons[i].SetActive(lang == languageSelectionButtons[i].lang);
            }
        }

        public float GetPositionInContent(string lang)
        {
            int i = 0;
            for (; i < languageSelectionButtons.Count; i++)
            {
                if (lang == languageSelectionButtons[i].lang)
                {
                    break;
                }
            }
            float pos = (i + 1) / (float)languageSelectionButtons.Count;
            float invertYPos = 1 - pos;
            return invertYPos;
        }
    }
}
