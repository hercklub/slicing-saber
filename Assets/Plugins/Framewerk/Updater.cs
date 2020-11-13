using System;
using System.Collections.Generic;

using UnityEngine;

namespace Framewerk
{
    public interface IUpdater
    {
        /// Provide action which will be called every Update
        void EveryFrame(Action action);
        /// Provide action which will be called every Nth Update
        void EveryNthFrame(int n, Action action);
        /// Remove Action from list of actions called on Update
        void RemoveFrameAction(Action action);
        
        /// Provide action which will be called every FixedUpdate
        void EveryStep(Action action);
        /// Provide action which will be called every Nth FixedUpdate
        void EveryNthStep(int n, Action action);
        /// Remove Action from list of actions called on FixedUpdate
        void RemoveStepAction(Action action);

        
        /// Provide action which will be called every LateUpdate
        void EveryLateUpdate(Action action);
        /// Provide action which will be called every Nth LateUpdate
        void EveryNthLateUpdate(int n, Action action);
        /// Remove Action from list of actions called on LateUpdate
        void RemoveLateAction(Action action);
        void Reset();
    }

    public class Updater : SingletonMono<Updater>, IUpdater
    {
        private class FrameAction
        {
            public Action Action;
            public int FrameSampling;
            public Exception ReportedException;
        }

        private List<FrameAction> frameActions;
        private List<FrameAction> toAddActions;
        private List<Action> toRemoveActions;
        private long frameCounter;

        private bool reset = false;

        private ActionUpdater _frameUpdater = new ActionUpdater();
        private ActionUpdater _fixedUpdater = new ActionUpdater();
        private ActionUpdater _lateUpdater = new ActionUpdater();

        public void Reset()
        {
            _frameUpdater.Reset();
            _fixedUpdater.Reset();
            _lateUpdater.Reset();
        }

        #region Update

        public void EveryFrame(Action action)
        {
            _frameUpdater.EveryFrame(action);
        }

        public void EveryNthFrame(int n, Action action)
        {
            _frameUpdater.EveryNthFrame(n, action);
        }

        public void RemoveFrameAction(Action action)
        {
            _frameUpdater.RemoveFrameAction(action);
        }

        #endregion

        #region FixedUpdate

        public void EveryStep(Action action)
        {
            _fixedUpdater.EveryFrame(action);
        }

        public void EveryNthStep(int n, Action action)
        {
            _fixedUpdater.EveryNthFrame(n, action);
        }

        public void RemoveStepAction(Action action)
        {
            _fixedUpdater.RemoveFrameAction(action);
        }

        #endregion
        
        #region LateUpdate

        public void EveryLateUpdate(Action action)
        {
            _lateUpdater.EveryFrame(action);
        }

        public void EveryNthLateUpdate(int n, Action action)
        {
            _lateUpdater.EveryNthFrame(n, action);
        }

        public void RemoveLateAction(Action action)
        {
            _lateUpdater.RemoveFrameAction(action);
        }

        #endregion
        
        protected override void SingletonMonoInit()
        {
            base.SingletonMonoInit();
            gameObject.name = "Updater";
        }
        
        private void Update()
        {
            _frameUpdater.Update();
        }

        private void FixedUpdate()
        {
            _fixedUpdater.Update();
        }

        private void LateUpdate()
        {
            _lateUpdater.Update();
        }
    }
}
