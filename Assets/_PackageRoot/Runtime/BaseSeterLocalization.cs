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
            UpdateComponent();
        }

        public void SetFontKey(string fontKey)
        {
            this.fontKey = fontKey;
            UpdateComponent();
        }

        protected abstract void UpdateComponent();
    }
}
