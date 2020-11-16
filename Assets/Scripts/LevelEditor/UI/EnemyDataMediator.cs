using System;
using Framewerk.UI;
using Level;
using LevelEditor.EventsList;
using LevelEditor.Interactions;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class EnemyDataMediator : ExtendedMediator
    {
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public EnemyDataView View { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }

        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }

        [Inject] public EditModeChangedSignal EditModeChangedSignal { get; set; }


        private int _enemyIndex;
        private EditorMode _editorMode;

        private int _currentDirectionIndex;
        private const int ROWS = 4;
        private const int COLS = 4;

        public override void OnRegister()
        {
            base.OnRegister();
            //react to event
            EventObjectSelectedSignal.AddListener(EventObjectSelectedHandler);
            EditModeChangedSignal.AddListener(EditModeChangedHandler);

            // start
            AddButtonListener(View.LayerUp, LayerUpHandler);
            AddButtonListener(View.LayerDown, LayerDownHandler);

            AddTmpInputListener(View.StartPositionInput, StartPositionInputHandler);
            AddTmpInputListener(View.StartHeightInput, StartHeightInputHandler);
            AddTmpDropdownListener(View.StartTypeDropdown, TypeDropdownHandler);

            //mid
            AddTmpInputListener(View.MidPositionInput, MidPositionInputHandler);
            AddTmpInputListener(View.MidHeightInput, MidHeightInputHandler);
            AddTmpDropdownListener(View.MidRotationDropDown, MidRotationTypeHandler);

            //end
            AddTmpDropdownListener(View.EndPosDropwDown, EndDirectionHandler);
            AddTmpDropdownListener(View.EndRotationDropDown, EndRotationTypeHandler);

            AddButtonListener(View.LeftButton, LeftDirButton);
            AddButtonListener(View.RightButton, RightDirButton);
            AddButtonListener(View.UpButton, UpDirButton);
            AddButtonListener(View.DownButton, DownDirButton);


            AddButtonListener(View.RemoveButton, RemoveButtonHandler);


            var airtypes = Enum.GetValues(typeof(EnemyAirType));
            View.StartTypeDropdown.options.Clear();
            foreach (var val in airtypes)
            {
                View.StartTypeDropdown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }

            var enemyDirection = Enum.GetValues(typeof(EnemyDirection));
            View.EndPosDropwDown.options.Clear();
            foreach (var val in enemyDirection)
            {
                View.EndPosDropwDown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }

            var midrotations = Enum.GetValues(typeof(EnemyRotation));
            View.MidRotationDropDown.options.Clear();
            foreach (var val in midrotations)
            {
                View.MidRotationDropDown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
            }

            var endRotations = Enum.GetValues(typeof(EnemyRotation));
            View.EndRotationDropDown.options.Clear();
            foreach (var val in endRotations)
            {
                View.EndRotationDropDown.options.Add(new TMP_Dropdown.OptionData(val.ToString()));
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
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);
            enemy.LayerIndex++;
            EnviromentMovedSignal.Dispatch();
            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void LayerDownHandler()
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);
            enemy.LayerIndex--;
            EnviromentMovedSignal.Dispatch();
            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void StartPositionInputHandler(string pos)
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);
            float position;
            if (float.TryParse(pos, out position))
            {
                enemy.StartPos = position;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void StartHeightInputHandler(string height)
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);
            float newHeight;
            if (float.TryParse(height, out newHeight))
            {
                enemy.StartHeight = newHeight;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void TypeDropdownHandler(int type)
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);

            if (type != (int) enemy.Type)
            {
                enemy.Type = (EnemyAirType) type;
                EnviromentMovedSignal.Dispatch();
                SetData(enemy);
                EnemyEventsModel.SaveChange();
            }
        }


        private void MidPositionInputHandler(string pos)
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);
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
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);
            float newHeight;
            if (float.TryParse(height, out newHeight))
            {
                enemy.MidHeight = newHeight;
                EnviromentMovedSignal.Dispatch();
            }

            SetData(enemy);
            EnemyEventsModel.SaveChange();
        }

        private void MidRotationTypeHandler(int midRot)
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);

            if (midRot != (int) enemy.MidRotation)
            {
                enemy.MidRotation = (EnemyRotation) midRot;
                EnviromentMovedSignal.Dispatch();
                SetData(enemy);
                EnemyEventsModel.SaveChange();
            }
        }


        private void EndDirectionHandler(int enemyDir)
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);

            if (enemyDir != (int) enemy.EndDirection)
            {
                enemy.EndDirection = (EnemyDirection) enemyDir;
                EnviromentMovedSignal.Dispatch();
                SetData(enemy);
                EnemyEventsModel.SaveChange();
            }
        }

        private void EndRotationTypeHandler(int enemyRot)
        {
            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);

            if (enemyRot != (int) enemy.EndRotation)
            {
                enemy.EndRotation = (EnemyRotation) enemyRot;
                EnviromentMovedSignal.Dispatch();
                SetData(enemy);
                EnemyEventsModel.SaveChange();
            }
        }

        private void RemoveButtonHandler()
        {
            DeleteSelected();
        }

        private void EditModeChangedHandler(EditorMode type)
        {
            _editorMode = type;
        }

        private void EventObjectSelectedHandler(EnemyType type, int index)
        {
            if (type != EnemyType.Air || _editorMode != EditorMode.Edit)
            {
                SetEnabled(false);
                return;
            }

            _enemyIndex = index;
            var enemyData = EnemyEventsModel.GetEnemy(_enemyIndex);

            if (enemyData != null)
            {
                SetData(enemyData);
                SetEnabled(true);
            }
            else
            {
                SetEnabled(false);
            }
        }

        private void RightDirButton()
        {
            ToggleCutDir(eCutDir.Right);
        }

        private void LeftDirButton()
        {
            ToggleCutDir(eCutDir.Left);
        }

        private void DownDirButton()
        {
            ToggleCutDir(eCutDir.Down);
        }

        private void UpDirButton()
        {
            ToggleCutDir(eCutDir.Up);
        }

        private void ToggleCutDir(eCutDir cutDir)
        {
            var _selectedEvents = LevelEditorControler.SelectedEvents;
            for (int i = 0; i < _selectedEvents.Count; i++)
            {
                var enemy = _selectedEvents[i] as EditorEnemyData;
                if (enemy != null)
                {
                    enemy.CutDir = enemy.CutDir.ToogleFlag(cutDir);
                }
            }
            
            var lastSelected = EnemyEventsModel.GetEnemy(_enemyIndex);
            SetData(lastSelected);
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void SetCutDirButtons(eCutDir cutDir)
        {
            View.LeftCheckmark.SetActive(cutDir.HasAxisFlag(eCutDir.Left));
            View.RightChekmark.SetActive(cutDir.HasAxisFlag(eCutDir.Right));
            View.DownCheckmark.SetActive(cutDir.HasAxisFlag(eCutDir.Down));
            View.UpCheckmark.SetActive(cutDir.HasAxisFlag(eCutDir.Up));
        }

        private void SetData(EditorEnemyData objectData)
        {
            View.Index.text = objectData.LayerIndex.ToString();
            View.StartPositionInput.text = objectData.StartPos.ToString();
            View.StartHeightInput.text = objectData.StartHeight.ToString();
            View.StartTypeDropdown.value = (int) objectData.Type;


            View.MidPositionInput.text = objectData.MidPos.ToString();
            View.MidHeightInput.text = objectData.MidHeight.ToString();
            View.MidRotationDropDown.value = (int) objectData.MidRotation;

            _currentDirectionIndex = (int) objectData.EndDirection;
            View.EndPosDropwDown.value = _currentDirectionIndex;
            View.EndRotationDropDown.value = (int) objectData.EndRotation;

            SetCutDirButtons(objectData.CutDir);
        }

        public void SetEnabled(bool isEnabled)
        {
            //disable
            View.gameObject.SetActive(isEnabled);
        }

        private void DeleteSelected()
        {
            EnemyEventsModel.RemoveEnemy(_enemyIndex);
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void SetDirection(EnemyDirection direction)
        {
            if (_enemyIndex == -1)
                return;

            var enemy = EnemyEventsModel.GetEnemy(_enemyIndex);
            enemy.EndDirection = direction;
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (_enemyIndex != -1)
                {
                    DeleteSelected();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentDirectionIndex += ROWS;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);
                SetDirection((EnemyDirection) _currentDirectionIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentDirectionIndex -= ROWS;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);
                if (_currentDirectionIndex < 0)
                {
                    _currentDirectionIndex = (ROWS * COLS) + _currentDirectionIndex;
                }

                SetDirection((EnemyDirection) _currentDirectionIndex);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currentDirectionIndex--;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);
                if (_currentDirectionIndex < 0)
                {
                    _currentDirectionIndex = (ROWS * COLS) + _currentDirectionIndex;
                }

                SetDirection((EnemyDirection) _currentDirectionIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currentDirectionIndex++;
                _currentDirectionIndex = _currentDirectionIndex % (ROWS * COLS);

                SetDirection((EnemyDirection) _currentDirectionIndex);
            }


            // if (Input.GetKeyDown(KeyCode.Keypad1))
            // {
            //     SetDirection(EnemyDirection.DownLeft);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad2))
            // {
            //     SetDirection(EnemyDirection.Down);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad3))
            // {
            //     SetDirection(EnemyDirection.DownRight);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad4))
            // {
            //     SetDirection(EnemyDirection.Left);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad5))
            // {
            //     SetDirection(EnemyDirection.None);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad6))
            // {
            //     SetDirection(EnemyDirection.Right);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad7))
            // {
            //     SetDirection(EnemyDirection.TopLeft);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad8))
            // {
            //     SetDirection(EnemyDirection.Up);
            // }
            // else if (Input.GetKeyDown(KeyCode.Keypad9))
            // {
            //     SetDirection(EnemyDirection.TopRight);
            // }
        }
    }
}