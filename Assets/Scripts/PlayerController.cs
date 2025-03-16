#nullable enable

namespace Game
{
    using UnityEngine;

    [RequireComponent(typeof(PlayerInputHandler))]
    public sealed class PlayerController : MonoBehaviour
    {
        [field: SerializeReference]
        public AudioClip MoveClip { get; private set; } = null!;

        [field: SerializeReference]
        public AudioClip LoseClip { get; private set; } = null!;

        [field: SerializeReference]
        [field: ResolveComponentInParent("Gameplay")]
        public GameplayController GameplayController { get; private set; } = null!;

        public ObstacleController ObstacleController => this.GameplayController.ObstacleController;

        [field: SerializeReference]
        [field: ResolveComponentInChildren("Sprite")]
        public SpriteRenderer SpriteRenderer { get; private set; } = null!;

        [field: SerializeReference]
        [field: ResolveComponentInChildren("Sprite")]
        public ParticleSystem ParticleSystem { get; private set; } = null!;

        [field: SerializeField]
        [field: ResolveComponent]
        public PlayerInputHandler PlayerInputHandler { get; private set; } = null!;

        [field: SerializeReference]
        public GameObject ExplosivePrefab { get; private set; } = null!;

        [field: SerializeField]
        [field: LayerMaskIsNothingOrEverythingWarning]
        public LayerMask ObstacleLayerMask { get; private set; } = new LayerMask();

        public float BaseRotationSpeed => this.ObstacleController.RotationSpeed;

        [field: SerializeField]
        [field: Range(1.0F, 3.0F)]
        public float RotationSpeedToObstacleRatio { get; private set; } = 1.2F;

        [field: SerializeField]
        public bool IsRotatingClockwise { get; private set; } = true;

        public float CurrentRotationSpeed { get; private set; } = 2.0F;

        private void Awake()
        {
            this.CurrentRotationSpeed = this.ObstacleController.RotationSpeed * this.RotationSpeedToObstacleRatio;
            if (this.IsRotatingClockwise)
            {
                this.CurrentRotationSpeed = -this.CurrentRotationSpeed;
            }
        }

        private void Update()
        {
            if (this.GameplayController.HasGameFinished)
            {
                return;
            }

            var playerInputHandler = this.PlayerInputHandler;
            if (playerInputHandler.ChangeDirectionPressed)
            {
                playerInputHandler.CancelChangeDirectionInputAction();
                this.IsRotatingClockwise = !this.IsRotatingClockwise;
                SoundManager.Instance.PlaySound(this.MoveClip);
            }

            this.CurrentRotationSpeed = this.ObstacleController.RotationSpeed * this.RotationSpeedToObstacleRatio;
            if (this.IsRotatingClockwise)
            {
                this.CurrentRotationSpeed = -this.CurrentRotationSpeed;
            }
        }

        private void FixedUpdate()
        {
            if (this.GameplayController.HasGameFinished)
            {
                return;
            }

            this.transform.Rotate(0, 0, this.CurrentRotationSpeed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var layerIndex = collision.gameObject.layer;
            LayerMask collisionLayerMask = 1 << layerIndex;
            if (collisionLayerMask != this.ObstacleLayerMask)
            {
                return;
            }

            SoundManager.Instance.PlaySound(this.LoseClip);
            this.GameplayController.EndGame();
            Object.Instantiate(this.ExplosivePrefab, this.SpriteRenderer.transform);
            this.SpriteRenderer.enabled = false;
            var mainModule = this.ParticleSystem.main;
            mainModule.loop = false;
        }
    }
}
