using System.Collections.Generic;
using Constants;
using StateManager;
using UnityEngine;

namespace Services
{
    public class GameState : BaseApplicationState
    {
        public readonly string UI_PREFAB = UIPrefabs.UIGame;
        public readonly string STATS_UI_PREFAB = UIPrefabs.UIStats;

        //public readonly int SCENE_INDEX = (int)SceneIndexes.FIRST_SCENE_NAME;
        //private AudioClip _mainMenuMusic = Resources.Load<AudioClip>("MainMenuMusic");
        private UIWidget _uiWidget;

        // stats overlay - TODO: toggle on/off via an input or settings menu option
        private UIWidget _statsWidget;

        public GameState()
        {

        }

        protected override void SetToActiveStateFromBackground(BaseState prevState, Dictionary<string, object> options)
        {
            if (_uiWidget != null)
            {
                _uiWidget.UIObject.SetActive(true);
            }

            if (_statsWidget != null)
            {
                _statsWidget.UIObject.SetActive(true);
            }
            
            Time.timeScale = 1f;
        }

        protected override void SetToBackgroundStateFromActive(BaseState prevState, Dictionary<string, object> options)
        {
            if (_uiWidget != null)
            {
                _uiWidget.UIObject.SetActive(false);
            }

            if (_statsWidget != null)
            {
                _statsWidget.UIObject.SetActive(false);
            }

        }

        protected override void SetupState(BaseState prevState, Dictionary<string, object> options)
        {
            ServiceLocator.Instance.Get<LevelSceneManager>().LevelLoaded += FinishStateSetup;
#if UNITY_EDITOR
            //don't load the next level if we're testing out a level in the editor
            // and we didn't launch the gamestate from the initial/menu scene (which should be index 0)
            int currentLevelIndex = ServiceLocator.Instance.Get<LevelSceneManager>().GetLevelIndex();
            if (currentLevelIndex == 0)
            {
                ServiceLocator.Instance.Get<LevelSceneManager>().LoadNextLevel();
            }
#else
            ServiceLocator.Instance.Get<LevelSceneManager>().LoadNextLevel();
#endif
            ServiceLocator.Instance.Get<PlayerManager>().SpawnPlayer(Vector3.zero);
        }

        private void FinishStateSetup()
        {
            _uiWidget = ServiceLocator.Instance.Get<UIManager>().LoadUI(UI_PREFAB);
            _statsWidget = ServiceLocator.Instance.Get<UIManager>().LoadUI(STATS_UI_PREFAB);
            ServiceLocator.Instance.Get<LevelSceneManager>().LevelLoaded -= FinishStateSetup;
        }

        protected override void SetupStateInBackground(BaseState prevState, Dictionary<string, object> options)
        {
            SetupState(prevState, options);
            SetToBackgroundStateFromActive(prevState, options);
        }

        protected override void TeardownState(BaseState prevState, Dictionary<string, object> options)
        {
            ServiceLocator.Instance.Get<LevelSceneManager>().ResetLevelCount();
            ServiceLocator.Instance.Get<PlayerManager>().Reset();
            if (_uiWidget != null)
            {
                ServiceLocator.Instance.Get<UIManager>().RemoveUIByGuid(_uiWidget.GUID);
            }

            if (_statsWidget != null)
            {
                ServiceLocator.Instance.Get<UIManager>().RemoveUIByGuid(_statsWidget.GUID);
            }
        }
    }
}
