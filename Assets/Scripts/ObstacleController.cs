#nullable enable

namespace Game
{
    using UnityEngine;

    public sealed class ObstacleController : MonoBehaviour
    {
        [field: Header("Gameplay controller")]

        [field: SerializeField]
        [field: ResolveComponentInParent("Gameplay")]
        private GameplayController gameplayController = null!;

        [field: Header("Speed info")]

        [field: SerializeField]
        [field: Range(0.1F, 1.0F)]
        public float MinBaseRotationSpeed { get; private set; } = 2.0F;

        [field: SerializeField]
        [field: Range(1.0F, 10.0F)]
        public float MaxBaseRotationSpeed { get; private set; } = 5.0F;

        [field: SerializeField]
        [field: ReadOnlyInInspector]
        public float BaseRotationSpeed { get; private set; } = 2.0F;

        [field: SerializeField]
        [field: Range(0.1F, 1.0F)]
        public float RotatonSpeedScaleToLevelSpeed { get; private set; } = 0.05F;

        [field: SerializeField]
        [field: Range(1.0F, 20.0F)]
        public float MinRotationSpeed { get; private set; } = 2.0F;

        [field: SerializeField]
        [field: Range(100.0F, 1000.0F)]
        public float MaxRotationSpeed { get; private set; } = 500.0F;

        [field: SerializeField]
        [field: ReadOnlyInInspector]
        public float CurrentRotationSpeed { get; private set; } = 2.0F;

        [field: Header("Time info")]

        [field: SerializeField]
        [field: Range(0.1F, 2.0F)]
        public float MinRotationTime { get; private set; } = 0.5F;

        [field: SerializeField]
        [field: Range(2.0F, 10.0F)]
        public float MaxRotationTime { get; private set; } = 5.0F;

        public float CurrentRotationTimer { get; private set; }

        [field: Header("Direction info")]

        [field: SerializeField]
        public bool IsRotatingClockwise { get; private set; } = true;

        private void Awake()
        {
            this.CurrentRotationTimer = Random.Range(this.MinRotationTime, this.MaxRotationTime);
            this.BaseRotationSpeed = Random.Range(this.MinBaseRotationSpeed, this.MaxBaseRotationSpeed);
            this.CurrentRotationSpeed = this.BaseRotationSpeed * this.gameplayController.CurrentLevelSpeed * this.RotatonSpeedScaleToLevelSpeed;
            this.CurrentRotationSpeed = Mathf.Clamp(this.CurrentRotationSpeed, this.MinRotationSpeed, this.MaxRotationSpeed);
            if (this.IsRotatingClockwise)
            {
                this.CurrentRotationSpeed = -this.CurrentRotationSpeed;
            }
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            if (this.CurrentRotationTimer > deltaTime)
            {
                this.CurrentRotationTimer -= deltaTime;
            }
            else
            {
                this.IsRotatingClockwise = !this.IsRotatingClockwise;
                this.CurrentRotationTimer = Random.Range(this.MinRotationTime, this.MaxRotationTime);
                this.BaseRotationSpeed = Random.Range(this.MinBaseRotationSpeed, this.MaxBaseRotationSpeed);
            }

            this.CurrentRotationSpeed = this.BaseRotationSpeed * this.gameplayController.CurrentLevelSpeed * this.RotatonSpeedScaleToLevelSpeed;
            this.CurrentRotationSpeed = Mathf.Clamp(this.CurrentRotationSpeed, this.MinRotationSpeed, this.MaxRotationSpeed);
            if (this.IsRotatingClockwise)
            {
                this.CurrentRotationSpeed = -this.CurrentRotationSpeed;
            }
        }

        private void FixedUpdate()
        {
            if (this.gameplayController.HasGameFinished)
            {
                return;
            }

            this.transform.Rotate(0, 0, this.CurrentRotationSpeed * Time.fixedDeltaTime);
        }
    }
}
