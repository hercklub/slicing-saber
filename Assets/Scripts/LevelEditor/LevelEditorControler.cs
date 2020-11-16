
using System;
using System.Collections.Generic;
using Level;
using LevelEditor.EventsList;
using strange.extensions.signal.impl;
using UnityEngine;

namespace LevelEditor
{
    public interface ILevelEditorControler
    {
        void Init();
        void ChangeCurrentIndex(int delta);
        float Bmp { get; set; }
        int EventsPerBeat { get; set; }
        float ArenaRadius { get; }
        float VisibleRadius { get; }
        
        Signal IndexChangedSignal { get; }
        List<IEditorData> SelectedEvents { get; set; }
        Signal UpdateSelectionHandle { get; }
        int CurrentLayerIndex { get; set; }
        
        EditorMode EditorMode{ get; set; }
        EventEditType CurrentEventType { get; set; }

        bool AllLinesMode { get; set; }
        bool VisibleObstacles { get; set; }
        Signal LineModeChanched { get; }
        Signal<bool> ObstaclesVisible { get; }
        void UpdateSelected();
        
        void Undo();
        void Redo();
        void DuplicateSelected();
    }

    public class LevelEditorControler : ILevelEditorControler
    {
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }
        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }
        
        public Signal UpdateSelectionHandle { get; } = new Signal();
        public Signal LineModeChanched { get; } = new Signal();
        
        public Signal<bool> ObstaclesVisible { get; } = new Signal<bool>();

        public float Bmp
        {
            get => _bmp;
            set => _bmp = value;
        }

        public int EventsPerBeat
        {
            get => _epb;
            set => _epb = value;
        }
        
        public float ArenaRadius => _arenaRadius;
        public float VisibleRadius => _visibleRadius;

        public bool AllLinesMode
        {
            get => _allLinesMode;
            set { _allLinesMode = value; LineModeChanched.Dispatch(); }
        }

        public bool VisibleObstacles
        {
            get => _visibleObstacle;
            set { _visibleObstacle = value; ObstaclesVisible.Dispatch(_visibleObstacle); }
        }

        public List<IEditorData> SelectedEvents { get; set; } = new List<IEditorData>();
        public Signal IndexChangedSignal { get; } = new Signal();

        private float _bmp;
        private int _epb = 4;
        private float _arenaRadius;
        private float _visibleRadius;
        private EventEditType _currentEventType = EventEditType.None;
        private bool _allLinesMode = false;
        private bool _visibleObstacle = false;

        private int _currentLayerIndex;
        public int CurrentLayerIndex
        {
            get => _currentLayerIndex;
            set => _currentLayerIndex = value;
        }
        public EditorMode EditorMode { get; set; }
        public EventEditType CurrentEventType
        {
            get => _currentEventType;
            set => _currentEventType = value;
        }

        public void Init()
        {
            _bmp = 60;
            _arenaRadius = 20f;
            _visibleRadius = 40f;
            
            EnviromentMovedSignal.AddListener(EnviromentMovedHandler);
        }

        private void EnviromentMovedHandler()
        {
            //Debug.Log("ENVIROMENT CHANGED !");
        }


        public void ChangeCurrentIndex(int delta)
        {
            if (delta == 0)
                return;
            
            var newIndex = _currentLayerIndex + delta;
            if (newIndex < 0)
            {
                _currentLayerIndex = 0;
            }
            else
            {
                _currentLayerIndex = newIndex;
            }
            
            IndexChangedSignal.Dispatch();
        }
        

        public void UpdateSelected()
        {
            if (SelectedEvents.Count > 0)
            {
                //probably dont need to dispach for each object, too lazy to investigate
                foreach (var editorEvent in SelectedEvents)
                {
                    EventObjectSelectedSignal.Dispatch(editorEvent.EnemyType, editorEvent.Index);
                }
            }
            else
            {
                EventObjectSelectedSignal.Dispatch(EnemyType.None, -1);
            }
            
            UpdateSelectionHandle.Dispatch();
        }

        public void DuplicateSelected()
        {
            foreach (var selectedEvent in SelectedEvents)
            {
                switch (selectedEvent.EnemyType)
                {
                    case EnemyType.None:
                        break;
                    case EnemyType.Fish:
                        EditorFishData fishData = selectedEvent as EditorFishData;
                        EnemyEventsModel.AddFishEvent(fishData.LayerIndex, fishData.Time, fishData.StartPos, fishData.FishType);
                        break;
                    case EnemyType.Obstacle:
                        EditorObstacleData obstacleData = selectedEvent as EditorObstacleData;
                        EnemyEventsModel.AddObstacleEvent(obstacleData);
                        break;
                    case EnemyType.Air:
                        EditorEnemyData enemyData = selectedEvent as EditorEnemyData;
                        EnemyEventsModel.AddEnemyEvent(enemyData);
                        break;
                    case EnemyType.Grass:
                        EditorGrassData grassData = selectedEvent as EditorGrassData;
                        EnemyEventsModel.AddGrassEvent(grassData.LayerIndex, grassData.Time, grassData.StartPos);
                        break;
                    case EnemyType.Target:
                        EditorTargetData targetData = selectedEvent as EditorTargetData;
                        EnemyEventsModel.AddTargetEvent(targetData.LayerIndex, targetData.Time, targetData.StartPos, targetData.Depth, targetData.TargetType);
                        break;
   
                }
            }
            
            EnviromentMovedSignal.Dispatch();
            EnemyEventsModel.SaveChange();
        }

        public void Undo()
        {
            EnemyEventsModel.Undo();
            IndexChangedSignal.Dispatch();
            

        }

        public void Redo()
        {
            EnemyEventsModel.Redo();
            IndexChangedSignal.Dispatch();

        }
    }
}