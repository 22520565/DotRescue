#nullable enable

namespace Game
{
    using System.Collections;
    using TMPro;
    using UnityEngine;

    public sealed class MainMenuController : MonoBehaviour
    {
        [field: Header("Text")]

        [field: SerializeReference]
        [field: ResolveComponentInChildren("ScoreText")]
        private TMP_Text scoreText = null!;

        [field: SerializeReference]
        [field: ResolveComponentInChildren("NewBestText")]
        private TMP_Text newBestText = null!;

        [field: SerializeReference]
        [field: ResolveComponentInChildren("HighScoreText")]
        private TMP_Text highScoreText = null!;

        [field: Header("Aduio clip")]

        [field: SerializeReference]
        private AudioClip clickAudioClip = null!;

        [field: Header("Score animation")]

        [field: SerializeField]
        private AnimationCurve speedCurve = null!;

        [field: SerializeField]
        [field: Range(0.5F, 3.0F)]
        public float AnimationTime { get; private set; } = 1.0F;

        public void OnPlayButtonClicked()
        {
            SoundManager.Instance.PlaySound(this.clickAudioClip);
            GameManager.Instance.GoToGameplay();
        }

        private IEnumerator? ShowScore()
        {
            var gameManager = GameManager.Instance;
            var currentScore = gameManager.CurrentScore;
            var highScore = gameManager.HighScore;
            if (currentScore > highScore)
            {
                this.newBestText.enabled = true;
                gameManager.HighScore = currentScore;
                highScore = currentScore;
            }
            else
            {
                this.newBestText.enabled = false;
            }
            this.scoreText.text = "0";
            this.highScoreText.text = highScore.ToString();

            int tempScore = 0;
            var animationSpeed = 1.0F / this.AnimationTime;
            var timeElasped = 0.0F;
            while (timeElasped < this.AnimationTime)
            {
                timeElasped += animationSpeed * Time.deltaTime;

                tempScore = Mathf.RoundToInt(this.speedCurve.Evaluate(timeElasped * animationSpeed) * currentScore);
                this.scoreText.text = tempScore.ToString();
                yield return null;
            }
            this.scoreText.text = currentScore.ToString();
        }

        private void Awake()
        {
            var gameManager = GameManager.Instance;
            if (gameManager.IsPlayed)
            {
                this.StartCoroutine(this.ShowScore());
            }
            else
            {
                this.scoreText.enabled = false;
                this.newBestText.enabled = false;
                this.highScoreText.text = gameManager.HighScore.ToString();
            }
        }
    }
}
