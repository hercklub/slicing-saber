using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class GenericInfoPopupView : View
    {
        public Button OkeyButton;
        public GameObject ButtonContainer;
        public Button YesButton;
        public Button NoButton;

        public TMP_Text YesText;
        public TMP_Text NoText;
        public TMP_Text OkeyText;

        public TMP_Text Header;
        public TMP_Text Message;

        public GenericPopupData Data;

        public void InitPopup()
        {
            Header.text = Data.Header;
            Message.text = Data.Message;

            // one button 
            if (Data.DenyAction == null)
            {
                OkeyButton.gameObject.SetActive(true);
                OkeyText.text = Data.ConfirmText;
            }
            else
            {
                ButtonContainer.gameObject.SetActive(true);
                YesButton.gameObject.SetActive(true);
                NoButton.gameObject.SetActive(true);

                YesText.text = Data.ConfirmText;
                NoText.text = Data.DenyText;
            }
        }
    }
}