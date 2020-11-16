using System;
using System.Linq;
using LevelEditor.Interactions;
using Plugins.Framewerk;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LevelEditor
{
    public class PlaybackControlerMediator : Mediator
    {
        [Inject] public PlaybackControlerView View { get; set; }
        [Inject] public ViewConfig ViewConfig { get; set; }
        [Inject] public ILevelEditorControler LevelEditorControler { get; set; }
        [Inject] public GhostObjectUpdateSignal GhostObjectUpdateSignal { get; set; }
        [Inject] public IEnemyEventsModel EnemyEventsModel { get; set; }

        [Inject] public EnviromentMovedSignal EnviromentMovedSignal { get; set; }


        private EventEnemyObjectView _selectedObject;

        public override void OnRegister()
        {
            base.OnRegister();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }


        private void Update()
        {
            // scroll
            LevelEditorControler.ChangeCurrentIndex((int) Input.mouseScrollDelta.y);

            //select
            if (Input.GetMouseButtonDown(0) && LevelEditorControler.EditorMode == EditorMode.Edit && !EventSystem.current.IsPointerOverGameObject(-1))
            {
                RaycastHit hit;
                Ray ray = ViewConfig.Camera3d.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    int hitLayer = hit.transform.gameObject.layer;
                    IEditorData data = null;
                    float radius = 0f;
                    if (hitLayer == LayerMasks.GrassLayer)
                    {
                        var hitData = hit.collider.transform.parent.GetComponent<EventGrassObjectView>();
                        data = hitData.Data;
                        radius = hitData.Radius;

                    }
                    else if (hitLayer == LayerMasks.EnemyLayer)
                    {
                        var hitData = hit.collider.transform.parent.GetComponent<EventEnemyObjectView>();
                        data = hitData.Data;
                        radius = hitData.Radius;

                    }
                    else if (hitLayer == LayerMasks.FishLayer)
                    {
                        var hitData =  hit.collider.transform.parent.GetComponent<EventFishObjectView>();
                        data = hitData.Data;
                        radius = hitData.Radius;
                    }
                    else if (hitLayer == LayerMasks.ObstacleLayer)
                    {
                        var hitData =  hit.collider.transform.parent.GetComponent<EventObstacleObjectView>();
                        data = hitData.Data;
                        radius = hitData.Radius;
                    }
                    else if (hitLayer == LayerMasks.TargetLayer)
                    {
                        var hitData =  hit.collider.transform.parent.GetComponent<EventTargetObjectView>();
                        data = hitData.Data;
                        radius = hitData.Radius;
                    }

                    if (data == null)
                        return;
                    //
                    if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
                    {
                        LevelEditorControler.SelectedEvents.Clear();
                    }

                    if (Input.GetKey(KeyCode.LeftShift))
                    { 
                        // multi select
                        int lastSelectedIndex = 0;
                        if (LevelEditorControler.SelectedEvents.Count > 0)
                        {
                            var lastSelected =
                                LevelEditorControler.SelectedEvents[LevelEditorControler.SelectedEvents.Count - 1];
                            if (lastSelected != null)
                            {
                                lastSelectedIndex = lastSelected.LayerIndex;
                            }
                        }

                        int currentSelectedIndex = data.LayerIndex;

                       for (int i = lastSelectedIndex; i <= currentSelectedIndex; i++)
                       {
                           var eventsOnLayer = EnemyEventsModel.GetAllEventsOnLayer(i);
                           for (int j = 0; j < eventsOnLayer.Count; j++)
                           {
                               var layerData = eventsOnLayer[j];
                               if (!LevelEditorControler.SelectedEvents.Contains(layerData))
                               {
                                   LevelEditorControler.SelectedEvents.Add(layerData);
                               }
                           }
                       }
                       
                       LevelEditorControler.UpdateSelected();
                       
                    }
                    else
                    {
                        if (!LevelEditorControler.SelectedEvents.Contains(data))
                        {
                            LevelEditorControler.SelectedEvents.Add(data);
                            LevelEditorControler.UpdateSelected();
                        }
                    }
   
                }
            }
            // cancel selection
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelEditorControler.SelectedEvents.Clear();
                LevelEditorControler.UpdateSelected();
            }

            // duplicate
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.D))
            {
                LevelEditorControler.DuplicateSelected();
                Debug.Log("DUPLICATE");
            }
        
            //undo
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) &&
                Input.GetKeyDown(KeyCode.X))
            {
                // CTRL + Z
                Debug.Log("UNDO!");
                LevelEditorControler.Undo();
            }

            //redo
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) &&
                Input.GetKeyDown(KeyCode.U))
            {
                // CTRL + Y
                Debug.Log("REDO!");
                LevelEditorControler.Redo();
            }

            //add 
            if (LevelEditorControler.EditorMode == EditorMode.Add && Cursor.visible && !EventSystem.current.IsPointerOverGameObject(-1))
            {
                RaycastHit hit;
                Ray ray = ViewConfig.Camera3d.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform.gameObject.layer == LayerMasks.GroundLayer)
                    {
                        float ratio = LevelEditorControler.ArenaRadius / LevelEditorControler.Bmp;
                        var ditanceToCenter = EditorHelpers.GetPointRadius(Vector3.zero, hit.point);
                        int layer = Mathf.RoundToInt(ditanceToCenter / ratio) ;
                        float angle = Mathf.Atan2(hit.point.x, hit.point.z) * Mathf.Rad2Deg;
                        var pos = EditorHelpers.GetPointOnCircle(angle, layer * ratio);
                        GhostObjectUpdateSignal.Dispatch(pos, LevelEditorControler.CurrentEventType);
                        
                        float posOnLayer =  EditorHelpers.Round(angle / 360f, 2);

                        if (Input.GetMouseButtonDown(0))
                        {
                            layer += LevelEditorControler.CurrentLayerIndex;
                            switch (LevelEditorControler.CurrentEventType)
                            {
                                case EventEditType.None:
                                    break;
                                case EventEditType.Red:
                                    EnemyEventsModel.AddEnemyEvent(new EditorEnemyData(layer - 1, layer - 1, posOnLayer, EnemyAirType.Normal, eCutDir.Down));
                                    break;
                                case EventEditType.Yellow:
                                    EnemyEventsModel.AddEnemyEvent(new EditorEnemyData(layer - 1, layer - 1, posOnLayer, EnemyAirType.Collectable, eCutDir.Down));
                                    break;
                                case EventEditType.Bullet:
                                    EnemyEventsModel.AddEnemyEvent(new EditorEnemyData(layer - 1, layer - 1, posOnLayer, EnemyAirType.Bullet, eCutDir.Down));
                                    break;
                                case EventEditType.SideBomb:
                                    EnemyEventsModel.AddFishEvent(layer - 1, layer - 1, posOnLayer, BombType.Water);
                                    break;
                                case EventEditType.Target:
                                    EnemyEventsModel.AddTargetEvent(layer - 1, layer - 1, posOnLayer, 1f, TargetType.Normal);
                                    break;
                                case EventEditType.Grass:
                                    EnemyEventsModel.AddGrassEvent(layer - 1, layer - 1, posOnLayer);
                                    break;
                                case EventEditType.Obstacle:
                                    EnemyEventsModel.AddObstacleEvent(new EditorObstacleData(layer - 1, layer - 1, posOnLayer, 1f));
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            EnviromentMovedSignal.Dispatch();
                            EnemyEventsModel.SaveChange();
                        }
                    }
                    else
                    {
                        GhostObjectUpdateSignal.Dispatch(Vector3.down * 10f, LevelEditorControler.CurrentEventType);

                    }
                }
            }
            else
            {
                GhostObjectUpdateSignal.Dispatch(Vector3.down * 10f, LevelEditorControler.CurrentEventType);
            }
            
        }
    }
}