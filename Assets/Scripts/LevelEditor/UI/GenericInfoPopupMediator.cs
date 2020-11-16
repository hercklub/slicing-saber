using System;
using Framewerk.UI;
using strange.extensions.mediation.impl;

namespace LevelEditor
{
    public class GenericPopupData
    {
        public Action ConfirmAction { get; }
        public Action DenyAction { get; }
        public string ConfirmText { get; }
        public string DenyText { get; }

        public string Message { get; }
        public string Header { get; }


        public GenericPopupData(Action confirmAction, Action denyAction, string confirmText, string denyText,
            string message = "", string header = "")
        {
            ConfirmAction = confirmAction;
            DenyAction = denyAction;
            ConfirmText = confirmText;
            DenyText = denyText;
            Message = message;
            Header = header;
        }

        public GenericPopupData(Action confirmAction, string confirmText,
            string message = "", string header = "")
        {
            ConfirmAction = confirmAction;
            ConfirmText = confirmText;
            Message = message;
            Header = header;
        }
    }

    public class GenericInfoPopupMediator : ExtendedMediator
    {
        [Inject] public GenericInfoPopupView View { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            AddButtonListener(View.OkeyButton, OkeyButtonHandler);
            AddButtonListener(View.YesButton, YesButtonHandler);
            AddButtonListener(View.NoButton, NoButtonHandler);
        }

        
        

        private void OkeyButtonHandler()
        {
            View.Data.ConfirmAction?.Invoke();
            Close();
        }

        private void NoButtonHandler()
        {
            View.Data.DenyAction?.Invoke();
            Close();
        }

        private void YesButtonHandler()
        {
            View.Data.ConfirmAction?.Invoke();
            Close();
        }

        private void Close()
        {
            Destroy(this.gameObject);
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }
    }
}