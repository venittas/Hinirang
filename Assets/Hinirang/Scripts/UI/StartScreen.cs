using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button newGameButton; 
    [SerializeField] private Button continueButton; 
    [SerializeField] private Button quitButton;

    private RectTransform rectTransform;
    private Vector2 targetPosition;
    private bool isSlide = true;
    private const float  SLIDE_SPEED = 10f;
    public GameObject FadeInCanvas;
    public GameObject FadeOutCanvas;
    public bool isNewGame = false;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        MusicManager.Instance.PlayTrack(MusicManager.MusicTrack.Title);
        newGameButton.onClick.AddListener(() =>
        {
            Instantiate(FadeInCanvas);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetEverything(); 
            }
            else
            {
                SceneSystem.Instance.LoadScene(1);
            }
        });

        continueButton.onClick.AddListener(() =>
        {
            if (GameManager.Instance != null) 
            {
                Instantiate(FadeInCanvas);
                SceneSystem.Instance.LoadScene(
                    (int)SceneSystem.Instance.currentPlayerLocation);
            }
            else 
            {
                Instantiate(FadeInCanvas);
                SceneSystem.Instance.LoadScene(1);
            }
        });

        continueButton.interactable = GameManager.Instance != null;

        quitButton.onClick.AddListener(() => Application.Quit());

        targetPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 1000f);
    }

    public void ResetEverything()
    {
        GameManager.Instance.ResetEverything();
    }
    void Update()
    {
        if (isSlide)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * SLIDE_SPEED);

            if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
            {
                rectTransform.anchoredPosition = targetPosition;
                isSlide = false;
            }
            else
            {
                isSlide = true;
            }
        }
    }
}
