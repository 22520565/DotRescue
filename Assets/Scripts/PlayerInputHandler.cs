#nullable enable

namespace Game
{
    using UnityEngine;

    public sealed class PlayerInputHandler : MonoBehaviour, System.IDisposable
    {
        private InputSystemAction inputSystemAction = null!;

        public bool ChangeDirectionPressed { get; private set; } = false;

        public void CancelChangeDirectionInputAction() => this.ChangeDirectionPressed = false;

        public void Dispose()
        {
            this.inputSystemAction?.Dispose();
        }

        private void Awake()
        {
            this.inputSystemAction = new InputSystemAction();
            var playerActions = this.inputSystemAction.Player;

            playerActions.ChangeDirection.performed += context => this.ChangeDirectionPressed = true;
            playerActions.ChangeDirection.canceled += context => this.ChangeDirectionPressed = false;

            this.inputSystemAction.Enable();
        }

        private void OnEnable()
        {
            this.inputSystemAction!.Enable();
        }

        private void OnDisable()
        {
            this.inputSystemAction!.Disable();
        }

        private void OnDestroy()
        {
            this.inputSystemAction!.Dispose();
        }
    }
}
