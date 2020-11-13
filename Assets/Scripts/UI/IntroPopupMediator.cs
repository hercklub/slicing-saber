using Contexts;
using Framewerk.UI;

namespace UI
{
    public class IntroPopupMediator : ExtendedMediator
    {
        [Inject] public IntroPopupView View { get; set; }
        [Inject] public InitGameSignal InitGameSignal { get; set; }
        public override void OnRegister()
        {
            base.OnRegister();
            
            AddButtonListener(View.OkButton, OkButtonHandler);
        }

        private void OkButtonHandler()
        {
            InitGameSignal.Dispatch();
            Destroy(View.gameObject);
        }
    }
}