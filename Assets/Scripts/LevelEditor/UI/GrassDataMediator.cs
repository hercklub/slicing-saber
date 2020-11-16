using Framewerk.UI;
using Level;
using UnityEngine;

namespace LevelEditor
{
    public class GrassDataMediator : ExtendedMediator
    {
        [Inject] public GrassDataView View { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }


        private int _grassIndex;
        private EditorMode _editorMode;

        public override void OnRegister()
        {
            base.OnRegister();
            //react to event
            EventObjectSelectedSignal.AddListener(EventObjectSelectedHandler);
            EditModeChangedSignal.AddListener(EditModeChangedHandler);
            
            AddButtonListener(View.LayerUp, LayerUpHandler);
            AddButtonListener(View.LayerDown, LayerDownHandler);
            AddTmpInputListener(View.PositionInput, PositionInputHandler);
            AddButtonListener(View.RemoveButton, RemoveButtonHandler);
            
            SetEnabled(false);

        }
        
        public override void OnRemove()
        {
            EventObjectSelectedSignal.RemoveListener(EventObjectSelectedHandler);
            EditModeChangedSignal.RemoveListener(EditModeChangedHandler);

            base.OnRemove();
        }
        
        
        private void EditModeChangedHandler(EditorMode type)
        {
            _editorMode = type;
        }
        
        private void LayerUpHandler()
        {
            var grass = EnemyEventsModel.GetGrass(_grassIndex);
            grass.LayerIndex++;
            EnviromentMovedSignal.Dispatch();
            SetData(grass);
            EnemyEventsModel.SaveChange();

        }
        
        private void LayerDownHandler()
        {
            var grass = EnemyEventsModel.GetGrass(_grassIndex);
            grass.LayerIndex--;
            EnviromentMovedSignal.Dispatch();
            SetData(grass);
            EnemyEventsModel.SaveChange();

        }
        private void PositionInputHandler(string pos)
        {
            var grass = EnemyEventsModel.GetGrass(_grassIndex);
            float position;
            if (float.TryParse(pos, out position))
            {
                grass.StartPos = position;
                EnviromentMovedSignal.Dispatch();
            }
            SetData(grass);
            EnemyEventsModel.SaveChange();
      
        }
        private void SetData(EditorGrassData objectData)
        {
            View.Index.text = objectData.LayerIndex.ToString();
            View.PositionInput.text = objectData.StartPos.ToString();

        }
        
        private void RemoveButtonHandler()
        {
            DeleteSelected();
        }
        
        private void EventObjectSelectedHandler(EnemyType type, int index)
        {
            if (type != EnemyType.Grass || _editorMode != EditorMode.Edit)
            {
                SetEnabled(false);
                return;
            }
            
            _grassIndex = index;
            var grassData = EnemyEventsModel.GetGrass(_grassIndex);
            if (grassData != null)
            {
                SetData(grassData);
                SetEnabled(true);

            }
            else
            {
                SetEnabled(false);

            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (_grassIndex != -1)
                {
                    DeleteSelected();
                }
            }
        }
        
        private void DeleteSelected()
        {
            EnemyEventsModel.RemoveGrass(_grassIndex);
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }
        public void SetEnabled(bool isEnabled)
        {
            //disable
            View.gameObject.SetActive(isEnabled);
        } 
    }
}