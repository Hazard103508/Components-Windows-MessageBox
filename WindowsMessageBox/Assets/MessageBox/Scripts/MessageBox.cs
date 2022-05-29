using UnityEngine;
using UnityEngine.UI;
using Rosso.Behaviours;
using Rosso.Extensions;
using System.Collections;
using System;

namespace Rosso.MessageBox
{
    public delegate void MessageBoxCallback(MessageBoxResult result);

    public class MessageBox : MonoBehaviour
    {
        #region Objects
        [SerializeField] private Text messageLabel;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button[] buttons;
        [SerializeField] private Text[] buttonsText;
        [SerializeField] private Sprite[] icons;

        private Window Window;
        #endregion

        #region Properties
        /// <summary>
        /// Resultado de confirmacion de message box
        /// </summary>
        public MessageBoxResult Result { get; private set; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            Window = GetComponent<Window>();
            Result = MessageBoxResult.Cancel;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Muestra un panel de mensaje
        /// </summary>
        /// <param name="message">mensaje a mostrar</param>
        /// <param name="caption">Titulo de la ventana</param>
        /// <param name="icon">Icono del mensaje</param>
        /// <param name="buttons">botones de conformacion</para
        /// <param name="selected">Boton seleccionado por defecto</param>
        /// <param name="callback">Funsion a ejecutar al cerrar el mensaje</param>
        public void Show(string message, string caption, MessageBoxIcons icon, MessageBoxButtons buttons, MessageBoxButtonIndex selected, MessageBoxCallback callback)
        {
            this.messageLabel.text = message;
            Window.Caption = caption;

            InitializeButtons(buttons);
            InitializeIcon(icon);
            SetDefaultButton(selected);

            Window.Show(win => callback?.Invoke(this.Result));
        }
        private void InitializeButtons(MessageBoxButtons btn)
        {
            switch (btn)
            {
                case MessageBoxButtons.OK:
                    buttonsText[0].text = "OK";
                    this.buttons[0].onClick.AddListener(() => this.Result = MessageBoxResult.OK);
                    this.buttons[1].gameObject.SetActive(false);
                    this.buttons[2].gameObject.SetActive(false);
                    break;
                case MessageBoxButtons.OKCancel:
                    buttonsText[0].text = "OK";
                    buttonsText[1].text = "Cancel";
                    this.buttons[0].onClick.AddListener(() => this.Result = MessageBoxResult.OK);
                    this.buttons[1].onClick.AddListener(() => this.Result = MessageBoxResult.Cancel);
                    this.buttons[2].gameObject.SetActive(false);
                    break;
                case MessageBoxButtons.YesNo:
                    buttonsText[0].text = "Yes";
                    buttonsText[1].text = "No";
                    this.buttons[0].onClick.AddListener(() => this.Result = MessageBoxResult.Yes);
                    this.buttons[1].onClick.AddListener(() => this.Result = MessageBoxResult.No);
                    this.buttons[2].gameObject.SetActive(false);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    buttonsText[0].text = "Yes";
                    buttonsText[1].text = "No";
                    buttonsText[2].text = "Cancel";
                    this.buttons[0].onClick.AddListener(() => this.Result = MessageBoxResult.Yes);
                    this.buttons[1].onClick.AddListener(() => this.Result = MessageBoxResult.No);
                    this.buttons[2].onClick.AddListener(() => this.Result = MessageBoxResult.Cancel);
                    break;
            }

            UnityEngine.Events.UnityAction onClose = () => Window.Close();
            this.buttons[0].onClick.AddListener(onClose);
            this.buttons[1].onClick.AddListener(onClose);
            this.buttons[2].onClick.AddListener(onClose);
        }
        private void SetDefaultButton(MessageBoxButtonIndex selected)
        {
            var transition = GetComponent<TransitionFade>();
            transition.StateChanged.AddListener(() =>
            {
                if (transition.State == TransitionFade.ControlState.Active)
                {
                    Array.ForEach(this.buttons, b => b.interactable = true);
                    this.buttons[(int)selected].Select();
                }
            });
        }
        private void InitializeIcon(MessageBoxIcons icon)
        {
            if (icon == MessageBoxIcons.None)
            {
                Destroy(iconImage.gameObject);
                ((RectTransform)messageLabel.transform).SetRight(0);
            }
            else
                iconImage.sprite = this.icons[(int)icon - 1];
        }
        #endregion
    }
}