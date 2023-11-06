using UnityEngine;

namespace Viter.Localization
{
    public abstract class BaseSeterLocalization : MonoBehaviour
    {
        [SerializeField] protected string key;
        [SerializeField] protected string fontKey;
        [SerializeField] protected bool translateFont;

        private void OnEnable()
        {
            MainMonoLocalization.OnUpdateLanguage += UpdateComponent;
            UpdateComponent();
        }

        private void OnDisable()
        {
            MainMonoLocalization.OnUpdateLanguage -= UpdateComponent;
        }

        public void SetKey(string key)
        {
            this.key = key;
        }

        public void SetFontKey(string fontKey)
        {
            this.fontKey = fontKey;
        }

        public void SetTranslateFont(bool translateFont)
        {
            this.translateFont = translateFont;
        }

        protected abstract void UpdateComponent();
    }
}
