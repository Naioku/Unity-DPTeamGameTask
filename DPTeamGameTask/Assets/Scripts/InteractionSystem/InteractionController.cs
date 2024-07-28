using System;
using DPTeam.InputSystem;
using UnityEngine;

namespace DPTeam.InteractionSystem
{
    [Serializable]
    public class InteractionController
    {
        [SerializeField] private float interactionRange = 99;
        [SerializeField] private LayerMask interactionLayer;

        private Camera mainCamera;
        private Interaction currentInteraction;
        private RaycastHit currentHitInfo;
        private Guid performInteractionId;
            
        /// <summary>
        /// Interaction type, which should be currently used like: Hover, Click, Key. Should be set by input.
        /// </summary>
        private Enums.InteractionType currentInteractionType = DefaultInteractionType;
        private const Enums.InteractionType DefaultInteractionType = Enums.InteractionType.Hover;

        public void Awake(Camera camera)
        {
            mainCamera = camera;
            AddInput();
        }

        public void OnDestroy() => RemoveInput();

        public void StartInteracting() =>
            Managers.Instance.UpdateManager.UpdateActions.AddAction(PerformInteraction);

        public void StopInteracting() =>
            Managers.Instance.UpdateManager.UpdateActions.RemoveAction(PerformInteraction);

        private void AddInput()
        {
            InputManager inputManager = Managers.Instance.InputManager;
            inputManager.GlobalMap.OnClickInteractionData.Performed += StartClickInteraction;
            inputManager.GlobalMap.OnClickInteractionData.Canceled += StopClickInteraction;
        }
        
        private void RemoveInput()
        {
            InputManager inputManager = Managers.Instance.InputManager;
            inputManager.GlobalMap.OnClickInteractionData.Performed -= StartClickInteraction;
            inputManager.GlobalMap.OnClickInteractionData.Canceled -= StopClickInteraction;
        }

        private void StartClickInteraction() => SwitchInteractionType(Enums.InteractionType.Click);
        private void StopClickInteraction() => SwitchInteractionType(DefaultInteractionType);

        private void PerformInteraction()
        {
            Ray ray = mainCamera.ScreenPointToRay(Managers.Instance.InputManager.CursorPosition);
            Interaction currentlyCheckedInteraction = !Physics.Raycast(ray, out currentHitInfo, interactionRange, interactionLayer) ?
                null : currentHitInfo.transform.GetComponent<Interaction>();
            
            if (currentlyCheckedInteraction != currentInteraction)
            {
                SwitchInteraction(currentlyCheckedInteraction);
            }
            
            Interact(Enums.InteractionState.Tick);
        }

        private void SwitchInteraction(Interaction currentlyCheckedInteraction)
        {
            Interact(Enums.InteractionState.ExitInteraction);
            currentInteraction = currentlyCheckedInteraction;
            if (currentInteraction)
            {
                Interact(Enums.InteractionState.EnterInteraction);
            }
        }

        private void SwitchInteractionType(Enums.InteractionType interactionType)
        {
            Interact(Enums.InteractionState.ExitType);
            currentInteractionType = interactionType;
            Interact(Enums.InteractionState.EnterType);
        }
        
        private void Interact(Enums.InteractionState interactionState)
        {
            if (!currentInteraction) return;
            
            Interaction.InteractionActionArgs interactionActionArgs = new Interaction.InteractionActionArgs
            {
                HitInfo = currentHitInfo,
            };
            
            Interaction.InteractionData interactionData = new Interaction.InteractionData
            {
                InteractionType = currentInteractionType,
                InteractionState = interactionState,
                InteractionActionArgs = interactionActionArgs
            };

            currentInteraction.Interact(interactionData);

            if (interactionState == Enums.InteractionState.ExitInteraction)
            {
                currentInteraction = null;
            }
        }
    }
}