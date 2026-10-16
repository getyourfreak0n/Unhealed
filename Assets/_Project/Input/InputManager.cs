using _Project.Code.Core.Singleton;
using _Project.Code.Gameplay;
using _Project.Code.Gameplay.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Input
{
    public class InputManager : SingletonBase<InputManager>
    {
        NewInputActions _actions;
        PlayerController _playerController;
        PlayerShooter _playerShooter;

        protected override void Awake()
        {
            base.Awake();
            _actions = new NewInputActions();
            _playerController = FindAnyObjectByType<PlayerController>();
            _playerShooter = FindAnyObjectByType<PlayerShooter>();
        }

        void OnEnable()
        {
            _actions.Enable();
            _actions.Player.Move.performed += OnMovePerformed;
            _actions.Player.Move.canceled += OnMoveCanceled;
            _actions.Player.Jump.performed += OnJumpPerformed;
            _actions.Player.Look.performed += OnLookPerformed;
            _actions.Player.Look.canceled += OnLookCanceled;
            _actions.Player.Shoot.performed += OnShootPerformed;
        }
        void OnDisable()
        {
            _actions.Player.Move.performed -= OnMovePerformed;
            _actions.Player.Move.canceled -= OnMoveCanceled;
            _actions.Player.Jump.performed -= OnJumpPerformed;
            _actions.Player.Look.performed -= OnLookPerformed;
            _actions.Player.Look.canceled -= OnLookCanceled;
            _actions.Player.Shoot.performed -= OnShootPerformed;
            _actions.Disable();
        }

        void OnShootPerformed(InputAction.CallbackContext obj)
        {
            _playerShooter.Shoot();
        }

        void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            _playerController.MoveInput(ctx.ReadValue<Vector2>());
        }

        void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            _playerController.MoveInput(Vector2.zero);
        }

        void OnLookPerformed(InputAction.CallbackContext ctx)
        {
            _playerController.LookInput(ctx.ReadValue<Vector2>());
        }

        void OnLookCanceled(InputAction.CallbackContext ctx)
        {
            _playerController.LookInput(Vector2.zero);
        }
        
        void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            _playerController.OnJump();
        }
        
        
    }
}
