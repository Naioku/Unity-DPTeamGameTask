using System;
using DPTeam.InputSystem;
using UnityEngine;

namespace DPTeam.InteractionSystem
{
    [Serializable]
    public class InteractionController
    {
        private const int HitResultsBufferSize = 10;
        [SerializeField] private float interactionRange = 99;
        
        [SerializeField] private LayerMask interactionLayer;
        [SerializeField] private LayerMask blockingLayers;

        private RaycastHit[] hitResultsBuffer = new RaycastHit [HitResultsBufferSize];
        private Camera mainCamera;
        private Interaction currentInteraction;
        private RaycastHit currentHitInfo;
        private Guid performInteractionId;
            
        /// <summary>
        /// Interaction type, which should be currently used like: Hover, Click, Key. Should be set by input.
        /// </summary>
        private Enums.InteractionType currentInteractionType = DefaultInteractionType;
        private const Enums.InteractionType DefaultInteractionType = Enums.InteractionType.Hover;

        public void Awake(CameraController cameraController)
        {
            mainCamera = cameraController.CameraObject;
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
		    
            Interaction currentlyCheckedInteraction = null;
            int raycastHitsNumber = Physics.RaycastNonAlloc(ray, hitResultsBuffer, interactionRange);
            for (int i = 0; i < raycastHitsNumber; i++)
            {
                Transform hitResultTransform = hitResultsBuffer[i].collider.transform;
                int checkedLayer = hitResultTransform.gameObject.layer;
                if (Utility.IsLayerInLayerMask(blockingLayers, checkedLayer)) return;

                if (Utility.GetMask(checkedLayer) == interactionLayer)
                {
                    if (!hitResultTransform.TryGetComponent(out currentlyCheckedInteraction))
                    {
                        Debug.LogWarning($"Game object {hitResultTransform.name} hasn't got {nameof(Interaction)} component," +
                                         $"but has layer: {LayerMask.LayerToName(interactionLayer)}.");
                    }
                        
                    break;
                }
            }

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