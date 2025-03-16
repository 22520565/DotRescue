#nullable enable

namespace Game
{
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public sealed class GameplayController : MonoBehaviour
    {
        [field: SerializeReference]
        [field: ResolveComponentInChildren("ScoreText")]
        private TMP_Text scoreText = null!;

        [field: Header("Score info")]

        [field: SerializeField]
        [field: Range(1.0F, 50.0F)]
        public float BaseScoreSpeed { get; private set; } = 1.0F;

        [field: SerializeField]
        [field: Range(0.1F, 5.0F)]
        public float ScoreAcceleration { get; private set; } = 0.0F;

        [field: SerializeField]
        [field: ReadOnlyInInspector]
        public float CurrentScoreSpeed { get; private set; } = 0.0F;

        public float CurrentScore { get; private set; } = 0.0F;

        [field: Header("Level info")]

        [field: SerializeField]
        [field: Range(1.0F, 50.0F)]
        public float BaseLevelSpeed { get; private set; } = 1.0F;

        [field: SerializeField]
        [field: Range(0.1F, 5.0F)]
        public float LevelAcceleration { get; private set; } = 1.0F;

        [field: SerializeField]
        [field: ReadOnlyInInspector]
        public float CurrentLevelSpeed { get; private set; } = 1.0F;

        public bool HasGameFinished { get; private set; } = false;

        public void EndGame()
        {
            this.HasGameFinished = true;
            GameManager.Instance.CurrentScore = Mathf.Min(Mathf.RoundToInt(this.CurrentScore), int.MaxValue);
            this.StartCoroutine(GameplayController.BackToMainMenu());
        }

        private static IEnumerator BackToMainMenu()
        {
            yield return new WaitForSeconds(2.0F);
            GameManager.Instance.GoToMainMenu();
        }

        private void Awake()
        {
            this.CurrentScoreSpeed = this.BaseScoreSpeed;
            this.CurrentLevelSpeed = this.BaseLevelSpeed;
        }

        private void Update()
        {
            if (this.HasGameFinished)
            {
                return;
            }

            var deltaTime = Time.deltaTime;

            this.CurrentScore += this.CurrentScoreSpeed * deltaTime;
            this.scoreText.text = Mathf.Min(Mathf.RoundToInt(this.CurrentScore), int.MaxValue).ToString();

            this.CurrentScoreSpeed += this.ScoreAcceleration * deltaTime;
            this.CurrentLevelSpeed += this.LevelAcceleration * deltaTime;
        }
    }
}
