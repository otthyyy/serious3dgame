using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairImage;
    [SerializeField] private float defaultSize = 6f;

    private void Awake()
    {
        if (crosshairImage == null)
        {
            crosshairImage = GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        if (crosshairImage != null)
        {
            crosshairImage.sizeDelta = new Vector2(defaultSize, defaultSize);
        }
    }

    public void SetVisible(bool visible)
    {
        if (crosshairImage != null)
        {
            crosshairImage.gameObject.SetActive(visible);
        }
    }

    public void SetSize(float size)
    {
        if (crosshairImage != null)
        {
            crosshairImage.sizeDelta = new Vector2(size, size);
        }
    }
}
