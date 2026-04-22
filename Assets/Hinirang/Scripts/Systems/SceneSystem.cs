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
        bool onIsland = currentPlayerLocation == SceneIndex.Island;
        if (AlingNena.Instance != null) AlingNena.Instance.gameObject.SetActive(onIsland);
        if (MangJuan.Instance != null) MangJuan.Instance.gameObject.SetActive(onIsland);
        if (MangJuan.Instance != null) MangJuan.Instance.DisableBoat();

        bool onVillage = currentPlayerLocation == SceneIndex.Village;
        if (MangEnko.Instance != null) MangEnko.Instance.gameObject.SetActive(onVillage);

        bool onMountain = currentPlayerLocation == SceneIndex.Mountain;
        if (Joba.Instance != null)
        {
            Joba.Instance.gameObject.SetActive(onMountain);
            if (onMountain)
            {
                Joba.Instance.transform.position = new Vector2(25.15816f, 54.98137f); 
            }
            else
            {
                Joba.Instance.transform.position = new Vector2(1000f, 1000f); 

            }
        }
        if (Boat.Instance != null)
            Boat.Instance.gameObject.SetActive(onIsland && !GameManager.Instance.boatCutscenePlayed);



        bool day7 = Player.Instance.eventNameTrigger == "Day7";
        if (Manananggal.Instance != null) Manananggal.Instance.gameObject.SetActive(day7);
    }


}
