using Common;
using Framewerk.Managers;
using strange.extensions.promise.api;
using strange.extensions.promise.impl;

namespace LevelEditor
{
    public interface IUserConfirmationService
    {
        Promise<bool> GetUserOverwrite(string fileName);
        Promise<bool> BundleDelete(string budnleName);
    }

    public class UserConfirmationService : IUserConfirmationService
    {
        [Inject] public IUiManager UiManager { get; set; }
        private const string OVERWRITE_HEADER = "Are you sure?";
        private const string OVERWRITE_MESSAGE = "will be ovewriten. Do you want to continue ?";
        
        private const string BUNDLE_DELETE_HEADER = "Are you sure?";
        private const string BUNDLE_DELETE_MESSAGE = "will be deleted. Do you want to continue ?";
        
        public Promise<bool> GetUserOverwrite(string fileName)
        {
            // create generic popup
            // passs text and promise 
            // on accpet
            var overidePromise = new Promise<bool>();
            var popupView = UiManager.InstantiateView<GenericInfoPopupView>("");
            popupView.Data = new GenericPopupData(
                ()=> overidePromise.Dispatch(true),  
                ()=> overidePromise.Dispatch(false),
                "Yes", "No", 
                 $"<color=\"red\">{fileName}</color> {OVERWRITE_MESSAGE}", OVERWRITE_HEADER);
            popupView.InitPopup();
            return  overidePromise;

        }
        
        public Promise<bool> BundleDelete(string budnleName)
        {
            // create generic popup
            // passs text and promise 
            // on accpet
            var overidePromise = new Promise<bool>();
            var popupView = UiManager.InstantiateView<GenericInfoPopupView>("");
            popupView.Data = new GenericPopupData(
                ()=> overidePromise.Dispatch(true),  
                ()=> overidePromise.Dispatch(false),
                "Yes", "No", 
                $"<color=\"red\">{budnleName}</color> {BUNDLE_DELETE_MESSAGE}", BUNDLE_DELETE_HEADER);
            popupView.InitPopup();
            return  overidePromise;
            ;
        }
        
        
    }
}