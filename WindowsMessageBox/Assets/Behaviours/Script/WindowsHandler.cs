using Rosso.MessageBox;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rosso.Behaviours
{
    public class WindowsHandler : MonoBehaviour
    {
        public GameObject messageBoxPrefab;
        public GameObject canvasPrefab;
        private static WindowsHandler _instance;

        public void Awake()
        {
            if (_instance != null)
                Destroy(_instance);

            _instance = this;
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Muestra un panel de mensaje
        /// </summary>
        /// <param name="message">mensaje a mostrar</param>
        /// <param name="caption">Titulo de la ventana</param>
        public static void ShowMessageBox(string message, string caption)
        {
            ShowMessageBox(message, caption, MessageBoxIcons.None, MessageBoxButtons.OK, null);
        }
        /// <summary>
        /// Muestra un panel de mensaje
        /// </summary>
        /// <param name="message">mensaje a mostrar</param>
        /// <param name="caption">Titulo de la ventana</param>
        /// <param name="icon">Icono del mensaje</param>
        public static void ShowMessageBox(string message, string caption, MessageBoxIcons icon)
        {
            ShowMessageBox(message, caption, icon, MessageBoxButtons.OK, null);
        }
        /// <summary>
        /// Muestra un panel de mensaje
        /// </summary>
        /// <param name="message">mensaje a mostrar</param>
        /// <param name="caption">Titulo de la ventana</param>
        /// <param name="icon">Icono del mensaje</param>
        /// <param name="callback">Funsion a ejecutar al cerrar el mensaje</param>
        public static void ShowMessageBox(string message, string caption, MessageBoxIcons icon, MessageBoxCallback callback)
        {
            ShowMessageBox(message, caption, icon, MessageBoxButtons.OK, callback);
        }
        /// <summary>
        /// Muestra un panel de mensaje
        /// </summary>
        /// <param name="message">mensaje a mostrar</param>
        /// <param name="caption">Titulo de la ventana</param>
        /// <param name="icon">Icono del mensaje</param>
        /// <param name="buttons">botones de conformacion</para
        public static void ShowMessageBox(string message, string caption, MessageBoxIcons icon, MessageBoxButtons buttons)
        {
            ShowMessageBox(message, caption, icon, buttons, null);
        }
        /// <summary>
        /// Muestra un panel de mensaje
        /// </summary>
        /// <param name="message">mensaje a mostrar</param>
        /// <param name="caption">Titulo de la ventana</param>
        /// <param name="icon">Icono del mensaje</param>
        /// <param name="buttons">botones de conformacion</para
        /// <param name="callback">Funsion a ejecutar al cerrar el mensaje</param>
        /// <param name="selected">Boton seleccionado por defecto</param>
        public static void ShowMessageBox(string message, string caption, MessageBoxIcons icon, MessageBoxButtons buttons, MessageBoxCallback callback, MessageBoxButtonIndex selected = MessageBoxButtonIndex.Button1)
        {
            var objMessageBox = Instantiate(_instance.messageBoxPrefab);
            var canvas = InstanceCanvas(objMessageBox.GetComponent<Window>());

            var messageBox = objMessageBox.GetComponent<MessageBox.MessageBox>();
            messageBox.Show(message, caption, icon, buttons, selected, callback);
        }

        public static void ShowWindows(Window window, Action<Window> callback = null)
        {
            var canvas = InstanceCanvas(window);
            window.Show(callback);
        }


        private static List<LockedCanvas> LockCanvas()
        {
            var currentCanvas = GameObject.FindObjectsOfType<Canvas>();

            var lockCanvas = new List<LockedCanvas>();
            Array.ForEach(currentCanvas, canvas =>
            {
                var rayCaster = canvas.GetComponent<GraphicRaycaster>();
                lockCanvas.Add(new WindowsHandler.LockedCanvas() { RayCaster = rayCaster, Enabled = rayCaster.enabled });
                rayCaster.enabled = false; // bloqueo la interaccion del usuario con el canvas
            });

            return lockCanvas;
        }
        private static void UnlockCanvas(List<LockedCanvas> lockedCanvas)
        {
            lockedCanvas.ForEach(c => c.RayCaster.enabled = c.Enabled); // restablece el canvas
        }
        private static GameObject InstanceCanvas(Window windows)
        {
            var lockedCanvas = LockCanvas();
            var initParent = windows.transform.parent;

            var canvas = Instantiate(_instance.canvasPrefab);
            windows.transform.SetParent(canvas.transform);
            windows.transform.localScale = Vector3.one;
            windows.transform.localPosition = Vector3.zero;
            var transition = windows.GetComponent<TransitionFade>();

            transition.StateChanged.AddListener(() =>
            {
                if (transition.State == TransitionFade.ControlState.Inactive)
                {
                    if (!windows.destroyOnClose)
                        windows.transform.parent = initParent; // restablesco en padre inicial de la ventana antes de destruir el canvas emergente

                    Destroy(canvas);
                    UnlockCanvas(lockedCanvas);
                }
            });

            return canvas;
        }

        private class LockedCanvas
        {
            public GraphicRaycaster RayCaster { get; set; }
            public bool Enabled { get; set; }
        }
    }
}