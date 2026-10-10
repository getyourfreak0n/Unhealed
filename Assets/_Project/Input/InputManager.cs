using _Project.Code.Core.Singleton;
using _Project.Code.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Input
{
    public class InputManager : SingletonBase<InputManager>
    {
        NewInputActions _actions;
        PlayerController _playerController;

        protected override void Awake()
        {
            base.Awake();
            _actions = new NewInputActions();
            _playerController = FindAnyObjectByType<PlayerController>();
        }

        void OnEnable()
        {
            _actions.Enable();
            _actions.Player.Move.performed += OnMovePerformed;
            _actions.Player.Move.canceled += OnMoveCanceled;


        }
        void OnDisable()
        {
            _actions.Player.Move.performed -= OnMovePerformed;
            _actions.Player.Move.canceled -= OnMoveCanceled;
            _actions.Disable();
        }

        void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            _playerController.MoveInput(ctx.ReadValue<Vector2>());
        }

        void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            _playerController.MoveInput(Vector2.zero);
        }
    }
}
