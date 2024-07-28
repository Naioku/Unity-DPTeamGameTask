using System;
using System.Collections.Generic;
using UnityEngine;

namespace DPTeam.InteractionSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class Interaction : MonoBehaviour
    {
        private readonly Dictionary<Enums.InteractionType, Dictionary<Enums.InteractionState, Action<InteractionActionArgs>>> interactionTypeLookup = new();

        private void Awake() => InitializeActions();

        private void InitializeActions()
        {
            foreach (Enums.InteractionType actionType in Enum.GetValues(typeof(Enums.InteractionType)))
            {
                var initialActions = new Dictionary<Enums.InteractionState, Action<InteractionActionArgs>>
                { 
                    {Enums.InteractionState.EnterType, null},
                    {Enums.InteractionState.Tick, null},
                    {Enums.InteractionState.ExitType, null},
                    {Enums.InteractionState.EnterInteraction, null},
                    {Enums.InteractionState.ExitInteraction, null}
                };
                
                interactionTypeLookup.Add(actionType, initialActions);
            }
        }
    
        public void SetAction(
            Enums.InteractionType interactionType,
            Enums.InteractionState interactionState,
            Action<InteractionActionArgs> action)
        {
            interactionTypeLookup[interactionType][interactionState] = action;
        }
    
        public void Interact(InteractionData interactionData)
        {
            if (!interactionTypeLookup.TryGetValue(interactionData.InteractionType, out var actions)) return;
            if (!actions.ContainsKey(interactionData.InteractionState)) return;
            
            actions[interactionData.InteractionState]?.Invoke(interactionData.InteractionActionArgs);
        }
    
        public struct InteractionData
        {
            public Enums.InteractionType InteractionType { get; set; }
            public Enums.InteractionState InteractionState { get; set; }
            public InteractionActionArgs InteractionActionArgs { get; set; }
        }
        
        public struct InteractionActionArgs
        {
            public RaycastHit HitInfo { get; set; }
        }
    }
}