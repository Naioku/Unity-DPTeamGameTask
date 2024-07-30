//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Logic/Core/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace DPTeam.InputSystem
{
    public partial class @Controls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @Controls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Global"",
            ""id"": ""d158cff9-c37e-48e6-9ecf-d64eeb80b3a2"",
            ""actions"": [
                {
                    ""name"": ""ClickInteraction"",
                    ""type"": ""Button"",
                    ""id"": ""cc5d743c-90ac-4915-8a33-def7bf33b8cf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Deselect"",
                    ""type"": ""Button"",
                    ""id"": ""7d72d1fe-ec98-4ae5-96fe-9e1a9359005e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShowMenu"",
                    ""type"": ""Button"",
                    ""id"": ""7c09fb77-67ea-43b4-85de-2c118a81f87d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraZoom"",
                    ""type"": ""Value"",
                    ""id"": ""b78e13a0-8c10-4146-97a2-9de4cdd46271"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7c5d4398-5c4e-4fdb-92ea-9e11a43baecd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClickInteraction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdf554f9-fbbb-4a65-a3e9-0e272a10a770"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deselect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5372e3a-69df-404b-9a2a-d4934b5a5c7a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShowMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1db19b99-5808-469e-963a-d78bed321388"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Global
            m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
            m_Global_ClickInteraction = m_Global.FindAction("ClickInteraction", throwIfNotFound: true);
            m_Global_Deselect = m_Global.FindAction("Deselect", throwIfNotFound: true);
            m_Global_ShowMenu = m_Global.FindAction("ShowMenu", throwIfNotFound: true);
            m_Global_CameraZoom = m_Global.FindAction("CameraZoom", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Global
        private readonly InputActionMap m_Global;
        private List<IGlobalActions> m_GlobalActionsCallbackInterfaces = new List<IGlobalActions>();
        private readonly InputAction m_Global_ClickInteraction;
        private readonly InputAction m_Global_Deselect;
        private readonly InputAction m_Global_ShowMenu;
        private readonly InputAction m_Global_CameraZoom;
        public struct GlobalActions
        {
            private @Controls m_Wrapper;
            public GlobalActions(@Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @ClickInteraction => m_Wrapper.m_Global_ClickInteraction;
            public InputAction @Deselect => m_Wrapper.m_Global_Deselect;
            public InputAction @ShowMenu => m_Wrapper.m_Global_ShowMenu;
            public InputAction @CameraZoom => m_Wrapper.m_Global_CameraZoom;
            public InputActionMap Get() { return m_Wrapper.m_Global; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
            public void AddCallbacks(IGlobalActions instance)
            {
                if (instance == null || m_Wrapper.m_GlobalActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_GlobalActionsCallbackInterfaces.Add(instance);
                @ClickInteraction.started += instance.OnClickInteraction;
                @ClickInteraction.performed += instance.OnClickInteraction;
                @ClickInteraction.canceled += instance.OnClickInteraction;
                @Deselect.started += instance.OnDeselect;
                @Deselect.performed += instance.OnDeselect;
                @Deselect.canceled += instance.OnDeselect;
                @ShowMenu.started += instance.OnShowMenu;
                @ShowMenu.performed += instance.OnShowMenu;
                @ShowMenu.canceled += instance.OnShowMenu;
                @CameraZoom.started += instance.OnCameraZoom;
                @CameraZoom.performed += instance.OnCameraZoom;
                @CameraZoom.canceled += instance.OnCameraZoom;
            }

            private void UnregisterCallbacks(IGlobalActions instance)
            {
                @ClickInteraction.started -= instance.OnClickInteraction;
                @ClickInteraction.performed -= instance.OnClickInteraction;
                @ClickInteraction.canceled -= instance.OnClickInteraction;
                @Deselect.started -= instance.OnDeselect;
                @Deselect.performed -= instance.OnDeselect;
                @Deselect.canceled -= instance.OnDeselect;
                @ShowMenu.started -= instance.OnShowMenu;
                @ShowMenu.performed -= instance.OnShowMenu;
                @ShowMenu.canceled -= instance.OnShowMenu;
                @CameraZoom.started -= instance.OnCameraZoom;
                @CameraZoom.performed -= instance.OnCameraZoom;
                @CameraZoom.canceled -= instance.OnCameraZoom;
            }

            public void RemoveCallbacks(IGlobalActions instance)
            {
                if (m_Wrapper.m_GlobalActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IGlobalActions instance)
            {
                foreach (var item in m_Wrapper.m_GlobalActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_GlobalActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public GlobalActions @Global => new GlobalActions(this);
        public interface IGlobalActions
        {
            void OnClickInteraction(InputAction.CallbackContext context);
            void OnDeselect(InputAction.CallbackContext context);
            void OnShowMenu(InputAction.CallbackContext context);
            void OnCameraZoom(InputAction.CallbackContext context);
        }
    }
}
