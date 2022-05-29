using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rosso.Demo
{
    public class Init : MonoBehaviour
    {
        void Start()
        {
            // inicializo el WindowsHandler una unica vez al iniciar la aplicacion
            Invoke("LoadDemo", 1);
        }

        private void LoadDemo()
        {
            SceneManager.LoadScene("Demo");
        }
    }
}