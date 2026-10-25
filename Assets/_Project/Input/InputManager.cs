using _Project.Code.Core.Singleton;
using _Project.Code.Gameplay.Combat;
using _Project.Code.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Input
{
    public class InputManager : SingletonBase<InputManager>
    {
        NewInputActions _actions;
        PlayerController _player;
        PlayerShooter _playerShooter;

        protected override void Awake()
        {
            base.Awake();
            _actions = new NewInputActions();
            _player = FindAnyObjectByType<PlayerController>();
            _playerShooter = FindAnyObjectByType<PlayerShooter>();
        }

        void OnEnable()
        {
            _actions.Enable();
            _actions.Player.Move.performed += OnMovePerformed;
            _actions.Player.Move.canceled += OnMoveCanceled;
            // _actions.Player.Jump.performed += OnJump;
            // _actions.Player.Jump.canceled += OnJump;
            _actions.Player.Dash.performed += OnDash;
            _actions.Player.Dash.canceled += OnDash;
            _actions.Player.Look.performed += OnLookPerformed;
            _actions.Player.Look.canceled += OnLookCanceled;
            _actions.Player.Shoot.performed += OnShootPerformed;
        }
        void OnDisable()
        {
            _actions.Player.Move.performed -= OnMovePerformed;
            _actions.Player.Move.canceled -= OnMoveCanceled;
            //_actions.Player.Jump.performed -= OnJump;
            //_actions.Player.Jump.canceled -= OnJump;
            _actions.Player.Dash.performed -= OnDash;
            _actions.Player.Dash.canceled -= OnDash;
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
            _player.GetMoveInput(ctx.ReadValue<Vector2>());
        }

        void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            _player.GetMoveInput(Vector2.zero);
        }

        void OnLookPerformed(InputAction.CallbackContext ctx)
        {
            _player.GetLookInput(ctx.ReadValue<Vector2>());
        }

        void OnLookCanceled(InputAction.CallbackContext ctx)
        {
            _player.GetLookInput(Vector2.zero);
        }

        void OnDash(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _player.OnDash(true);            
            }

            if (ctx.canceled)
            {
                _player.OnDash(false);
            }
        }
        
        /*void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _playerController.OnJump(true);
            }

            if (ctx.canceled)
            {
                _playerController.OnJump(false);
            }
        }*/
    }
}
