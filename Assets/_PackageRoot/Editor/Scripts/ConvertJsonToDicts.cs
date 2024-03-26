using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
        private string json;
        private Object jsonTextAsset;
        private Object font;
        private Object fontTmp;
        private bool drawDefaultInspector;
        public override void OnInspectorGUI()
        {
            if (drawDefaultInspector)
            {
                DrawDefaultInspector();
            }
            GUILayout.BeginHorizontal();
            drawDefaultInspector = EditorGUILayout.Toggle(drawDefaultInspector);
            GUILayout.Label("drawDefaultInspector");
            GUILayout.EndHorizontal();
            GUILayout.Label("Localization JSON");
            // json = EditorGUILayout.TextArea(json, GUILayout.Height(150));
            jsonTextAsset = EditorGUILayout.ObjectField(jsonTextAsset, typeof(TextAsset), jsonTextAsset);
            font = EditorGUILayout.ObjectField(font, typeof(Font), font);
            fontTmp = EditorGUILayout.ObjectField(fontTmp, typeof(TMP_FontAsset), fontTmp);
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

                        Undo.RecordObject(mainMonoLocalization, "Set localization from json file");
                        mainMonoLocalization.StringContainer.SetAllDicts(allDicts);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(mainMonoLocalization);

                    }
                }
            }
            if (GUILayout.Button("Set default fonts"))
            {
                if (target is MainMonoLocalization mainMonoLocalization)
                {
                    Undo.RecordObject(mainMonoLocalization, "Set DefaultFonts");
                    DefaultFonts(mainMonoLocalization);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(mainMonoLocalization);
                }
            }
            if (GUILayout.Button("Clear json"))
            {
                if (jsonTextAsset != null)
                {
                    ClearJson();
                }
            }
            if (GUILayout.Button("Check wrongKeys"))
            {
                MainMonoLocalization m = target as MainMonoLocalization;
                m.SetLang(MainMonoLocalization.DEFAULT_LANG);
                CheckKeys<TextMonoLocalization>(m);
                CheckKeys<TmpTextMonoLocalization>(m);
            }
        }

        private static void CheckKeys<T>(MainMonoLocalization m) where T : BaseSeterLocalization
        {
            T[] locTexts = GameObject.FindObjectsOfType<T>(true);
            for (int i = 0; i < locTexts.Length; i++)
            {
                string key = locTexts[i].GetKey();
                if (string.IsNullOrWhiteSpace(key))
                {
                    Debug.LogError($"name\t{locTexts[i].name}\tkey:\t{key}");
                }
                if (!m.StringContainer.CurrentDict.Dict.ContainsKey(key))
                {
                    Debug.LogError($"name\t{locTexts[i].name}\tkey:\t{key}");
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
            SerializableDicrionary<string, SerializableDicrionary<string, TmpData>> allDictsTmp = new();

            List<string> globalKeys = new();
            List<SerializableDicrionary<string, Font>> dicts = new();
            List<SerializableDicrionary<string, TmpData>> dictsTmp = new();

            Font defaultFont = font as Font;
            TMP_FontAsset defaultFontTmp = fontTmp as TMP_FontAsset;

            foreach (KeyValuePair<string, SerializableDicrionary<string, string>> locDict in mainMonoLocalization.StringContainer.AllDicts.Dict)
            {
                SerializableDicrionary<string, Font> fontDict = new();
                SerializableDicrionary<string, TmpData> fontTmpDict = new();

                fontDict.SetPairs_EDITOR_ONLY(new string[] { MainMonoLocalization.DEFAULT_FONT }, new Font[] { defaultFont });
                fontTmpDict.SetPairs_EDITOR_ONLY(new string[] { MainMonoLocalization.DEFAULT_FONT }, new TmpData[] { new(){ tmpFontAsset = defaultFontTmp } });

                globalKeys.Add(locDict.Key);
                dicts.Add(fontDict);
                dictsTmp.Add(fontTmpDict);
            }

            allDicts.SetPairs_EDITOR_ONLY(globalKeys.ToArray(), dicts.ToArray());
            allDictsTmp.SetPairs_EDITOR_ONLY(globalKeys.ToArray(), dictsTmp.ToArray());
            mainMonoLocalization.FontContainer.SetAllDicts(allDicts);
            mainMonoLocalization.TmpFontContainer.SetAllDicts(allDictsTmp);
        }

        private void ClearJson()
        {
            string s = (jsonTextAsset as TextAsset).text;

            Regex regex = new(@"\\u200b");
            string newJson = regex.Replace(s, "");

            Regex regex2 = new(@"\\u200c");
            newJson = regex2.Replace(newJson, "");

            Regex regex3 = new(@"\\u200d");
            newJson = regex3.Replace(newJson, "");


            string path = Path.Combine(Application.dataPath, "../", AssetDatabase.GetAssetPath(jsonTextAsset));
            byte[] jsBytes = Encoding.UTF8.GetBytes(newJson);
            File.WriteAllBytes(path, jsBytes);
            AssetDatabase.Refresh();
        }
    }
}

#endif