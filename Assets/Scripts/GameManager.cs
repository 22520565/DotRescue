#nullable enable

namespace Game
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public string MainMenuSceneName { get; } = "MainMenu";

        public string GameplaySceneName { get; } = "Gameplay";

        public string HighScorePrefKey { get; } = "HighScore";

        public int CurrentScore { get; set; } = 0;

        public int HighScore
        {
            get
            {
                return PlayerPrefs.GetInt(this.HighScorePrefKey, 0);
            }

            set
            {
                PlayerPrefs.SetInt(this.HighScorePrefKey, value);
            }
        }

        public bool IsPlayed { get; private set; } = false;

        protected override GameManager LocalInstance => this;

        public void GoToMainMenu()
        {
            this.IsPlayed = true;
            SceneManager.LoadScene(this.MainMenuSceneName);
        }

        public void GoToGameplay()
        {
            SceneManager.LoadScene(this.GameplaySceneName);
        }
    }
}
