using Level;
using Plugins.Framewerk;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace LevelEditor.Interactions
{
    public class SelectedObjectMediator : Mediator
    {
        [Inject] public SelectedObjectView View { get; set; }

        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }

        [Inject] public ViewConfig ViewConfig { get; set; }

        private bool _posHandlePressed = false;
        private bool _heiHandlePressed = false;
        private bool _depthHandlePressed = false;


        private float _radius;
        private Vector3 screenPoint;
        private Vector3 offset;

        private float _posOnLayer;
        private float _height;
        private float _depth;

        private IEditorData _lastSelected;


        public override void OnRegister()
        {
            base.OnRegister();
            LevelEditorControler.UpdateSelectionHandle.AddListener(UpdateSelection);
            View.Init();
            View.TogglePosHandle(false);
            View.ToggleHeiHandle(false);
            View.ToggleDepthHandle(false);
        }

        private void UpdateHandles(EnemyType type)
        {
            if (type == EnemyType.Air)
            {
                View.SetHeightHandleEnabled(true);
            }
            else if (type == EnemyType.Grass)
            {
                View.SetHeightHandleEnabled(false);
            }
            else if (type == EnemyType.Fish)
            {
                View.SetHeightHandleEnabled(false);
            }
            else if (type == EnemyType.Target)
            {
                View.SetDepthHandleEnabled(true);
                View.SetHeightHandleEnabled(false);
            }
        }

        private void UpdateSelection()
        {
            var _selectedEvents = LevelEditorControler.SelectedEvents;


            SetEnabled(_selectedEvents.Count > 0);

            if (_selectedEvents.Count == 0)
                return;

            _lastSelected = _selectedEvents[_selectedEvents.Count - 1]; // last
            if (_lastSelected == null)
                return;

            if (_lastSelected.EnemyType == EnemyType.Air || _lastSelected.EnemyType == EnemyType.Obstacle)
            {
                SetEnabled(false);
                return;
            }

            UpdateHandles(_lastSelected.EnemyType);

            int pos = EnemyEventsModel.VisibleLayerIndexes.IndexOf(_lastSelected.LayerIndex) + 1;
            float ratio = LevelEditorControler.ArenaRadius / LevelEditorControler.Bmp;
            _radius = pos * ratio;

            View.SetData(_lastSelected, _radius);
            _posOnLayer = _lastSelected.StartPos;
            _height = _lastSelected.StartHeight;
        }

        private void SetEnabled(bool isEnabled)
        {
            View.gameObject.SetActive(isEnabled);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = ViewConfig.Camera3d.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, LayerMasks.Dome))
                {
                    //retarded comparison
                    if (hit.transform.gameObject == View.PosHandler)
                    {
                        _posHandlePressed = true;
                        View.TogglePosHandle(true);
                        screenPoint = ViewConfig.Camera3d.WorldToScreenPoint(gameObject.transform.position);
                        offset = gameObject.transform.position -
                                 ViewConfig.Camera3d.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                     Input.mousePosition.y,
                                     screenPoint.z));
                    }
                    else if (hit.transform.gameObject == View.HeightHandler)
                    {
                        _heiHandlePressed = true;
                        View.ToggleHeiHandle(true);
                        screenPoint = ViewConfig.Camera3d.WorldToScreenPoint(gameObject.transform.position);
                        offset = gameObject.transform.position -
                                 ViewConfig.Camera3d.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                     Input.mousePosition.y,
                                     screenPoint.z));
                    }
                    else if (hit.transform.gameObject == View.DepthHandler)
                    {
                        _depthHandlePressed = true;
                        View.ToggleDepthHandle(true);
                        screenPoint = ViewConfig.Camera3d.WorldToScreenPoint(gameObject.transform.position);
                        offset = gameObject.transform.position -
                                 ViewConfig.Camera3d.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                     Input.mousePosition.y,
                                     screenPoint.z));
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_posHandlePressed || _heiHandlePressed || _depthHandlePressed)
                {
                    //commit data
                    CommitChanges();
                }

                _posHandlePressed = false;
                _heiHandlePressed = false;
                _depthHandlePressed = false;

                View.TogglePosHandle(false);
                View.ToggleHeiHandle(false);
                View.ToggleDepthHandle(false);
            }

            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = ViewConfig.Camera3d.ScreenToWorldPoint(cursorPoint) + offset;

            if (_posHandlePressed)
            {
                float angle = Mathf.Atan2(cursorPosition.x, cursorPosition.z) * Mathf.Rad2Deg;
                _posOnLayer = angle / 360f;
                View.SetPosition(_lastSelected, _posOnLayer, _radius);
            }
            else if (_heiHandlePressed)
            {
                _height = cursorPosition.y;
                View.SetHeight(_height);
            }
            else if (_depthHandlePressed)
            {
                //_depth = cursorPosition.z;
                //View.SetPosition(_lastSelected,_posOnLayer , _radius);
            }
        }

        private void CommitChanges()
        {
            var _selectedEvents = LevelEditorControler.SelectedEvents;

            float posOffset = _posOnLayer - _lastSelected.StartPos;
            float heightOffset = _height - _lastSelected.StartHeight;

            foreach (var selectedEvent in _selectedEvents)
            {
                if (selectedEvent == null)
                    return;
                // selected event null ??
                var enemyData = EnemyEventsModel.GetEventData(selectedEvent.EnemyType, selectedEvent.Index);
                enemyData.StartPos = EditorHelpers.Round(enemyData.StartPos + posOffset, 2);
                enemyData.StartHeight = EditorHelpers.Round(enemyData.StartHeight + heightOffset, 2);
            }

            EnviromentMovedSignal.Dispatch();
        }
    }
}