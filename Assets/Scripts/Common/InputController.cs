using Framewerk;
using Plugins.Framewerk;
using UnityEngine;

namespace Common
{
    public interface IInputController
    {
        void Init();
        void Destroy();

        Vector3 MouseDirection { get; }
    }

    public class InputController : IInputController
    {
        [Inject] public IUpdater Updater { get; set; }
        [Inject] public ViewConfig ViewConfig { get; set; }


        public Vector3 MouseDirection => _mouseDirection;
        private Vector3 _mouseDirection;
        
        public void Init()
        {
            Updater.EveryFrame(InputUpdate);
        }

        public void Destroy()
        {
            Updater.RemoveFrameAction(InputUpdate);
        }
        
        private void InputUpdate()
        {
            var ray = ViewConfig.Camera3d.ScreenPointToRay(Input.mousePosition);
            _mouseDirection = ray.direction;
        }
    }
}