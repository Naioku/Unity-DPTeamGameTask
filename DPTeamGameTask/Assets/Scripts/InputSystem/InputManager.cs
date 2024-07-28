using System.Collections.Generic;
using DPTeam.InputSystem.ActionMaps;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DPTeam.InputSystem
{
    public class InputManager
    {
        private readonly Controls controls = new();
        private readonly List<ActionMap> mapsList = new();
        
        public GlobalMap GlobalMap { get; private set; }
        public Vector2 CursorPosition { get; private set; }
        
        public void Awake()
        {
            Managers.Instance.UpdateManager.UpdateActions.AddAction(UpdateCursorPosition);
            InitializeMaps();
            BuildMapsList();
        }

        public void Destroy()
        {
            Managers.Instance.UpdateManager.UpdateActions.RemoveAction(UpdateCursorPosition);
        }

        public void DisableAllMaps()
        {
            foreach (ActionMap actionMap in mapsList)
            {
                actionMap.Disable();
            }
        }

        private void UpdateCursorPosition() => CursorPosition = Mouse.current.position.ReadValue();

        private void InitializeMaps()
        {
            GlobalMap = new GlobalMap(controls.Global);
        }

        private void BuildMapsList()
        {
            mapsList.Add(GlobalMap);
        }
    }
}