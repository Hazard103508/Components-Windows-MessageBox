using Rosso.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Rosso.Demo
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] private Text resultText;
        [SerializeField] private Window sampleWindow;

        public void OnButtonTextClick()
        {
            Behaviours.WindowsHandler.ShowMessageBox("This is an text message", "Text Title");
        }
        public void OnButtonInfoClick()
        {
            Behaviours.WindowsHandler.ShowMessageBox("This is an information message", "Information Title", MessageBox.MessageBoxIcons.Info, OnOnButtonQuestionCallback);
        }
        public void OnButtonErrorClick()
        {
            Behaviours.WindowsHandler.ShowMessageBox("This is an error message", "Error Title", MessageBox.MessageBoxIcons.Error, MessageBox.MessageBoxButtons.OK, OnOnButtonQuestionCallback);
        }
        public void OnButtonWarningClick()
        {
            Behaviours.WindowsHandler.ShowMessageBox("This is an warning message", "Warning Title", MessageBox.MessageBoxIcons.Warning, MessageBox.MessageBoxButtons.OKCancel, OnOnButtonQuestionCallback);
        }
        public void OnButtonQuestionClick()
        {
            Behaviours.WindowsHandler.ShowMessageBox("This is an question message?", "Question Title", MessageBox.MessageBoxIcons.Question, MessageBox.MessageBoxButtons.YesNo, OnOnButtonQuestionCallback, MessageBox.MessageBoxButtonIndex.Button2);
        }
        public void OnButtonQuestion2Click()
        {
            Behaviours.WindowsHandler.ShowMessageBox("This is an question message?", "Question Title", MessageBox.MessageBoxIcons.Question, MessageBox.MessageBoxButtons.YesNoCancel, OnOnButtonQuestionCallback, MessageBox.MessageBoxButtonIndex.Button3);
        }
        public void OnButtonOpenWindowsClick()
        {
            Behaviours.WindowsHandler.ShowWindows(sampleWindow, OnSampleWindowsClose);
        }


        private void OnOnButtonQuestionCallback(MessageBox.MessageBoxResult result)
        {
            resultText.text = result.ToString();
        }
        private void OnSampleWindowsClose(Window window)
        {
            resultText.text = $"Windows '{window.Caption}' closed";
        }
    }
}

