using UnityEngine;

public class DeathUI : MonoBehaviour
{
    public void Respawn()
    {
        if (Player.Instance != null)
        {
            if (GameManager.Instance.DeathCanvas != null)
            {
                GameManager.Instance.DeathCanvas.SetActive(false);
                Player.Instance.Respawn();
            }
        }
    }

    public void QuitToMainMenu()
    {
        MusicManager.Instance.PlayTrack(MusicManager.MusicTrack.Title);
        GameManager.Instance.ResetEverything();
        SceneSystem.Instance.LoadScene((int)SceneSystem.SceneIndex.StartScreen, 0f, 0f);
    }
}
