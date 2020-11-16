using System;
using Framewerk.UI;
using Level;
using Obstacle;
using TMPro;
using UnityEngine;

namespace LevelEditor
{
    public class ObstacleDataMediator : ExtendedMediator
    {
        [Inject] public ObstacleDataView View { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }

        private int _currentDirectionIndex;

        private int _obstacleIndex;
        private EditorMode _editorMode;
        
        private const int ROWS = 3;
        private const int COLS = 3;

        public override void OnRegister()
        {
            base.OnRegister();

            EventObjectSelectedSignal.AddListener(EventObjectSelectedHandler);
            EditModeChangedSignal.AddListener(EditModeChangedHandler);
            //react to event
            // start
            AddButtonListener(View.LayerUp, LayerUpHandler);
            AddButtonListener(View.LayerDown, LayerDownHandler);

            AddTmpInputListener(View.StartPositionInput, StartPositionInputHandler);
            AddTmpInputListener(View.StartHeightInput, StartHeightInputHandler);

            AddTmpInputListener(View.ScaleX, ScaleXHandler);
            AddTmpInputListener(View.ScaleY, ScaleYHandler);

            //mid
            AddTmpInputListener(View.MidPositionInput, MidPositionInputHandler);
            AddTmpInputListener(View.MidHeightInput, MidHeightInputHandler);

            //end
            AddTmpDropdownListener(View.EndPosDropwDown, EndDirectionHandler);
            AddTmpInputListener(View.EndRotationDropDown, EndRotationTypeHandler);
            
            AddTmpDropdownListener(View.PivotDropDown, PivotHandler);
            AddButtonListener(View.IsPortal, Portalhandler);

            AddButtonListener(View.RemoveButton, RemoveButtonHandler);


            var enemyDirection = Enum.GetValues(typeof(ObstacleEndDir));
            View.EndPosDropwDown.options.Clear();
            foreach (var val in enemyDirection)
            {
                View.EndPosDropwDown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }
            
            var pivot = Enum.GetValues(typeof(ObstacleEndDir));
            View.PivotDropDown.options.Clear();
            foreach (var val in pivot)
            {
                View.PivotDropDown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }


            SetEnabled(false);
        }
        

        public override void OnRemove()
        {
            EventObjectSelectedSignal.RemoveListener(EventObjectSelectedHandler);
            EditModeChangedSignal.RemoveListener(EditModeChangedHandler);

            base.OnRemove();
        }


