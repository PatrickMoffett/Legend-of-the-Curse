using System;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : IService
{
    public event Action LevelLoaded;
    private readonly LevelsSettings _levelSettings = Resources.Load<LevelsSettings>("LevelSettings");
    private int _nextLevel = 0;
    public void ResetLevelCount()
    {
        _nextLevel = 0;
    }
    
    public void LoadNextLevel()
    {
        //clear out the current scene
        SceneManager.sceneLoaded += GenerateLevelAfterSceneLoaded;
        LoadLevelGenerationScene();


    }
    private void GenerateLevelAfterSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (_nextLevel < _levelSettings.levels.Count)
        {
            //while levels left, spawn the next one
            _levelSettings.levels[_nextLevel].SpawnLevel();
            _nextLevel++;
        }
        else
        {
            //if there are no levels left, the player has won the game
            ServiceLocator.Instance.Get<ApplicationStateManager>().PushState<GameWonState>(true);
        }
        SceneManager.sceneLoaded -= GenerateLevelAfterSceneLoaded;
        LevelLoaded?.Invoke();
    }
    public void LoadLevelGenerationScene()
    {
        SceneManager.LoadScene("LevelGenerationScene");
    }
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    public string GetLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public int GetLevelIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}