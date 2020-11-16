using Level;
using LevelEditor.Common;
using LevelEditor.EventsList;
using PathCreation;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace LevelEditor.Interactions
{
    public class SelectedObjectView : View
    {
        public GameObject PosHandler;
        public GameObject HeightHandler;
        public GameObject DepthHandler;

        public Vector3 Pos;


        private Material _posHandleMaterial;
        private Material _heiHandleMaterial;
        private Material _depthHandleMaterial;

        public void Init()
        {
            _posHandleMaterial = PosHandler.GetComponent<Renderer>().material;
            _heiHandleMaterial = HeightHandler.GetComponent<Renderer>().material;
            _depthHandleMaterial = DepthHandler.GetComponent<Renderer>().material;
        }

        public void SetData(IEditorData data, float radius)
        {
            var progressInArena = radius / 20f;
            if (data.EnemyType == EnemyType.Air && progressInArena <= 1f)
            {
                EditorEnemyData enemyData = data as EditorEnemyData;
                var vertexPath = EditorHelpers.GetEnemyObjectPath(enemyData, new Bounds(Vector3.zero, Vector3.one * 2f));
                Pos = vertexPath.GetPointAtDistance(progressInArena * vertexPath.length,
                    EndOfPathInstruction.Stop);
                transform.position = Pos;
                Vector3 rotPos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius);
                transform.rotation = Quaternion.LookRotation(new Vector3(rotPos.x, 0f, rotPos.z), Vector3.up);
            }
            else
            {
                Vector3 pos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius);
                pos.y = data.StartHeight;
                transform.position = pos;
                transform.rotation = Quaternion.LookRotation(new Vector3(pos.x, 0f, pos.z), Vector3.up);
            }
        }

        public void SetHeightHandleEnabled(bool isEnabled)
        {
            HeightHandler.SetActive(isEnabled);
        }

        public void SetDepthHandleEnabled(bool isEnabled)
        {
            DepthHandler.SetActive(isEnabled);
        }
        
        public void SetPosition(IEditorData data, float newPos, float radius)
        {
            var progressInArena = radius / 20f;
            if (data.EnemyType == EnemyType.Air && progressInArena <= 1f)
            {
                EditorEnemyData enemyData = data as EditorEnemyData;
                data.StartPos = newPos;
                var vertexPath = EditorHelpers.GetEnemyObjectPath(enemyData, new Bounds(Vector3.zero, Vector3.one * 2f));
                Pos = vertexPath.GetPointAtDistance(progressInArena * vertexPath.length,
                    EndOfPathInstruction.Stop);
                transform.position = Pos;
               
                Vector3 rotPos = EditorHelpers.GetPointOnCircle(data.StartPos * 360f, radius);
                transform.rotation = Quaternion.LookRotation(new Vector3(rotPos.x, 0f, rotPos.z), Vector3.up);
            }
            else
            {
                Vector3 pos = EditorHelpers.GetPointOnCircle(EditorHelpers.Round(newPos, 2) * 360f, radius);
                pos.y = data.StartHeight;
                transform.position = pos;
                transform.rotation = Quaternion.LookRotation(new Vector3(pos.x, 0f, pos.z), Vector3.up);
            }
            
        }

        public void SetHeight(float height)
        {
            Vector3 pos = transform.position;
            pos.y = height;
            transform.position = pos;
        }
        
        public void TogglePosHandle(bool isPressed)
        {
            _posHandleMaterial.SetColor("_Color", isPressed ? ColorHelper.EditorHandlePressedColor: ColorHelper.EditorHandlerColor);
        }
        public void ToggleHeiHandle(bool isPressed)
        {
            _heiHandleMaterial.SetColor("_Color", isPressed ? ColorHelper.EditorHandlePressedColor: ColorHelper.EditorHandlerColor);
        }
        public void ToggleDepthHandle(bool isPressed)
        {
            _depthHandleMaterial.SetColor("_Color", isPressed ? ColorHelper.EditorHandlePressedColor: ColorHelper.EditorHandlerColor);
        }
    }
}