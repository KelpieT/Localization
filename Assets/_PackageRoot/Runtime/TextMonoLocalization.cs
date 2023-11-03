using UnityEngine;
using UnityEngine.UI;

namespace Viter.Localization
{
    [RequireComponent(typeof(Text)), DisallowMultipleComponent]
    public class TextMonoLocalization : BaseSeterLocalization
    {
        [SerializeField] private Text text;
        protected override void UpdateComponent()
        {
            text.text = MainMonoLocalization.instance.GetString(key);
            if (translateFont)
            {
                text.font = MainMonoLocalization.instance.GetFont(fontKey);
            }
        }

        private void OnValidate()
        {
            if (text == null)
            {
                text = GetComponent<Text>();
            }
        }
    }
}
