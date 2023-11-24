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
            text.font = MainMonoLocalization.instance.GetTmpFont(fontKey);
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
