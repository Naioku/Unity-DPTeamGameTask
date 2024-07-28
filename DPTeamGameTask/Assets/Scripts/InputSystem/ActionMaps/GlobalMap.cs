using UnityEngine.InputSystem;

namespace DPTeam.InputSystem.ActionMaps
{
    public class GlobalMap : ActionMap, Controls.IGlobalActions
    {
        public ActionData OnClickInteractionData { get; }
        public ActionData OnDeselectData { get; }
        
        public GlobalMap(Controls.GlobalActions actionMap)
        {
            this.actionMap = actionMap.Get();
            actionMap.SetCallbacks(this);
        
            OnClickInteractionData = new ActionData(actionMap.ClickInteraction);
            OnDeselectData = new ActionData(actionMap.Deselect);
        }

        public void OnClickInteraction(InputAction.CallbackContext context) => OnClickInteractionData.Invoke(context.phase);
        public void OnDeselect(InputAction.CallbackContext context) => OnDeselectData.Invoke(context.phase);
    }
}