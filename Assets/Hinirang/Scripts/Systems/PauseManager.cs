using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseUI;

    public void ResumeGame()
    {
        Destroy(PauseUI);
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        Instantiate(PauseUI);
        Time.timeScale = 0f;
    }

    public void ToMainMenu()
    {
        SceneSystem.LoadScene(0, 0f, 0f);
        Time.timeScale = 1f;
    }
}
