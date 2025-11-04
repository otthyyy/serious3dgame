using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private bool showFPS = true;
    [SerializeField] private float updateInterval = 0.5f;

    private float deltaTime = 0f;
    private float updateTimer = 0f;

    private void Awake()
    {
        if (fpsText == null)
        {
            fpsText = GetComponent<Text>();
        }
    }

    private void Update()
    {
        if (!showFPS || fpsText == null) return;

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        updateTimer += Time.unscaledDeltaTime;

        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            float fps = 1.0f / deltaTime;
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
            
            if (fps >= 60)
            {
                fpsText.color = Color.green;
            }
            else if (fps >= 30)
            {
                fpsText.color = Color.yellow;
            }
            else
            {
                fpsText.color = Color.red;
            }
        }
    }

    public void SetVisible(bool visible)
    {
        showFPS = visible;
        if (fpsText != null)
        {
            fpsText.gameObject.SetActive(visible);
        }
    }
}
