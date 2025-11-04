using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float mouseSensitivity = 0.3f;
    [SerializeField] private float fieldOfView = 90f;

    private bool isPaused;
    private PlayerInputActions inputActions;

    private Canvas uiCanvas;
    private Crosshair crosshair;
    private FPSCounter fpsCounter;
    private PauseMenu pauseMenu;
    private GameObject pauseMenuPanel;
    private Slider sensitivitySlider;
    private Slider fovSlider;
    private Slider volumeSlider;
    private Text sensitivityValueText;
    private Text fovValueText;
    private Text volumeValueText;
    private Button resumeButton;
    private Button quitButton;
    private FirstPersonCamera cachedCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        inputActions = new PlayerInputActions();
        AudioListener.volume = masterVolume;
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        inputActions.UI.Pause.performed -= OnPausePressed;
        inputActions.UI.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
        UnregisterUIEvents();
    }

    private void Start()
    {
        cachedCamera = FindObjectOfType<FirstPersonCamera>();
        BuildUI();

        SetMasterVolume(masterVolume);
        SetMouseSensitivity(mouseSensitivity);
        SetFieldOfView(fieldOfView);
        SetPaused(false);
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        SetPaused(!isPaused);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = isPaused ? 0f : 1f;

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }

        crosshair?.SetVisible(!paused);
        fpsCounter?.SetVisible(!paused);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = masterVolume;

        if (volumeSlider != null && !Mathf.Approximately(volumeSlider.value, masterVolume))
        {
            volumeSlider.SetValueWithoutNotify(masterVolume);
        }

        if (volumeValueText != null)
        {
            volumeValueText.text = $"{masterVolume * 100f:0}%";
        }
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = Mathf.Clamp(sensitivity, 0.05f, 2f);

        if (cachedCamera == null)
        {
            cachedCamera = FindObjectOfType<FirstPersonCamera>();
        }

        cachedCamera?.SetSensitivity(mouseSensitivity);

        if (sensitivitySlider != null && !Mathf.Approximately(sensitivitySlider.value, mouseSensitivity))
        {
            sensitivitySlider.SetValueWithoutNotify(mouseSensitivity);
        }

        if (sensitivityValueText != null)
        {
            sensitivityValueText.text = mouseSensitivity.ToString("0.00");
        }
    }

    public void SetFieldOfView(float fov)
    {
        fieldOfView = Mathf.Clamp(fov, 70f, 110f);

        if (Camera.main != null)
        {
            Camera.main.fieldOfView = fieldOfView;
        }

        if (fovSlider != null && !Mathf.Approximately(fovSlider.value, fieldOfView))
        {
            fovSlider.SetValueWithoutNotify(fieldOfView);
        }

        if (fovValueText != null)
        {
            fovValueText.text = $"{fieldOfView:0}";
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public bool IsPaused => isPaused;
    public float MasterVolume => masterVolume;
    public float MouseSensitivity => mouseSensitivity;
    public float FieldOfView => fieldOfView;

    private void BuildUI()
    {
        uiCanvas = FindObjectOfType<Canvas>();
        if (uiCanvas == null)
        {
            var canvasGO = new GameObject("UI", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            uiCanvas = canvasGO.GetComponent<Canvas>();
            uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }

        EnsureEventSystem();

        crosshair = FindObjectOfType<Crosshair>();
        if (crosshair == null)
        {
            crosshair = CreateCrosshair(uiCanvas.transform);
        }

        fpsCounter = FindObjectOfType<FPSCounter>();
        if (fpsCounter == null)
        {
            fpsCounter = CreateFPSCounter(uiCanvas.transform);
        }

        pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu == null)
        {
            pauseMenu = CreatePauseMenu(uiCanvas.transform);
        }

        pauseMenu.SetPauseMenuPanel(pauseMenuPanel);

        RegisterUIEvents();
    }

    private void RegisterUIEvents()
    {
        UnregisterUIEvents();

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(HandleSensitivityChanged);
        }

        if (fovSlider != null)
        {
            fovSlider.onValueChanged.AddListener(HandleFovChanged);
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(HandleVolumeChanged);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(() => SetPaused(false));
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void UnregisterUIEvents()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners();
        }
        if (fovSlider != null)
        {
            fovSlider.onValueChanged.RemoveAllListeners();
        }
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
        }
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
        }
    }

    private void HandleSensitivityChanged(float value)
    {
        SetMouseSensitivity(value);
    }

    private void HandleFovChanged(float value)
    {
        SetFieldOfView(value);
    }

    private void HandleVolumeChanged(float value)
    {
        SetMasterVolume(value);
    }

    private Crosshair CreateCrosshair(Transform parent)
    {
        var crosshairGO = new GameObject("Crosshair", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Crosshair));
        crosshairGO.transform.SetParent(parent, false);

        var rect = crosshairGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(6f, 6f);

        var image = crosshairGO.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.85f);

        return crosshairGO.GetComponent<Crosshair>();
    }

    private FPSCounter CreateFPSCounter(Transform parent)
    {
        var fpsGO = new GameObject("FPSCounter", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text), typeof(FPSCounter));
        fpsGO.transform.SetParent(parent, false);

        var rect = fpsGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-20f, -20f);
        rect.sizeDelta = new Vector2(140f, 30f);

        var text = fpsGO.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 18;
        text.alignment = TextAnchor.UpperRight;
        text.color = Color.green;
        text.text = "FPS: --";

        return fpsGO.GetComponent<FPSCounter>();
    }

    private PauseMenu CreatePauseMenu(Transform parent)
    {
        var panelGO = new GameObject("PauseMenu", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(PauseMenu));
        panelGO.transform.SetParent(parent, false);
        pauseMenuPanel = panelGO;

        var rect = panelGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(420f, 460f);
        rect.anchoredPosition = Vector2.zero;

        var background = panelGO.GetComponent<Image>();
        background.color = new Color(0f, 0f, 0f, 0.7f);

        var font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        CreateTitle(panelGO.transform, font);
        CreateResumeButton(panelGO.transform, font);
        CreateQuitButton(panelGO.transform, font);
        CreateSliders(panelGO.transform, font);

        var menu = panelGO.GetComponent<PauseMenu>();
        menu.SetPauseMenuPanel(panelGO);
        panelGO.SetActive(false);
        return menu;
    }

    private void CreateTitle(Transform parent, Font font)
    {
        var titleGO = new GameObject("Title", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        titleGO.transform.SetParent(parent, false);
        var rect = titleGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -40f);
        rect.sizeDelta = new Vector2(360f, 60f);

        var text = titleGO.GetComponent<Text>();
        text.font = font;
        text.fontSize = 34;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.text = "PAUSED";
    }

    private void CreateResumeButton(Transform parent, Font font)
    {
        resumeButton = CreateButton(parent, font, "ResumeButton", "Resume", new Vector2(0f, -120f));
        resumeButton.onClick.AddListener(() => SetPaused(false));
    }

    private void CreateQuitButton(Transform parent, Font font)
    {
        quitButton = CreateButton(parent, font, "QuitButton", "Quit Game", new Vector2(0f, -360f));
        quitButton.onClick.AddListener(QuitGame);
    }

    private Button CreateButton(Transform parent, Font font, string name, string textValue, Vector2 anchoredPosition)
    {
        var buttonGO = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        buttonGO.transform.SetParent(parent, false);

        var rect = buttonGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(260f, 48f);

        var image = buttonGO.GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.2f);

        var button = buttonGO.GetComponent<Button>();
        button.targetGraphic = image;

        var textGO = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textGO.transform.SetParent(buttonGO.transform, false);
        var textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 0f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        var text = textGO.GetComponent<Text>();
        text.font = font;
        text.fontSize = 22;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.text = textValue;

        return button;
    }

    private void CreateSliders(Transform parent, Font font)
    {
        float startY = -190f;
        float spacing = 70f;

        CreateSliderBlock(parent, font, "Mouse Sensitivity", ref sensitivitySlider, ref sensitivityValueText, startY, 0.1f, 1.5f, mouseSensitivity);
        CreateSliderBlock(parent, font, "Field of View", ref fovSlider, ref fovValueText, startY - spacing, 70f, 110f, fieldOfView);
        CreateSliderBlock(parent, font, "Master Volume", ref volumeSlider, ref volumeValueText, startY - spacing * 2f, 0f, 1f, masterVolume);
    }

    private void CreateSliderBlock(
        Transform parent,
        Font font,
        string labelText,
        ref Slider slider,
        ref Text valueText,
        float anchoredY,
        float min,
        float max,
        float defaultValue)
    {
        var labelGO = new GameObject(labelText + " Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        labelGO.transform.SetParent(parent, false);
        var labelRect = labelGO.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0.5f, 1f);
        labelRect.anchorMax = new Vector2(0.5f, 1f);
        labelRect.pivot = new Vector2(0.5f, 1f);
        labelRect.anchoredPosition = new Vector2(0f, anchoredY);
        labelRect.sizeDelta = new Vector2(360f, 28f);

        var labelTextComp = labelGO.GetComponent<Text>();
        labelTextComp.font = font;
        labelTextComp.fontSize = 20;
        labelTextComp.alignment = TextAnchor.MiddleLeft;
        labelTextComp.color = Color.white;
        labelTextComp.text = labelText;

        valueText = new GameObject(labelText + " Value", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text)).GetComponent<Text>();
        valueText.transform.SetParent(parent, false);
        var valueRect = valueText.GetComponent<RectTransform>();
        valueRect.anchorMin = new Vector2(0.5f, 1f);
        valueRect.anchorMax = new Vector2(0.5f, 1f);
        valueRect.pivot = new Vector2(0.5f, 1f);
        valueRect.anchoredPosition = new Vector2(140f, anchoredY);
        valueRect.sizeDelta = new Vector2(120f, 28f);

        valueText.font = font;
        valueText.fontSize = 18;
        valueText.alignment = TextAnchor.MiddleRight;
        valueText.color = Color.white;

        slider = CreateSlider(parent, anchoredY - 30f, min, max, defaultValue);
    }

    private Slider CreateSlider(Transform parent, float anchoredY, float min, float max, float value)
    {
        var sliderGO = new GameObject("Slider", typeof(RectTransform), typeof(CanvasRenderer), typeof(Slider));
        sliderGO.transform.SetParent(parent, false);
        var rect = sliderGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, anchoredY);
        rect.sizeDelta = new Vector2(320f, 30f);

        var slider = sliderGO.GetComponent<Slider>();
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = Mathf.Clamp(value, min, max);
        slider.direction = Slider.Direction.LeftToRight;
        slider.transition = Selectable.Transition.None;

        var background = new GameObject("Background", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        background.transform.SetParent(sliderGO.transform, false);
        var bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 0.25f);
        bgRect.anchorMax = new Vector2(1f, 0.75f);
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        var bgImage = background.GetComponent<Image>();
        bgImage.color = new Color(1f, 1f, 1f, 0.2f);

        var fillArea = new GameObject("Fill Area", typeof(RectTransform));
        fillArea.transform.SetParent(sliderGO.transform, false);
        var fillAreaRect = fillArea.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0f, 0.25f);
        fillAreaRect.anchorMax = new Vector2(1f, 0.75f);
        fillAreaRect.offsetMin = new Vector2(8f, 0f);
        fillAreaRect.offsetMax = new Vector2(-8f, 0f);

        var fill = new GameObject("Fill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        fill.transform.SetParent(fillArea.transform, false);
        var fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(1f, 1f);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        var fillImage = fill.GetComponent<Image>();
        fillImage.color = new Color(0.8f, 0.8f, 0.8f, 0.9f);

        slider.fillRect = fillRect;

        var handleArea = new GameObject("Handle Slide Area", typeof(RectTransform));
        handleArea.transform.SetParent(sliderGO.transform, false);
        var handleAreaRect = handleArea.GetComponent<RectTransform>();
        handleAreaRect.anchorMin = new Vector2(0f, 0f);
        handleAreaRect.anchorMax = new Vector2(1f, 1f);
        handleAreaRect.offsetMin = new Vector2(10f, 0f);
        handleAreaRect.offsetMax = new Vector2(-10f, 0f);

        var handle = new GameObject("Handle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        handle.transform.SetParent(handleArea.transform, false);
        var handleRect = handle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(20f, 20f);
        var handleImage = handle.GetComponent<Image>();
        handleImage.color = Color.white;

        slider.targetGraphic = handleImage;
        slider.handleRect = handleRect;

        return slider;
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() != null) return;

        var eventSystemGO = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        eventSystemGO.transform.SetParent(uiCanvas.transform, false);
    }
}
