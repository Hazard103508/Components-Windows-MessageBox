using Rosso.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosso.Demo
{
    public class SampleWindows : MonoBehaviour
    {
        // agregar logica particular de ventana aqui
        // el componente windows se encarga de abrir y cerrar la ventana

        [SerializeField] private Window window;

        void Awake()
        {
            window = GetComponent<Window>();
        }

        public void OnButtonTestClick()
        {
            window.Close();
        }
    }
}