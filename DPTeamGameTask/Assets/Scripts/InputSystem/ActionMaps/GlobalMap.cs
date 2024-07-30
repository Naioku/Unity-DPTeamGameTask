using UnityEngine;
using UnityEngine.InputSystem;

namespace DPTeam.InputSystem.ActionMaps
{
    public class GlobalMap : ActionMap, Controls.IGlobalActions
    {
        public ActionData OnClickInteractionData { get; }
        public ActionData OnDeselectData { get; }
        public ActionData OnShowMenuData { get; }
        public ActionData<float> OnCameraZoomData { get; set; }

        public GlobalMap(Controls.GlobalActions actionMap)
        {
            this.actionMap = actionMap.Get();
            actionMap.SetCallbacks(this);
        
            OnClickInteractionData = new ActionData(actionMap.ClickInteraction);
            OnDeselectData = new ActionData(actionMap.Deselect);
            OnShowMenuData = new ActionData(actionMap.ShowMenu);
            OnCameraZoomData = new ActionData<float>(actionMap.CameraZoom);
        }

        public void OnClickInteraction(InputAction.CallbackContext context) => OnClickInteractionData.Invoke(context.phase);
        public void OnDeselect(InputAction.CallbackContext context) => OnDeselectData.Invoke(context.phase);
        public void OnShowMenu(InputAction.CallbackContext context) => OnShowMenuData.Invoke(context.phase);
        public void OnCameraZoom(InputAction.CallbackContext context) => OnCameraZoomData.Invoke(context.phase, context.ReadValue<Vector2>().y);
    }
}