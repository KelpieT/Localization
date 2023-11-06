using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Viter.Dictionary;

#if UNITY_EDITOR
namespace Viter.Localization.Editor
{
    [CustomEditor(typeof(MainMonoLocalization))]
    public class ConvertJsonToDicts : UnityEditor.Editor
    {
        private const string DEFAULT_FONT = "defaultFont";
        private string json;
        private Object jsonTextAsset;
        private Object font;
        private Object fontTmp;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Label("Localization JSON");
            // json = EditorGUILayout.TextArea(json, GUILayout.Height(150));
            jsonTextAsset = EditorGUILayout.ObjectField(jsonTextAsset, typeof(TextAsset));
            font = EditorGUILayout.ObjectField(font, typeof(Font));
            fontTmp = EditorGUILayout.ObjectField(fontTmp, typeof(TMP_FontAsset));
            // if (!string.IsNullOrWhiteSpace(json))
            // {
            //     if (GUILayout.Button("Set localization from json"))
            //     {
            //         if (target is MainMonoLocalization mainMonoLocalization)
            //         {
            //             SerializableDicrionary<string, SerializableDicrionary<string, string>> allDicts = FromJson();
            //             mainMonoLocalization.StringContainer.SetAllDicts(allDicts);
            //         }
            //     }
            // }
            if (jsonTextAsset != null)
            {
                if (GUILayout.Button("Set localization from json file"))
                {
                    if (target is MainMonoLocalization mainMonoLocalization)
                    {
                        json = (jsonTextAsset as TextAsset).text;
                        SerializableDicrionary<string, SerializableDicrionary<string, string>> allDicts = FromJson();
                        mainMonoLocalization.StringContainer.SetAllDicts(allDicts);
                    }
                }
            }
            if (GUILayout.Button("Set default fonts"))
            {
                if (target is MainMonoLocalization mainMonoLocalization)
                {
                    DefaultFonts(mainMonoLocalization);
                }
            }
        }

        public SerializableDicrionary<string, SerializableDicrionary<string, string>> FromJson()
        {
            SerializableDicrionary<string, SerializableDicrionary<string, string>> allDicts = new();
            LocalizationDatas localizationData = JsonUtility.FromJson<LocalizationDatas>(json);
            List<string> globalKeys = new();
            List<SerializableDicrionary<string, string>> dicts = new();
            foreach (LanguageData languageData in localizationData.LocalizationData)
            {
                SerializableDicrionary<string, string> dict = new();

                dict.SetPairs_EDITOR_ONLY(languageData.loc, x => x.key, x => x.value);

                globalKeys.Add(languageData.lang);
                dicts.Add(dict);
            }
            allDicts.SetPairs_EDITOR_ONLY(globalKeys.ToArray(), dicts.ToArray());
            return allDicts;
        }

        public void DefaultFonts(MainMonoLocalization mainMonoLocalization)
        {
            SerializableDicrionary<string, SerializableDicrionary<string, Font>> allDicts = new();
            SerializableDicrionary<string, SerializableDicrionary<string, TMP_FontAsset>> allDictsTmp = new();

            List<string> globalKeys = new();
            List<SerializableDicrionary<string, Font>> dicts = new();
            List<SerializableDicrionary<string, TMP_FontAsset>> dictsTmp = new();

            Font defaultFont = font as Font;
            TMP_FontAsset defaultFontTmp = fontTmp as TMP_FontAsset;

            foreach (KeyValuePair<string, SerializableDicrionary<string, string>> locDict in mainMonoLocalization.StringContainer.AllDicts.Dict)
            {
                SerializableDicrionary<string, Font> fontDict = new();
                SerializableDicrionary<string, TMP_FontAsset> fontTmpDict = new();

                fontDict.SetPairs_EDITOR_ONLY(new string[] { DEFAULT_FONT }, new Font[] { defaultFont });
                fontTmpDict.SetPairs_EDITOR_ONLY(new string[] { DEFAULT_FONT }, new TMP_FontAsset[] { defaultFontTmp });

                globalKeys.Add(locDict.Key);
                dicts.Add(fontDict);
                dictsTmp.Add(fontTmpDict);
            }

            allDicts.SetPairs_EDITOR_ONLY(globalKeys.ToArray(), dicts.ToArray());
            allDictsTmp.SetPairs_EDITOR_ONLY(globalKeys.ToArray(), dictsTmp.ToArray());
            mainMonoLocalization.FontContainer.SetAllDicts(allDicts);
            mainMonoLocalization.TmpFontContainer.SetAllDicts(allDictsTmp);

        }
    }
}

#endif