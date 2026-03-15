using System;
using System.Collections.Generic;
using Risk.Runtime.BackendCommunication;
using Risk.Runtime.GameBoard;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Risk.Runtime.Input
{
    public class BoardInputManager : MonoBehaviour
    {
        [Header("Board Input Settings")]
        [SerializeField] private LayerMask _boardLayerMask = 1 << 6;
        [SerializeField] private float _minOrthoSize = 3f;
        [SerializeField] private float _maxOrthoSize = 6f;
        [SerializeField] private float _zoomSpeed = 15f;
        [SerializeField] private float _panningSpeed = 2f;
        
        private BoardInputActions _inputActions;
        private Camera _mainCamera;
        
        public event Action<BoardTerritory> BoardTerritoryClicked;
        public event Action<BoardTerritory> BoardTerritoryHovered;
        public event Action BoardTerritoryHoverExited;
        
        
        #region MonoBehaviour
            
        private void Awake()
        {
            _inputActions = new BoardInputActions();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Board.Click.performed += OnBoardClick;
            
            _inputActions.Board.Zoom.performed += OnZoom;
            _inputActions.Board.Zoom.canceled += OnZoom;
            
            _inputActions.Board.Pan.performed += OnPanning;
            _inputActions.Board.Pan.canceled += OnPanning;
        }
        
        private void OnDisable()
        {
            _inputActions.Board.Click.performed -= OnBoardClick;
            
            _inputActions.Board.Zoom.performed -= OnZoom;
            _inputActions.Board.Zoom.canceled -= OnZoom;
                
            _inputActions.Board.Pan.performed -= OnPanning;
            _inputActions.Board.Pan.canceled -= OnPanning;
            _inputActions.Disable();
        }

        private void Update()
        {
            Vector2 screenPos = _inputActions.Board.Pointer.ReadValue<Vector2>();
            Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            
            Collider2D hit = Physics2D.OverlapPoint(worldPos, _boardLayerMask);

            if (hit == null)
            {
                BoardTerritoryHoverExited?.Invoke();    
                return;
            }
            BoardTerritory hitTerritory = hit.transform.parent.gameObject.GetComponent<BoardTerritory>();
            BoardTerritoryHovered?.Invoke(hitTerritory);
        }
        #endregion
        
        private void OnBoardClick(InputAction.CallbackContext ctx)
        {
            Vector2 screenPos = _inputActions.Board.Pointer.ReadValue<Vector2>();
            Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            
            Collider2D hit = Physics2D.OverlapPoint(worldPos, _boardLayerMask);
            if (hit != null)
            {
                BoardTerritory clickedBoardTerritory = hit.transform.parent.gameObject.GetComponent<BoardTerritory>();
                BoardTerritoryClicked?.Invoke(clickedBoardTerritory);
                Debug.Log($"Clicked territory: {hit.transform.parent.name}", hit.gameObject);
            }
            else
            {
                Debug.Log("Clicked nothing");
            }   
        }
        
        private void OnZoom(InputAction.CallbackContext ctx)
        {
            float scrollDelta = _inputActions.Board.Zoom.ReadValue<float>();
            float currentSize = _mainCamera.orthographicSize;
            
            // Zoom towards the mouse position
            Vector2 mousePos = _inputActions.Board.Pointer.ReadValue<Vector2>();
            Vector3 mouseWorldBefore = _mainCamera.ScreenToWorldPoint(mousePos);
            
            // Apply zoom (negative scroll = zoom in)
            currentSize -= scrollDelta * _zoomSpeed * Time.deltaTime;
            currentSize = Mathf.Clamp(currentSize, _minOrthoSize, _maxOrthoSize);
            _mainCamera.orthographicSize = currentSize;
            
            // Move camera to keep mouse world position centered
            Vector3 mouseWorldAfter = _mainCamera.ScreenToWorldPoint(mousePos);
            Vector3 offset = mouseWorldBefore - mouseWorldAfter;
            _mainCamera.transform.position += offset;
            
            Debug.Log($"Zoom: {scrollDelta}, Size: {currentSize}, Offset: {offset}");
        }
        
        private void OnPanning(InputAction.CallbackContext ctx)
        {
            if (_mainCamera.orthographicSize >= _maxOrthoSize) return; // No pan when fully zoomed out
    
            Vector2 mouseDelta = _inputActions.Board.Pan.ReadValue<Vector2>();
    
            // Convert screen delta to world space (zoom-aware)
            float aspect = _mainCamera.aspect;
            Vector3 worldDelta = new Vector3(
                mouseDelta.x * _mainCamera.orthographicSize / _mainCamera.pixelHeight * aspect,
                mouseDelta.y * _mainCamera.orthographicSize / _mainCamera.pixelHeight,
                0
            ) * _panningSpeed;
    
            // Apply movement
            Vector3 newPos = _mainCamera.transform.position + worldDelta;
    
            // CLAMP to max zoomed-out bounds
            float maxHalfWidth = _maxOrthoSize * aspect;
            float maxHalfHeight = _maxOrthoSize;
    
            // Current camera size relative to max size
            float currentHalfWidth = _mainCamera.orthographicSize * aspect;
            float currentHalfHeight = _mainCamera.orthographicSize;
    
            newPos.x = Mathf.Clamp(newPos.x, -maxHalfWidth + currentHalfWidth, maxHalfWidth - currentHalfWidth);
            newPos.y = Mathf.Clamp(newPos.y, -maxHalfHeight + currentHalfHeight, maxHalfHeight - currentHalfHeight);
    
            _mainCamera.transform.position = newPos;
        }


        
        
    }
}