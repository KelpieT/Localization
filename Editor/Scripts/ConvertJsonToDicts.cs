using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Viter.Dictionary;

namespace Viter.Localization.Editor
{
    [CustomEditor(typeof(MainMonoLocalization))]
    public class ConvertJsonToDicts : UnityEditor.Editor
    {
        private string json;
        private Object jsonTextAsset;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Label("Localization JSON");
            // json = EditorGUILayout.TextArea(json, GUILayout.Height(150));
            jsonTextAsset = EditorGUILayout.ObjectField(jsonTextAsset, typeof(TextAsset));
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
    }
}
