using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Viter.Localization.Editor
{
    public class LocalizeMenu
    {
        [MenuItem("CONTEXT/Text/Set Viter Localization")]
        public static void AddLocalizationText()
        {
            AddLocalization<TextMonoLocalization, Text>();
        }

        [MenuItem("CONTEXT/TMP_Text/Set Viter Localization")]
        public static void AddLocalizationTMP_Text()
        {
            AddLocalization<TmpTextMonoLocalization, TMP_Text>();
        }

        private static void AddLocalization<TLocComp, TText>()
            where TLocComp : MonoBehaviour
            where TText : MonoBehaviour
        {
            var objects = Selection.gameObjects;
            foreach (var item in objects)
            {
                TText text;
                if (item.TryGetComponent<TText>(out text))
                {
                    TLocComp textMonoLocalization;
                    if (item.TryGetComponent<TLocComp>(out textMonoLocalization))
                    {
                        continue;
                    }
                    else
                    {
                        item.AddComponent<TLocComp>();
                    }
                }
            }
        }
    }
}
