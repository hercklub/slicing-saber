using System;
using Common;
using Framewerk.Managers;
using Framewerk.UI;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class SaveModePopupMediator : ExtendedMediator
    {
        [Inject] public SaveModePopupView View { get; set; }
        
        [Inject] public NewLevelSignal NewLevelSignal { get; set; }
        [Inject] public SaveAsSignal SaveAsSignal { get; set; }
        [Inject] public SaveLevelSignal SaveLevelSignal { get; set; }
        [Inject] public IUiManager UiManager { get; set; }
        [Inject] public LoadLevelSignal LoadLevelSignal { get; set; }
        
        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }
        

        public override void OnRegister()
        {
            base.OnRegister();
            AddButtonListener(View.NewButton, NewButtonHandler);
            AddButtonListener(View.SaveAsButton, SaveAsHandler);
            AddButtonListener(View.LoadButton, LoadButtonHandler);
            AddButtonListener(View.LoadBundleButton, LoadBundleButtonHandler);

            EditModeChangedSignal.AddListener(EditModeChangedHandler);
            View.SetEnabled(false);
        }
        
        private void EditModeChangedHandler(EditorMode type)
        {
            View.SetEnabled(type == EditorMode.Save);
        }

        
        private void NewButtonHandler()
        {
            NewLevelSignal.Dispatch();
        }

        private void LoadButtonHandler()
        {
            LoadLevelSignal.Dispatch();
        }

        private void LoadBundleButtonHandler()
        {
            UiManager.InstantiateView<BundleListMenuView>(ResourcePath.LEVEL_EDITOR);
        }
        private void SaveButtonHadnler()
        {
            // curently not used ... save as act as both saveAs  acts the same way
            //SaveLevelSignal.Dispatch();
        }
        private void SaveAsHandler()
        {
            SaveAsSignal.Dispatch();
        }



    }
}