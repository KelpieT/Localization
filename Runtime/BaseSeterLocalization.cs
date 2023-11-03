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

        protected abstract void UpdateComponent();
    }
}
