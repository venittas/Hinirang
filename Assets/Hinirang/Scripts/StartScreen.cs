using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button newGameButton; 
    [SerializeField] private Button continueButton; 
    [SerializeField] private Button modeLeftButton; 
    [SerializeField] private Button modeRightButton;
    private RectTransform rectTransform;
    private Vector2 targetPosition;
    private bool isSlide = true;
    private const float  SLIDE_SPEED = 10f;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        newGameButton.onClick.AddListener(() =>
        {
            Debug.Log("Start Game button clicked");
        });
        continueButton.onClick.AddListener(() =>
        {
            Debug.Log("Continue Game button clicked");
        });
        modeLeftButton.onClick.AddListener(() =>
        {
            Debug.Log("Mode Left button clicked");
        });
        modeRightButton.onClick.AddListener(() =>
        {
            Debug.Log("Mode Right button clicked");
        });

        targetPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 1000f);

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
        }
    }
}
