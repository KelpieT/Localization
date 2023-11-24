using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace Viter.Localization
{
    public abstract class LanguageSelectionButton : MonoBehaviour
    {
        public event Action<string> OnLanguageSelect;
        public string lang;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text labelText;

        public void Init(string lang, string label, TMP_FontAsset fontAsset)
        {
            this.lang = lang;
            labelText.text = label;
            labelText.font = fontAsset;
            button.onClick.AddListener(SelectLang);
        }

        private void SelectLang()
        {
            OnLanguageSelect?.Invoke(lang);
        }

        public abstract void SetActive(bool active);

    }
}
