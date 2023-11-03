using TMPro;
using UnityEngine;

namespace Viter.Localization
{
    [RequireComponent(typeof(TMP_Text)), DisallowMultipleComponent]
    public class TmpTextMonoLocalization : BaseSeterLocalization
    {
        [SerializeField] private TMP_Text text;
        protected override void UpdateComponent()
        {
            text.text = MainMonoLocalization.instance.GetString(key);
            if (translateFont)
            {
                text.font = MainMonoLocalization.instance.GetTmpFont(fontKey);
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
