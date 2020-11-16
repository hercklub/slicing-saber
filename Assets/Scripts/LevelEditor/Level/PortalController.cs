using System.Collections;
using System.Collections.Generic;
using Framewerk;
using Framewerk.Managers;
using Framewerk.StrangeCore;
using strange.extensions.signal.impl;
using UnityEngine;

namespace BehindYou.Scripts.Controller
{
    public enum Dimension
    {
        Good,
        Evil
    }

    public interface IPortalControler
    {
        void Init();
        void SubscribeObject(GameObject obj, Dimension dimension);
        void UnSubscribeObject(GameObject obj);
        Dimension CurrentDimension { get; }
        Signal<Dimension> PortalDidPassedSignal { get; }
    }

    public class PortalController : IPortalControler, IDestroyable
    {
        [Inject] public PassedPortalSignal PassedPortalSignal { get; set; }

        private List<GameObject> _goodDimnsionObjects = new List<GameObject>(10);
        private List<GameObject> _evilDimnsionObjects = new List<GameObject>(10);

        public Signal<Dimension> PortalDidPassedSignal { get; } = new Signal<Dimension>();

        private Dimension _currentDimension = Dimension.Good;
        public Dimension CurrentDimension => _currentDimension;
        public void Init()
        {
            PassedPortalSignal.AddListener(PortalPassedHandler);
            _currentDimension = Dimension.Good;
            UpdateDimensionLayers();
            PortalDidPassedSignal.Dispatch(_currentDimension);
        }

        public void Destroy()
        {
            PassedPortalSignal.RemoveListener(PortalPassedHandler);
        }


        public void SubscribeObject(GameObject obj, Dimension dimension)
        {
            if (dimension == Dimension.Good)
            {
                _goodDimnsionObjects.Add(obj);
            }
            else
            {
                _evilDimnsionObjects.Add(obj);
            }
            
            UpdateDimensionLayers();
        }

        public void UnSubscribeObject(GameObject obj)
        {
            if (_goodDimnsionObjects.Contains(obj))
            {
                _goodDimnsionObjects.Remove(obj);
            }
            else if (_evilDimnsionObjects.Contains(obj))
            {
                _evilDimnsionObjects.Remove(obj);
            }
        }

        private void PortalPassedHandler()
        {
            _currentDimension = _currentDimension == Dimension.Good ? Dimension.Evil : Dimension.Good;
            UpdateDimensionLayers();
            PortalDidPassedSignal.Dispatch(_currentDimension);
        }

        private void UpdateDimensionLayers()
        {
            int goodDimensionMask = _currentDimension == Dimension.Good
                ? LayerMasks.VisibleDimension
                : LayerMasks.InVisibleDimension;
            int evilDimensionMask = _currentDimension == Dimension.Evil
                ? LayerMasks.VisibleDimension
                : LayerMasks.InVisibleDimension;

            for (int i = 0; i < _goodDimnsionObjects.Count; i++)
            {
                _goodDimnsionObjects[i].layer = goodDimensionMask;
            }

            for (int i = 0; i < _evilDimnsionObjects.Count; i++)
            {
                _evilDimnsionObjects[i].layer = evilDimensionMask;
            }
        }
    }
}