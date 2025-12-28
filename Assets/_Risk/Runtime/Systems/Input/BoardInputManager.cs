using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Risk.Runtime.Input
{
    public class BoardInputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _boardLayerMask = 1 << 6;
        private BoardInputActions _inputActions;
        private Camera _mainCamera;

        private void Awake()
        {
            _inputActions = new BoardInputActions();
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.Board.Click.performed += OnBoardClick;
        }
        
        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.Board.Click.performed -= OnBoardClick;

        }

        private void OnBoardClick(InputAction.CallbackContext ctx)
        {
            Vector2 screenPos = _inputActions.Board.Pointer.ReadValue<Vector2>();
            Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            
            Collider2D hit = Physics2D.OverlapPoint(worldPos, _boardLayerMask);
            if (hit != null)
            {
                Debug.Log($"Clicked territory: {hit.transform.parent.name}", hit.gameObject);
                // TODO: Call territory.OnClick()
            }
            else
            {
                Debug.Log("Clicked nothing");
            }
        }
        
    }
}