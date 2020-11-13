using System;
using System.Collections.Generic;
using Framewerk.UI.Components;
using strange.extensions.mediation.impl;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framewerk.UI
{
    public class ExtendedMediator : Mediator
    {
        [Inject] public ButtonClickedSignal ButtonClickedSignal { get; set; }

        // Handlers
        private Dictionary<GameObject, UnityAction> ButtonHandlers = new Dictionary<GameObject, UnityAction>();
        private Dictionary<GameObject, UnityAction<bool>> ToggleHandlers = new Dictionary<GameObject, UnityAction<bool>>();
        private Dictionary<GameObject, UnityAction<bool, Vector2>> PointerHandlers = new Dictionary<GameObject, UnityAction<bool, Vector2>>();
        private Dictionary<GameObject, UnityAction<float>> SliderHandlers = new Dictionary<GameObject, UnityAction<float>>();
        private Dictionary<GameObject, UnityAction<string>> InputHandlers = new Dictionary<GameObject, UnityAction<string>>();
        private Dictionary<GameObject, UnityAction<string>> InputTmpHandlers = new Dictionary<GameObject, UnityAction<string>>();
        private Dictionary<GameObject, Action<DragData>> DragHandlers = new Dictionary<GameObject, Action<DragData>>();
        private Dictionary<GameObject, UnityAction<int>> DropdownHandlers = new Dictionary<GameObject, UnityAction<int>>();
        private Dictionary<GameObject, UnityAction<int>> DropdownTmpHandlers = new Dictionary<GameObject, UnityAction<int>>();


        public override void OnRemove()
        {
            RemoveListeners();
            base.OnRemove();
        }

        #region RemovingListeners

        protected void RemoveListeners()
        {
            RemoveButtonListeners();
            RemoveToggleListeners();
            RemoveSliderListeners();
            RemoveInputListeners();
            RemoveTmpInputListeners();
            RemovePointerListeners();
            RemoveDragListeners();
            RemoveDropdownListeners();
            RemoveTmpDropdownListeners();

        }

        protected void RemoveSliderListeners()
        {
            foreach (var pair in SliderHandlers)
            {
                var s = pair.Key.GetComponent<Slider>();
                s.onValueChanged.RemoveListener(pair.Value);
            }
            SliderHandlers.Clear();
        }

        protected void RemoveButtonListeners()
        {
            foreach (var pair in ButtonHandlers)
            {
                var b = pair.Key.GetComponent<Button>();
                b.onClick.RemoveListener(pair.Value);
            }
            ButtonHandlers.Clear();
        }

        protected void RemoveToggleListeners()
        {
            foreach (var pair in ToggleHandlers)
            {
                var b = pair.Key.GetComponent<Toggle>();
                b.onValueChanged.RemoveListener(pair.Value);
            }
            ToggleHandlers.Clear();
        }

        protected void RemoveInputListeners()
        {
            foreach (var pair in InputHandlers)
            {
                var b = pair.Key.GetComponent<InputField>();
                b.onValueChanged.RemoveListener(pair.Value);
            }
            InputHandlers.Clear();
        }
        
        protected void RemoveTmpInputListeners()
        {
            foreach (var pair in InputTmpHandlers)
            {
                var b = pair.Key.GetComponent<TMP_InputField>();
                b.onValueChanged.RemoveListener(pair.Value);
            }
            InputTmpHandlers.Clear();
        }

        protected void RemovePointerListeners()
        {
            foreach (var pair in PointerHandlers)
            {
                var m = pair.Key.GetComponent<PointerElement>();
                m.OnPointerChanged.RemoveListener(pair.Value);
            }
            PointerHandlers.Clear();
        }

        protected void RemoveDragListeners()
        {
            foreach (var pair in DragHandlers)
            {
                var m = pair.Key.GetComponent<DragElement>();
                m.DragChangedSignal.RemoveListener(pair.Value);
            }
            DragHandlers.Clear();
        }
        
        protected void RemoveDropdownListeners()
        {
            foreach (var pair in DropdownHandlers)
            {
                var m = pair.Key.GetComponent<Dropdown>();
                m.onValueChanged.RemoveListener(pair.Value);
            }
            DropdownHandlers.Clear();
        }
        
        protected void RemoveTmpDropdownListeners()
        {
            foreach (var pair in DropdownTmpHandlers)
            {
                var m = pair.Key.GetComponent<TMP_Dropdown>();
                m.onValueChanged.RemoveListener(pair.Value);
            }
            DropdownTmpHandlers.Clear();
        }

        #endregion

        #region Adding Listeners

        protected void AddButtonListener(Button button, Action func)
        {
            UnityAction internalAction = () => { func(); ButtonClickedSignal.Dispatch(); };
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(internalAction);
            ButtonHandlers[button.gameObject] = internalAction;
        }

        protected void AddSliderListener(Slider slider, Action<float> func)
        {
            UnityAction<float> internalAction = (val) => { func(val); };

            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(internalAction);
            SliderHandlers[slider.gameObject] = internalAction;
        }

        protected void AddToggleListener(Toggle toggle, Action<bool> func)
        {
            UnityAction<bool> internalAction = (val) => { func(val); };

            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(internalAction);
            ToggleHandlers[toggle.gameObject] = internalAction;
        }

        protected void AddInputListener(InputField input, Action<string> func)
        {
            UnityAction<string> internalAction = (val) => { func(val); };

            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener(internalAction);
            InputHandlers[input.gameObject] = internalAction;
        }
        
        protected void AddTmpInputListener(TMP_InputField input, Action<string> func)
        {
            UnityAction<string> internalAction = (val) => { func(val); };
            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener(internalAction);
            InputTmpHandlers[input.gameObject] = internalAction;
        }
        
        protected void AddPointerListener(PointerElement pointerElement, Action<bool, Vector2> func)
        {
            UnityAction<bool, Vector2> internalAction = (state, pos) => { func(state, pos); };

            pointerElement.OnPointerChanged.RemoveAllListeners();
            pointerElement.OnPointerChanged.AddListener(internalAction);
            PointerHandlers[pointerElement.gameObject] = internalAction;
        }

        protected void AddDragListener(DragElement dragElement, Action<DragData> handler)
        {
            dragElement.DragChangedSignal.RemoveAllListeners();
            dragElement.DragChangedSignal.AddListener(handler);

            DragHandlers[dragElement.gameObject] = handler;
        }
        
        protected void AddDropdownListener(Dropdown dropdown, Action<int> func)
        {
            UnityAction<int> internalAction = (val) => { func(val); };
            
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener(internalAction);
            
            DropdownHandlers[dropdown.gameObject] = internalAction;
        }
        
        protected void AddTmpDropdownListener(TMP_Dropdown dropdown, Action<int> func)
        {
            UnityAction<int> internalAction = (val) => { func(val); };
            
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener(internalAction);
            
            DropdownTmpHandlers[dropdown.gameObject] = internalAction;
        }

        #endregion
    }
}