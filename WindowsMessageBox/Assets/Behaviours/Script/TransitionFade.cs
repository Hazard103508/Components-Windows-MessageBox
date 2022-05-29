using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Rosso.Behaviours
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TransitionFade : MonoBehaviour
    {
        #region Objects
        private CanvasGroup canvasGroup;
        private ControlState _state;
        private Coroutine showCO;
        #endregion

        #region Events
        /// <summary>
        /// Se desencadena al cambiar el estado del control (estado anterior, estado actual)
        /// </summary>
        public UnityEvent StateChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Duracion en segundo del efecto fade para mostrar un ocultar el control
        /// </summary>
        public float FadeTime { get; set; }
        /// <summary>
        /// Estado del control
        /// </summary>
        public ControlState State
        {
            get => _state;
            private set
            {
                _state = value;
                StateChanged.Invoke();
            }
        }
        #endregion

        #region Unity Methods
        void Awake()
        {
            canvasGroup = this.GetComponent<CanvasGroup>();

            FadeTime = 0.2f;
            State = ControlState.Inactive;
            this.Hide(false); // inicia inactvo
        }
        #endregion

        #region Methods
        /// <summary>
        /// Muestra el control
        /// </summary>
        public void Show()
        {
            if (showCO != null)
                StopCoroutine(showCO);

            showCO = StartCoroutine(ShowCO());
        }
        /// <summary>
        /// Oculta el control
        /// </summary>
        public void Hide(bool showTransition = true)
        {
            if (showCO != null)
                StopCoroutine(showCO);

            if (showTransition)
                StartCoroutine(HideCO());
            else
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                this._state = ControlState.Inactive;
            }
        }
        /// <summary>
        /// Desahilita el control
        /// </summary>
        public void Disable()
        {
            if (this.State != TransitionFade.ControlState.Disabled && this.State != TransitionFade.ControlState.Inactive)
            {
                this.canvasGroup.interactable = false;
                this.State = ControlState.Disabled;
            }
        }
        /// <summary>
        /// Habilita el control
        /// </summary>
        public void Enable()
        {
            if (this.State == TransitionFade.ControlState.Disabled)
                this.canvasGroup.interactable = true;
        }
        #endregion

        #region Coroutine Methods
        /// <summary>
        /// Muestra el control
        /// </summary>
        private IEnumerator ShowCO()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            this.State = ControlState.Showing;

            float timer = 0;
            while (timer < this.FadeTime)
            {
                if (this.State != ControlState.Disabled)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Clamp01(timer / this.FadeTime);
                }

                yield return null;
            }

            this.State = ControlState.Active;
        }
        /// <summary>
        /// Oculta el control
        /// </summary>
        private IEnumerator HideCO()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            this.State = ControlState.Hiding;

            float timer = 0;
            while (timer < this.FadeTime)
            {
                if (this.State != ControlState.Disabled)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = 1 - Mathf.Clamp01(timer / this.FadeTime);
                }
                yield return null;
            }

            this.State = ControlState.Inactive;
        }
        #endregion

        #region Structures
        public enum ControlState
        {
            Inactive,
            Showing,
            Active,
            Hiding,
            Disabled
        }
        #endregion
    }
}