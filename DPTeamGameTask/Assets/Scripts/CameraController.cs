using System;
using System.Collections;
using DPTeam.InputSystem;
using DPTeam.UpdateSystem.CoroutineSystem;
using UnityEngine;

namespace DPTeam
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cameraObject;
        [SerializeField] private float borderModeMovementSpeed = 20;
        [SerializeField] private float screenBorderThickness = 10;
        [SerializeField] private ScreenLimits screenLimitsX;
        [SerializeField] private ScreenLimits screenLimitsZ;
        [SerializeField] private Zoom zoom;
        
        private Settings settings;
        private CoroutineManager.CoroutineCaller coroutineCaller;
        private Guid movementCoroutineId;
        
        public Camera CameraObject => cameraObject;

        private void Awake()
        {
            settings = Managers.Instance.GameManager.GameSettings;
            coroutineCaller = Managers.Instance.CoroutineManager.GenerateCoroutineCaller();
            UpdateScreenLimits();
            zoom.Awake(this);
            SetInitPosition();
        }
        
        public void OnDestroy() => StopMovement();

        private void OnDrawGizmos() => zoom.OnDrawGizmos(transform);
        private void OnValidate() => zoom.OnValidate();

        private void AddInput()
        {
            InputManager inputManager = Managers.Instance.InputManager;
            inputManager.GlobalMap.OnCameraZoomData.Performed += zoom.UpdateZoomTarget;
        }

        private void RemoveInput()
        {
            InputManager inputManager = Managers.Instance.InputManager;
            inputManager.GlobalMap.OnCameraZoomData.Performed -= zoom.UpdateZoomTarget;
        }

        public void StartMovement()
        {
            AddInput();
            movementCoroutineId = coroutineCaller.StartCoroutine(Perform2DMovement());
        }

        public void StopMovement()
        {
            RemoveInput();
            coroutineCaller.StopCoroutine(ref movementCoroutineId);
        }

        private void SetInitPosition()
        {
            float initialX = (screenLimitsX.Limits.Min + screenLimitsX.Limits.Max) / 2;
            float initialZ = (screenLimitsZ.Limits.Min + screenLimitsZ.Limits.Max) / 2;
            transform.position = new Vector3(initialX, transform.position.y, initialZ);
        }

        private void UpdateScreenLimits()
        {
            Vector2 gameBoardRealHalfSize = settings.GameBoardSize / 2;

            screenLimitsX.Limits = new MinMaxTuple<float>
            {
                Min = -gameBoardRealHalfSize.x - screenLimitsX.Margin.Min,
                Max = gameBoardRealHalfSize.x + screenLimitsX.Margin.Max
            };
            screenLimitsZ.Limits = new MinMaxTuple<float>
            {
                Min = -gameBoardRealHalfSize.y - screenLimitsZ.Margin.Min,
                Max = gameBoardRealHalfSize.y + screenLimitsZ.Margin.Max
            };
        }

        private IEnumerator Perform2DMovement()
        {
            while (true)
            {
                if (!Application.isFocused) yield return null;

                Vector3 position = transform.position;
                position += CalculateMovementVectorBorderMode();
                position.x = Mathf.Clamp(position.x, screenLimitsX.Limits.Min, screenLimitsX.Limits.Max);
                position.z = Mathf.Clamp(position.z, screenLimitsZ.Limits.Min, screenLimitsZ.Limits.Max);
                transform.position = position;
            
                yield return null;
            }
        }
        
        private Vector3 CalculateMovementVectorBorderMode()
        {
            Vector3 cursorMovement = Vector3.zero;
            Vector2 cursorPosition = Managers.Instance.InputManager.CursorPosition;

            if (cursorPosition.y >= Screen.height - screenBorderThickness)
            {
                cursorMovement.z += 1;
            }
            else if (cursorPosition.y <= screenBorderThickness)
            {
                cursorMovement.z -= 1;
            }

            if (cursorPosition.x >= Screen.width - screenBorderThickness)
            {
                cursorMovement.x += 1;
            }
            else if (cursorPosition.x <= screenBorderThickness)
            {
                cursorMovement.x -= 1;
            }
                
            return borderModeMovementSpeed * Time.deltaTime * cursorMovement.normalized;
        }
        
        [Serializable]
        private class ScreenLimits
        {
            public MinMaxTuple<float> Limits { get; set; }
            [field: SerializeField] public MinMaxTuple<float> Margin { get; private set; }
        }
        
        [Serializable]
        private class MinMaxTuple<T>
        {
            [field: SerializeField] public T Min { get; set; }
            [field: SerializeField] public T Max { get; set; }
        }
        
        [Serializable]
        private class Zoom
        {
            [SerializeField] private Camera cameraObject;
            [SerializeField] private float zoomTrailLength;
            [SerializeField, Range(0, 1)] private float zoomTargetNormalizedPosition = 0.5f;
            [SerializeField] private float zoomStep = 0.1f;
            [SerializeField] private float zoomSpeed = 1;
            [SerializeField] private Color trailColor = Color.magenta;

            private MinMaxTuple<Vector3> zoomLocalLimits = new();
            private float zoomNormalizedCurrentPosition;
            private CameraController owner;
            private Guid zoomingCoroutineId;
            
            public void Awake(CameraController owner)
            {
                this.owner = owner;
                UpdateTrail();
                zoomNormalizedCurrentPosition = zoomTargetNormalizedPosition;
            }

            public void OnDrawGizmos(Transform transform)
            {
                Gizmos.color = trailColor;
                Gizmos.DrawLine(transform.TransformPoint(zoomLocalLimits.Min), transform.TransformPoint(zoomLocalLimits.Max));
            }
            
            public void OnValidate()
            {
                UpdateTrail();
                UpdateZoom(true);
            }

            public void UpdateZoomTarget(float zoomInput)
            {
                zoomTargetNormalizedPosition = Mathf.Clamp01(zoomTargetNormalizedPosition + zoomInput * zoomStep);
                UpdateZoom();
            }

            private void UpdateTrail()
            {
                float zoomTrailHalfLength = zoomTrailLength / 2;
                zoomLocalLimits.Min = new Vector3(0, 0, -zoomTrailHalfLength);
                zoomLocalLimits.Max = new Vector3(0, 0, zoomTrailHalfLength);
            }

            private void UpdateZoom(bool immediately = false)
            {
                owner?.coroutineCaller?.StopCoroutine(ref zoomingCoroutineId);

                if (immediately)
                {
                    zoomNormalizedCurrentPosition = zoomTargetNormalizedPosition;
                    cameraObject.transform.localPosition = Vector3.Lerp(zoomLocalLimits.Min, zoomLocalLimits.Max, zoomNormalizedCurrentPosition);
                }
                else
                {
                    zoomingCoroutineId = owner.coroutineCaller.StartCoroutine(ZoomingCoroutine(zoomNormalizedCurrentPosition, zoomTargetNormalizedPosition));
                }
            }
            
            private IEnumerator ZoomingCoroutine(float from, float to)
            {
                float deltaTime = 0;
                while (true)
                {
                    deltaTime += Time.deltaTime * zoomSpeed;
                    zoomNormalizedCurrentPosition = Utility.EaseOutCubic(from, to, deltaTime);
            
                    if (Mathf.Approximately(zoomNormalizedCurrentPosition, zoomTargetNormalizedPosition))
                    {
                        zoomNormalizedCurrentPosition = zoomTargetNormalizedPosition;
                        break;
                    }
            
                    cameraObject.transform.localPosition = Vector3.Lerp(zoomLocalLimits.Min, zoomLocalLimits.Max, zoomNormalizedCurrentPosition);
                    yield return null;
                }
            }
        }
    }
}