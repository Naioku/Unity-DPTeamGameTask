namespace DPTeam
{
    public abstract class Enums
    {
        public enum SpawnableObjects
        {
            GameBoard,
            GameplayVolume,
            AgentManager,
            Agent
        }
        
        public enum InteractionType
        {
            Hover, Click
        }
        
        public enum InteractionState
        {
            /// <summary>
            /// Sent on entering an interaction type from another one, e.g. Hover -> Click.
            /// </summary>
            EnterType,
            
            /// <summary>
            /// Sent on every frame of interaction lasting.
            /// </summary>
            Tick,
            
            /// <summary>
            /// Sent on exiting an interaction type from another one, e.g. Hover -> Click.
            /// </summary>
            ExitType,
            
            /// <summary>
            /// Sent on entering an interaction collider.
            /// </summary>
            EnterInteraction,
            
            /// <summary>
            /// Sent on exiting an interaction collider.
            /// </summary>
            ExitInteraction
        }
    }
}