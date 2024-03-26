using TMPro;
using UnityEngine;

namespace Viter.Localization
{
    [RequireComponent(typeof(TMP_Text)), DisallowMultipleComponent]
    public class TmpTextMonoLocalizationFont : BaseSeterLocalization
    {
        [SerializeField] private TMP_Text text;
        protected override void UpdateComponent()
        {
            TmpData tmpData = MainMonoLocalization.instance.GetTmpFont(fontKey);
            text.font = tmpData.tmpFontAsset;
            if(tmpData.fontMaterial != null)
            {
                text.fontSharedMaterial = tmpData.fontMaterial;
            }
        }

        private void OnValidate()
        {
            if (text == null)
            {
                text = GetComponent<TMP_Text>();
            }
        }
    }
}
