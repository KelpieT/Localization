using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
namespace Viter.Localization.Editor
{
    [System.Serializable]
    public class LocalizationDatas
    {
        public List<LanguageData> LocalizationData;
    }

    [System.Serializable]
    public class LanguageData
    {
        public string lang;
        public List<Loc> loc;
    }

    [System.Serializable]
    public class Loc
    {
        public string key;
        public string value;
    }


}

#endif