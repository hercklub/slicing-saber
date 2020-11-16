using Common;
using LevelEditor.Common;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace LevelEditor.Interactions
{
    public class HandlesContainer : View
    {
        public GameObject PosHandle;
        public GameObject HeightHandle;

        public Camera RaycastCamera;
        public bool SphericalHeight = false;

        private Vector3 _screenPoint;
        private Vector3 _offset;

        private bool _posHandlePressed = false;
        private bool _heiHandlePressed = false;

        private float _pos;
        private float _height;

        private Material _posHandleMaterial;
        private Material _heiHandleMaterial;

        
        

        public Signal<float, bool> PosMovedSignal = new Signal<float, bool>();
        public Signal<float, bool> HeightMovedSignal = new Signal<float, bool>();

        protected override void Awake()
        {
            base.Awake();
            _posHandleMaterial = PosHandle.GetComponent<MeshRenderer>().material;
            _heiHandleMaterial = HeightHandle.GetComponent<MeshRenderer>().material;
            
            TogglePosHandle(false);
            ToggleHeiHandle(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, LayerMasks.Dome))
                {
                    //retarded comparison v2
                    if (hit.transform.gameObject == PosHandle)
                    {
                        _posHandlePressed = true;
                        TogglePosHandle(true);

                        _screenPoint = RaycastCamera.WorldToScreenPoint(gameObject.transform.position);
                        _offset = gameObject.transform.position -
                                 RaycastCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                     Input.mousePosition.y,
                                     _screenPoint.z));
                    }
                    else if (hit.transform.gameObject == HeightHandle)
                    {
                        _heiHandlePressed = true;
                        ToggleHeiHandle(true);
                        _screenPoint = RaycastCamera.WorldToScreenPoint(gameObject.transform.position);
                        _offset = gameObject.transform.position -
                                 RaycastCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                     Input.mousePosition.y,
                                     _screenPoint.z));
                    }
                }
                
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_posHandlePressed)
                {
                    //send event
                    PosMovedSignal.Dispatch(_pos, true);
                    _posHandlePressed = false;
                    TogglePosHandle(false);

                }
                else if (_heiHandlePressed)
                {
                    HeightMovedSignal.Dispatch(_height, true);
                    _heiHandlePressed = false;
                    ToggleHeiHandle(false);

                }

            }
            
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            Vector3 cursorPosition = RaycastCamera.ScreenToWorldPoint(cursorPoint) + _offset;
            if (_posHandlePressed)
            {
                float angle = Mathf.Atan2(cursorPosition.x, cursorPosition.z) * Mathf.Rad2Deg;
                _pos = angle / 360f;
                PosMovedSignal.Dispatch(_pos, false);

            }
            else if (_heiHandlePressed)
            {
                if (SphericalHeight)
                {
                    float angle = Mathf.Acos(Mathf.Clamp(cursorPosition.y / 20f, -1f, 1f)) * Mathf.Rad2Deg;
                    _height = -(angle - 90f) / 360f;
                }
                else
                {
                    _height = cursorPosition.y;
                }
                
                HeightMovedSignal.Dispatch(_height, false);

            }
        }

        private void TogglePosHandle(bool isPressed)
        {
            _posHandleMaterial.SetColor("_Color", isPressed ? ColorHelper.EditorHandlePressedColor: ColorHelper.EditorHandlerColor);
        }
        private void ToggleHeiHandle(bool isPressed)
        {
            _heiHandleMaterial.SetColor("_Color", isPressed ? ColorHelper.EditorHandlePressedColor: ColorHelper.EditorHandlerColor);
        }
    }
}