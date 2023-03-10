using Services;
using UnityEngine.SceneManagement;

public class LevelSceneManager : IService
{
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