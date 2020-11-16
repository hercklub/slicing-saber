using Level;
using PathCreation;
using Plugins.Framewerk;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace LevelEditor.Interactions
{
    public class PathSelectedObjectMediator : Mediator
    {
        [Inject] public PathSelectedObjectView View { get; set; }

        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public ViewConfig ViewConfig { get; set; }

        private IPathEditorData _lastSelected;

        private float _radius;
        private Vector3 screenPoint;
        private Vector3 offset;


        public override void OnRegister()
        {
            base.OnRegister();
            LevelEditorControler.UpdateSelectionHandle.AddListener(UpdateSelection);

            View.StartHandles.PosMovedSignal.AddListener(StartPosHandleMovedhadnler);
            View.StartHandles.HeightMovedSignal.AddListener(StartHeightHandleMovedhadnler);
            
            
            View.MidHandles.PosMovedSignal.AddListener(MidPosHandleMovedhadnler);
            View.MidHandles.HeightMovedSignal.AddListener(MidHeightHandleMovedhadnler);

            View.StartHandles.RaycastCamera = ViewConfig.Camera3d;
            View.StartHandles.SphericalHeight = true;
            View.MidHandles.RaycastCamera = ViewConfig.Camera3d;
            View.MidHandles.SphericalHeight = false;
        }


        public override void OnRemove()
        {
            LevelEditorControler.UpdateSelectionHandle.RemoveListener(UpdateSelection);
            
            View.StartHandles.PosMovedSignal.RemoveListener(StartPosHandleMovedhadnler);
            View.StartHandles.HeightMovedSignal.RemoveListener(StartHeightHandleMovedhadnler);
            
            View.MidHandles.PosMovedSignal.RemoveListener(MidPosHandleMovedhadnler);
            View.MidHandles.HeightMovedSignal.RemoveListener(MidHeightHandleMovedhadnler);
            
            //View.MidHandles.HandlesMovedSignal.RemoveListener(MidHandleMovedhadnler);
            base.OnRemove();
        }

        private void StartPosHandleMovedhadnler(float pos, bool commit)
        {
            if (_lastSelected == null)
                return;
            CommitStartChanges(pos, _lastSelected.StartHeight, commit);
        }
        private void StartHeightHandleMovedhadnler(float height, bool commit)
        {
            if (_lastSelected == null)
                return;
            CommitStartChanges(_lastSelected.StartPos, height, commit);
        }

        private void MidPosHandleMovedhadnler(float pos, bool commit)
        {
            if (_lastSelected == null)
                return;
            CommitMidChanges(pos, _lastSelected.MidHeight, commit);
        }
        private void MidHeightHandleMovedhadnler(float height, bool commit)
        {
            if (_lastSelected == null)
                return;
            CommitMidChanges(_lastSelected.MidPos, height, commit);
        }

        private void UpdateSelection()
        {
            var _selectedEvents = LevelEditorControler.SelectedEvents;


            SetEnabled(_selectedEvents.Count > 0);

            if (_selectedEvents.Count == 0)
                return;

            _lastSelected = _selectedEvents[_selectedEvents.Count - 1] as IPathEditorData;
            
            if (_lastSelected == null)
            {
                SetEnabled(false);
                return;
            }

            if (_lastSelected.EnemyType != EnemyType.Air && _lastSelected.EnemyType != EnemyType.Obstacle  )
            {
                SetEnabled(false);
                return;
            }

            SetStartPosition(_lastSelected.StartPos, _lastSelected.StartHeight);
            SetMidPosition(_lastSelected.MidPos, _lastSelected.MidHeight);

        }

        private void SetEnabled(bool isEnabled)
        {
            View.gameObject.SetActive(isEnabled);
        }

        private void CommitStartChanges(float pos, float height, bool commit)
        {
            var _selectedEvents = LevelEditorControler.SelectedEvents;

            float posOffset = pos - _lastSelected.StartPos;
            float heightOffset = height - _lastSelected.StartHeight;
            foreach (var selectedEvent in _selectedEvents)
            {
                if (selectedEvent == null)
                    return;
                // selected event null ??
                int roundTo = 3;
                var newStartPos = EditorHelpers.Round(selectedEvent.StartPos + posOffset, roundTo);
                var newStartHeight = EditorHelpers.Round(selectedEvent.StartHeight + heightOffset, roundTo + 1);

                SetStartPosition(newStartPos, newStartHeight );
                if (commit)
                {
                    selectedEvent.StartPos = newStartPos;
                    selectedEvent.StartHeight = newStartHeight;
                }
            }

            if (commit)
            {
                EnviromentMovedSignal.Dispatch();
            }
        }

        private void CommitMidChanges(float pos, float height, bool commit)
        {
            var _selectedEvents = LevelEditorControler.SelectedEvents;

            float posOffset = pos - _lastSelected.MidPos;
            float heightOffset = height - _lastSelected.MidHeight;
            foreach (var selectedEvent in _selectedEvents)
            {
                var pathEditData = selectedEvent as IPathEditorData;
                if (selectedEvent == null)
                    return;
                // selected event null ??
                int roundTo = 3;

                var newMidPos = EditorHelpers.Round(pathEditData.MidPos + posOffset, roundTo);
                var newMidHeight = EditorHelpers.Round(pathEditData.MidHeight + heightOffset, roundTo);
                
                SetMidPosition(newMidPos, newMidHeight );
                if (commit)
                {
                    pathEditData.MidPos = newMidPos;
                    pathEditData.MidHeight = newMidHeight;
                }
            }

            if (commit)
            {
                EnviromentMovedSignal.Dispatch();
            }
        }

        private void SetMidPosition(float newPos, float newHeight)
        {
            Vector3 pos = EditorHelpers.GetPointOnCircle(newPos * 360f,  10f);
            pos.y = newHeight;
            View.MidHandles.transform.position = pos;
            View.MidHandles.transform.rotation = Quaternion.LookRotation(new Vector3(pos.x, 0f, pos.z), Vector3.up);
        }

        private void SetStartPosition(float newPos, float newHeight)
        {
            Vector3 pos = EditorHelpers.GetPointOnCircle(newPos * 360f, newHeight * 360f, 20f);
            View.StartHandles.transform.position = pos;
            View.StartHandles.transform.rotation = Quaternion.LookRotation(new Vector3(pos.x, 0f, pos.z), Vector3.up);
        }
    }
}