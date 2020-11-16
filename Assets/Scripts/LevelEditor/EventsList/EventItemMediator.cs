using System.Collections.Generic;
using System.Linq;
using Framewerk.UI.List;
using strange.extensions.pool.api;
using UnityEngine;

namespace LevelEditor.EventsList
{
    public class EventItemMediator : ListItemMediator<EventItemView, EventLayerDataprovider>
    {
        [Inject] public IPool<EventEnemyObjectView> EventEnemyObjectsPool { get; set; }
        [Inject] public IPool<EventGrassObjectView> EventGrassObjectsPool { get; set; }
        [Inject] public IPool<EventFishObjectView> EventFishObjectsPool { get; set; }
        [Inject] public IPool<EventObstacleObjectView> EventObstacleObjectPool { get; set; }
        
        [Inject] public IPool<EventTargetObjectView> EventTargetObjectPool { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        private List<EventEnemyObjectView> _enemyEventOnLayer = new List<EventEnemyObjectView>();
        private List<EventGrassObjectView> _grassEventOnLayer = new List<EventGrassObjectView>();
        private List<EventFishObjectView> _fishEventOnLayer = new List<EventFishObjectView>();
        private List<EventObstacleObjectView> _obstacleEventsOnLayer = new List<EventObstacleObjectView>();
        private List<EventTargetObjectView> _targetEventsOnLayer = new List<EventTargetObjectView>();
        public override void SetData(EventLayerDataprovider dataProvider, int index)
        {
            base.SetData(dataProvider, index);
            int label = dataProvider.Index % LevelEditorControler.EventsPerBeat;
            string beatText = (dataProvider.Index / LevelEditorControler.EventsPerBeat).ToString();
            string eventText = label.ToString();
            string time = dataProvider.Time.ToString();
         
            if (label == 0)
            {
                View.LevelLabel.text = $"{beatText}";

            }
            else
            {
                View.LevelLabel.text = $"{beatText}.{eventText}";
            }
            
            View.TimeLabel.text = $"({time})";
            View.transform.position = EditorHelpers.GetPointOnCircle(0f, dataProvider.Radius);


            // if not empty return previous instances

            if (dataProvider.EnemiesOnLayer != null)
            {
                foreach (var eventOnLayer in dataProvider.EnemiesOnLayer)
                {
                    var enemyEvent = EventEnemyObjectsPool.GetInstance();
                    enemyEvent.Deploy();
                    //set new data
                    enemyEvent.SetData(eventOnLayer, dataProvider.Radius);
                    // add to local colection
                    _enemyEventOnLayer.Add(enemyEvent);
                    
                }
            }

            if (dataProvider.GrassOnLayer != null)
            {
                foreach (var grassOnLayer in dataProvider.GrassOnLayer)
                {
                    var grassEvent = EventGrassObjectsPool.GetInstance();
                    grassEvent.Deploy();
                    //set new data
                    grassEvent.SetData(grassOnLayer, dataProvider.Radius);
                    // add to local colection
                    _grassEventOnLayer.Add(grassEvent);
                }
            }

            if (dataProvider.FishOnLayer != null)
            {
                foreach (var eventOnLayer in dataProvider.FishOnLayer)
                {
                    var fishEvent = EventFishObjectsPool.GetInstance();
                    fishEvent.Deploy();
                    //set new data
                    fishEvent.SetData(eventOnLayer, dataProvider.Radius);
                    // add to local colection
                    _fishEventOnLayer.Add(fishEvent);
                }
            }

            if (dataProvider.ObstaceOnLayer != null)
            {
                foreach (var eventOnLayer in dataProvider.ObstaceOnLayer)
                {
                    var obstacleEvent = EventObstacleObjectPool.GetInstance();
                   obstacleEvent.Deploy();
                    //set new data
                    obstacleEvent.SetData(eventOnLayer, dataProvider.Radius);
                    // add to local colection
                    _obstacleEventsOnLayer.Add(obstacleEvent);
                }
            }
            
            if (dataProvider.TargetsOnLayer != null)
            {
                foreach (var eventOnLayer in dataProvider.TargetsOnLayer)
                {
                    var targetEvent = EventTargetObjectPool.GetInstance();
                    targetEvent.Deploy();
                    //set new data
                    targetEvent.SetData(eventOnLayer, dataProvider.Radius);
                    // add to local colection
                    _targetEventsOnLayer.Add(targetEvent);
                }
            }
            
            if (dataProvider.Index % LevelEditorControler.EventsPerBeat == 0)
            {
                View.DrawCircle(dataProvider.Radius, 0.07f, Color.red);
            }
            else
            {
                View.DrawCircle(dataProvider.Radius, 0.07f, Color.blue);
 
            }

        }

        public void ClearAllEvents()
        {
            foreach (var eventObject in _enemyEventOnLayer)
            {
                EventEnemyObjectsPool.ReturnInstance(eventObject);
            }

            foreach (var eventObject in _grassEventOnLayer)
            {
                EventGrassObjectsPool.ReturnInstance(eventObject);
            }

            foreach (var eventObject in _fishEventOnLayer)
            {
                EventFishObjectsPool.ReturnInstance(eventObject);
            }
            foreach (var eventObject in _obstacleEventsOnLayer)
            {
                EventObstacleObjectPool.ReturnInstance(eventObject);
            }

            foreach (var eventObject in _targetEventsOnLayer)
            {
                EventTargetObjectPool.ReturnInstance(eventObject);
            }
            _enemyEventOnLayer.Clear();
            _grassEventOnLayer.Clear();
            _fishEventOnLayer.Clear();
            _obstacleEventsOnLayer.Clear();
            _targetEventsOnLayer.Clear();
        }

        public override void SetSelected(bool selected)
        {
            base.SetSelected(selected);
        }
    }
}