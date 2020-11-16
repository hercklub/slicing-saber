using System.Collections.Generic;
using Framewerk.UI.List;
using Level;
using UnityEngine;

namespace LevelEditor.EventsList
{
    public class EventListMediator : ListMediator<EventListView, EventLayerDataprovider>
    {
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }

        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }

        [Inject] public EventObjectSelectedSignal EventObjectSelectedSignal { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            
            RefreshData();
            EnviromentMovedSignal.AddListener(EnviromentMovedHandler);
        }

        private void EnviromentMovedHandler()
        {
            RefreshData();
        }


        public void RefreshData()
        {
            int count = EnemyEventsModel.VisibleLayerIndexes.Count;

            if (count == 0)
                return;

            var eventItem = new List<EventLayerDataprovider>();

            float ratio = LevelEditorControler.ArenaRadius / LevelEditorControler.Bmp;
            int dataIndex = 0;
            float time = 0f;
            float timeRatio = (LevelEditorControler.Bmp * LevelEditorControler.EventsPerBeat);
            timeRatio = timeRatio > 0 ? timeRatio : 1f;
            for (int i = 0; i < count; i++)
            {
                time = 60f / timeRatio;
                dataIndex = EnemyEventsModel.VisibleLayerIndexes[i];
                eventItem.Add(new EventLayerDataprovider(dataIndex, (i + 1) * ratio, dataIndex * time,
                    EnemyEventsModel.GetAllEnemisOnLayer(dataIndex), EnemyEventsModel.GetAllGrasssOnLayer(dataIndex),
                    EnemyEventsModel.GetAllFishOnLayer(dataIndex), EnemyEventsModel.GetAllObstaclesOnLayer(dataIndex), EnemyEventsModel.GetAllTargetsOnLayer(dataIndex)));
            }


            for (int i = 0; i < ItemMediators.Count; i++)
            {
                var item = ItemMediators[i] as EventItemMediator;
                if (item != null)
                {
                    item.ClearAllEvents();
                }
            }

            SetData(eventItem);

            LevelEditorControler.UpdateSelected();
        }
    }
}