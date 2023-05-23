using System;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class LevelSceneManager : IService
{
    public event Action LevelLoaded;
    private readonly LevelsSettings _levelSettings = Resources.Load<LevelsSettings>("LevelSettings");
    private int _nextLevel = 0;
    private Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();
    public void ResetLevelCount()
    {
        _nextLevel = 0;
    }
    
    public void LoadNextLevel()
    {
        if (_nextLevel < _levelSettings.levels.Count)
        {
            ServiceLocator.Instance.Get<PlayerManager>().SetPlayerLocation(Vector3.zero);
            //listen for scene loaded so we can do stuff after we load a clean scene
            SceneManager.sceneLoaded += GenerateLevelAfterSceneLoaded;
            //Clear out the tilemaps variable since we are about to load a new scene
            _tilemaps.Clear();
            //load a clean scene to generate a level in
            LoadLevelGenerationScene();
        }
        else
        {
            //if there are no levels left, the player has won the game
            ServiceLocator.Instance.Get<ApplicationStateManager>().PushState<GameWonState>(true);
        }

    }

    public Dictionary<string, Tilemap> GetTilemapDictionary()
    {
        return _tilemaps;
    }
    private void GenerateLevelAfterSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //start spawning the level
        _levelSettings.levels[_nextLevel].SpawnLevel();
        //increment the index so we spawn a new level next time
        _nextLevel++;
        
        //set the global tilemap to the new tilemap available here
        AddTilemapsInSceneToDictionary();

        SceneManager.sceneLoaded -= GenerateLevelAfterSceneLoaded;
        LevelLoaded?.Invoke();
    }

    private void AddTilemapsInSceneToDictionary()
    {
        //store all the tilemaps in the scene
        Tilemap[] tilemapsInScene = Object.FindObjectsOfType<Tilemap>();
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            _tilemaps.Add(tilemap.name, tilemap);
        }
    }

    private void LoadLevelGenerationScene()
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