        private void LayerUpHandler()
        {
            var enemy = EnemyEventsModel.GetObstacle(_obstacleIndex);
            enemy.LayerIndex++;
            EnviromentMovedSignal.Dispatch();
            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void LayerDownHandler()
        {
            var enemy = EnemyEventsModel.GetObstacle(_obstacleIndex);
            enemy.LayerIndex--;
            EnviromentMovedSignal.Dispatch();
            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void StartPositionInputHandler(string pos)
        {
            var obstacle = EnemyEventsModel.GetObstacle(_obstacleIndex);
            float position;
            if (float.TryParse(pos, out position))
            {
                obstacle.StartPos = position;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(obstacle);
            EnemyEventsModel.SaveChange();
        }

        private void StartHeightInputHandler(string height)
        {
            var obstacle = EnemyEventsModel.GetObstacle(_obstacleIndex);
            float newHeight;
            if (float.TryParse(height, out newHeight))
            {
                obstacle.StartHeight = newHeight;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(obstacle);
            EnemyEventsModel.SaveChange();
        }

        private void ScaleXHandler(string scaleX)
        {
            float newScaleX;
            if (float.TryParse(scaleX, out newScaleX))
            {
                var _selectedEvents = LevelEditorControler.SelectedEvents;
                for (int i = 0; i < _selectedEvents.Count; i++)
                {
                    var obstacle = _selectedEvents[i] as EditorObstacleData;
                    if (obstacle != null)
                    {
                        obstacle.ScaleX = newScaleX;
                    } 
                }
            }

            var lastSelected = EnemyEventsModel.GetObstacle(_obstacleIndex);
            SetData(lastSelected);
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
            
            
        }

        private void ScaleYHandler(string scaleY)
        {
            float newScaleY;
            if (float.TryParse(scaleY, out newScaleY))
            {
                var _selectedEvents = LevelEditorControler.SelectedEvents;
                for (int i = 0; i < _selectedEvents.Count; i++)
                {
                    var obstacle = _selectedEvents[i] as EditorObstacleData;
                    if (obstacle != null)
                    {
                        obstacle.ScaleY = newScaleY;
                    } 
                }
            }
            
            var lastSelected = EnemyEventsModel.GetObstacle(_obstacleIndex);
            SetData(lastSelected);
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void MidPositionInputHandler(string pos)
        {
            var enemy = EnemyEventsModel.GetObstacle(_obstacleIndex);
            float position;
            if (float.TryParse(pos, out position))
            {
                enemy.MidPos = position;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void MidHeightInputHandler(string height)
        {
            var enemy = EnemyEventsModel.GetObstacle(_obstacleIndex);
            float newHeight;
            if (float.TryParse(height, out newHeight))
            {
                enemy.MidHeight = newHeight;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }


        private void EndDirectionHandler(int enemyDir)
        {
            
           // var _selectedEvents = LevelEditorControler.SelectedEvents;
            // for (int i = 0; i < _selectedEvents.Count; i++)
            // {
            //     var obstacle = _selectedEvents[i] as EditorObstacleData;
            //     if (obstacle != null)
            //     {
            //         obstacle.EndDirection = (ObstacleEndDir) enemyDir;
            //     }
            // }
            
            
            var enemy = EnemyEventsModel.GetObstacle(_obstacleIndex);
            if (enemyDir != (int) enemy.EndDirection)
            {
                enemy.EndDirection = (ObstacleEndDir) enemyDir;
                EnviromentMovedSignal.Dispatch();
                SetData(enemy);
                EnemyEventsModel.SaveChange();
            }
        }

        private void EndRotationTypeHandler(string enemyRot)
        {
            var enemy = EnemyEventsModel.GetObstacle(_obstacleIndex);
            float newEndRotation;
            if (float.TryParse(enemyRot, out newEndRotation))
            {
                enemy.EndRotation = newEndRotation;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void PivotHandler(int pivot)
        {
            // var _selectedEvents = LevelEditorControler.SelectedEvents;
            // for (int i = 0; i < _selectedEvents.Count; i++)
            // {
            //     var obstacle = _selectedEvents[i] as EditorObstacleData;
            //     if (obstacle != null)
            //     {
            //         obstacle.Pivot = (ObstacleEndDir) pivot;
            //     }
            // }

            var lastSelected = EnemyEventsModel.GetObstacle(_obstacleIndex);
            if (pivot != (int) lastSelected.Pivot)
            {
                lastSelected.Pivot = (ObstacleEndDir) pivot;
                EnviromentMovedSignal.Dispatch();
                SetData(lastSelected);
                EnemyEventsModel.SaveChange();
            }

        }

        private void Portalhandler()
        {
            var obstacle = EnemyEventsModel.GetObstacle(_obstacleIndex);
            obstacle.IsPortal = !obstacle.IsPortal;
            EnviromentMovedSignal.Dispatch();
            SetData(obstacle);
            EnemyEventsModel.SaveChange();
        }
        
        private void RemoveButtonHandler()
        {
            DeleteSelected();
        }

        public void SetEnabled(bool isEnabled)
        {
            //disable
            View.gameObject.SetActive(isEnabled);
        }

        private void DeleteSelected()
        {
            EnemyEventsModel.RemoveObstacle(_obstacleIndex);
            EnviromentMovedSignal.Dispatch();
        }

        private void EditModeChangedHandler(EditorMode type)
        {
            _editorMode = type;
        }

        private void EventObjectSelectedHandler(EnemyType type, int index)
        {
            if (type != EnemyType.Obstacle || _editorMode != EditorMode.Edit)
            {
                SetEnabled(false);
                return;
            }

             _obstacleIndex = index;
            var obstacleData = EnemyEventsModel.GetObstacle(_obstacleIndex);
            
            if (obstacleData != null)
            {
                SetData(obstacleData);
                SetEnabled(true);
            }
            else
            {
                SetEnabled(false);
            }
        }

        private void SetData(EditorObstacleData objectData)
        {
            View.Index.text = objectData.LayerIndex.ToString();

            View.StartPositionInput.text = objectData.StartPos.ToString();
            View.StartHeightInput.text = objectData.StartHeight.ToString();

            View.ScaleX.text = objectData.ScaleX.ToString();
            View.ScaleY.text = objectData.ScaleY.ToString();

            View.MidPositionInput.text = objectData.MidPos.ToString();
            View.MidHeightInput.text = objectData.MidHeight.ToString();

            _currentDirectionIndex = (int) objectData.EndDirection;
            View.EndPosDropwDown.value = _currentDirectionIndex;
            View.EndRotationDropDown.text = objectData.EndRotation.ToString();

            View.PivotDropDown.value = (int) objectData.Pivot;
            View.IsPortalImage.gameObject.SetActive(objectData.IsPortal);
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (_obstacleIndex != -1)
                {
                    DeleteSelected();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentDirectionIndex += ROWS;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);
                SetDirection((ObstacleEndDir) _currentDirectionIndex);

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentDirectionIndex -= ROWS;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);
                if (_currentDirectionIndex < 0)
                {
                    _currentDirectionIndex = (ROWS * COLS) + _currentDirectionIndex;
                }

                SetDirection((ObstacleEndDir) _currentDirectionIndex);

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currentDirectionIndex--;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);
                if (_currentDirectionIndex < 0)
                {
                    _currentDirectionIndex = (ROWS * COLS) + _currentDirectionIndex;
                }

                SetDirection((ObstacleEndDir) _currentDirectionIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currentDirectionIndex++;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);

                SetDirection((ObstacleEndDir) _currentDirectionIndex);
            }
        }
        private void SetDirection(ObstacleEndDir direction)
        {
            if (_obstacleIndex == -1)
                return;

            var obstacle = EnemyEventsModel.GetObstacle(_obstacleIndex);
            obstacle.EndDirection = direction;
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }
    }
}