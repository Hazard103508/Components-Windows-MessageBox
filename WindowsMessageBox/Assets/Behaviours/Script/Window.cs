using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rosso.Behaviours
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class Window : MonoBehaviour
    {
        #region Objects
        public KeyCode closeKey = KeyCode.Escape;
        public bool destroyOnClose;

        [SerializeField] private Text _captionText;
        [SerializeField] private Button _buttonExit;
        private TransitionFade _transition;
        private Action<Window> callback;

        public UnityEvent Confirm = new UnityEvent();
        #endregion

        #region Properties
        /// <summary>
        /// Duracion en segundos del efecto fade para mostrar un ocultar el control
        /// </summary>
        public float FadeTime { get => _transition.FadeTime; set => _transition.FadeTime = value; }
        /// <summary>
        /// Obtiene o setea el titulo de la ventana
        /// </summary>
        public string Caption
        {
            get => _captionText.text;
            set => _captionText.text = value;
        }
        public TransitionFade.ControlState State { get => _transition.State; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _transition = GetComponent<TransitionFade>();
            _buttonExit?.onClick.AddListener(Close);
        }
        private void Update()
        {
            if (!Application.isPlaying)
                return;

            if (this.closeKey != KeyCode.None && UnityEngine.Input.GetKeyDown(this.closeKey))
                Close();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Cierra la ventana
        /// </summary>
        public void Close()
        {
            if (_transition.State == TransitionFade.ControlState.Active)
            {
                _transition.Hide();

                if (this.callback != null)
                    this.callback.Invoke(this);
            }
        }
        /// <summary>
        /// abre la ventana
        /// </summary>
        public void Show(Action<Window> callback = null)
        {
            this.callback = callback;
            _transition.Show();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
        #endregion

        #region Strcutres
        [Serializable]
        public class WindowEvent : UnityEvent<Window>
        {
        }
        #endregion
    }
}