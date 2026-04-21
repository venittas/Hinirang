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
        if (Player.Instance.currentState == Player.PlayerState.Interacting) return;
        Instantiate(PauseUI);
        Time.timeScale = 0f;
    }

    public void ToMainMenu()
    {
        SceneSystem.Instance.LoadScene(0, 0f, 0f);
        Time.timeScale = 1f;
        Player.Instance.playerExitX = Player.Instance.transform.position.x; 
        Player.Instance.playerExitY = Player.Instance.transform.position.y;
        MangJuan.Instance.DisableBoat();
    }
}
