using _Project.Code.Core.Singleton;
using _Project.Code.Gameplay;
using _Project.Code.Gameplay.Combat;
using _Project.Code.Player;
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
            _actions.Player.Jump.performed += OnJump;
            _actions.Player.Jump.canceled += OnJump;
            _actions.Player.Look.performed += OnLookPerformed;
            _actions.Player.Look.canceled += OnLookCanceled;
            _actions.Player.Shoot.performed += OnShootPerformed;
        }
        void OnDisable()
        {
            _actions.Player.Move.performed -= OnMovePerformed;
            _actions.Player.Move.canceled -= OnMoveCanceled;
            _actions.Player.Jump.performed -= OnJump;
            _actions.Player.Jump.canceled -= OnJump;
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
            _playerController.GetMoveInput(ctx.ReadValue<Vector2>());
        }

        void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            _playerController.GetMoveInput(Vector2.zero);
        }

        void OnLookPerformed(InputAction.CallbackContext ctx)
        {
            _playerController.GetLookInput(ctx.ReadValue<Vector2>());
        }

        void OnLookCanceled(InputAction.CallbackContext ctx)
        {
            _playerController.GetLookInput(Vector2.zero);
        }
        
        void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _playerController.OnJump(true);
            }

            if (ctx.canceled)
            {
                _playerController.OnJump(false);
            }
        }
    }
}
