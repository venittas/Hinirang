using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public SceneIndex currentPlayerLocation = SceneIndex.Island;
    public static SceneSystem Instance;
    public enum SceneIndex
    {
        StartScreen = 0,
        Island = 1,
        PlayerHouse = 2,
        PlayerRoom = 3,
        Village = 4,
        Mountain = 5,
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(int sceneIndex, float x, float y)
    {
        if (sceneIndex == (int)SceneIndex.StartScreen)
        {
            LoadScene(sceneIndex);
            return;
        }
        Debug.Log($"Loading scene {sceneIndex} with player position ({x}, {y})");
        GameManager.Instance.TransitionToScene(sceneIndex, x, y);
        currentPlayerLocation = (SceneIndex)sceneIndex;
        Invoke("CheckNPCs", 1f);
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        if (vcam != null)
        {
            vcam.Follow = Player.Instance.transform;
        }
    }
    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex == (int)SceneIndex.StartScreen)
        {
            SceneManager.LoadScene(sceneIndex);
            return;
        }
        currentPlayerLocation = (SceneIndex)sceneIndex;
        SceneManager.LoadScene(sceneIndex);
        StartCoroutine(AssignCinemachineAfterLoad());
    }

    private IEnumerator AssignCinemachineAfterLoad()
    {
        yield return null; 
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        if (vcam != null)
        {
            vcam.Follow = Player.Instance.transform;
        }
        CheckNPCs();
    }


    public void CheckNPCs()
    {
        if (currentPlayerLocation != SceneSystem.SceneIndex.Island)
        {
            if (AlingNena.Instance != null) AlingNena.Instance.gameObject.SetActive(false);
            if (MangJuan.Instance != null) MangJuan.Instance.gameObject.SetActive(false);
            if (Joba.Instance != null) Joba.Instance.gameObject.SetActive(false);
            if (MangJuan.Instance != null) MangJuan.Instance.DisableBoat();
        }
        else
        {
            if (AlingNena.Instance != null) AlingNena.Instance.gameObject.SetActive(true);
            if (Joba.Instance != null) Joba.Instance.gameObject.SetActive(true);

        }
        if (currentPlayerLocation != SceneSystem.SceneIndex.Village)
        {
            if (MangEnko.Instance != null) MangEnko.Instance.gameObject.SetActive(false);
        }
        else
        {
            if (MangEnko.Instance != null) MangEnko.Instance.gameObject.SetActive(true);
        }
        if (Player.Instance.eventNameTrigger != "Day7")
        {
            if (Manananggal.Instance != null) Manananggal.Instance.gameObject.SetActive(false);
        }
        else
        {
            if (Manananggal.Instance != null) Manananggal.Instance.gameObject.SetActive(true);
        }
        Debug.Log(Player.Instance.eventNameTrigger);
    }


}
