using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public enum SceneIndex
    {
        StartScreen = 0,
        Island = 1,
        TestScene = 2,
        PlayerHouse = 3,
        PlayerRoom = 4,
    }
    public static void LoadScene(int sceneIndex, float x, float y)
    {
        Debug.Log($"Loading scene {sceneIndex} with player position ({x}, {y})");
        GameManager.Instance.TransitionToScene(sceneIndex, x, y);
    }

}
