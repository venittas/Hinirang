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
        SceneSystem.LoadScene((int)SceneSystem.SceneIndex.StartScreen);
    }
}
