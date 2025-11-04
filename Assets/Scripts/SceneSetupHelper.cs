using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneSetupHelper : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/3D Object/Create First-Person Scene")]
    public static void CreateFirstPersonScene()
    {
        if (EditorUtility.DisplayDialog("Create First-Person Scene",
            "This will set up the Main scene with Player, Environment, UI, and Game Manager. Continue?",
            "Yes", "Cancel"))
        {
            SetupScene();
        }
    }

    private static void SetupScene()
    {
        GameObject player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        if (player != null)
        {
            GameObject playerInstance = PrefabUtility.InstantiatePrefab(player) as GameObject;
            if (playerInstance != null)
            {
                playerInstance.transform.position = new Vector3(0, 1, 0);
                playerInstance.tag = "Player";
                Debug.Log("Player created successfully");
            }
        }
        else
        {
            Debug.LogError("Player prefab not found at Assets/Prefabs/Player.prefab");
        }

        GameObject environment = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GreyboxEnvironment.prefab");
        if (environment != null)
        {
            GameObject envInstance = PrefabUtility.InstantiatePrefab(environment) as GameObject;
            if (envInstance != null)
            {
                envInstance.transform.position = Vector3.zero;
                Debug.Log("Environment created successfully");
            }
        }
        else
        {
            Debug.LogError("GreyboxEnvironment prefab not found");
        }

        GameObject gameManagerObj = new GameObject("GameManager");
        GameManager gameManager = gameManagerObj.AddComponent<GameManager>();
        Debug.Log("GameManager created successfully");

        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        GameObject crosshair = new GameObject("Crosshair");
        crosshair.transform.SetParent(canvas.transform, false);
        UnityEngine.UI.Image crosshairImage = crosshair.AddComponent<UnityEngine.UI.Image>();
        crosshairImage.color = new Color(1, 1, 1, 0.8f);
        RectTransform crosshairRect = crosshair.GetComponent<RectTransform>();
        crosshairRect.anchorMin = new Vector2(0.5f, 0.5f);
        crosshairRect.anchorMax = new Vector2(0.5f, 0.5f);
        crosshairRect.sizeDelta = new Vector2(6, 6);
        crosshairRect.anchoredPosition = Vector2.zero;
        crosshair.AddComponent<Crosshair>();
        Debug.Log("Crosshair created successfully");

        GameObject fpsCounterObj = new GameObject("FPS Counter");
        fpsCounterObj.transform.SetParent(canvas.transform, false);
        UnityEngine.UI.Text fpsText = fpsCounterObj.AddComponent<UnityEngine.UI.Text>();
        fpsText.text = "FPS: 60";
        fpsText.color = Color.green;
        fpsText.fontSize = 16;
        fpsText.alignment = TextAnchor.UpperRight;
        fpsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        RectTransform fpsRect = fpsCounterObj.GetComponent<RectTransform>();
        fpsRect.anchorMin = new Vector2(1, 1);
        fpsRect.anchorMax = new Vector2(1, 1);
        fpsRect.sizeDelta = new Vector2(100, 30);
        fpsRect.anchoredPosition = new Vector2(-20, -20);
        fpsCounterObj.AddComponent<FPSCounter>();
        Debug.Log("FPS Counter created successfully");

        GameObject pauseMenuObj = new GameObject("PauseMenu");
        pauseMenuObj.transform.SetParent(canvas.transform, false);
        pauseMenuObj.AddComponent<PauseMenu>();
        pauseMenuObj.SetActive(false);
        Debug.Log("Pause Menu created successfully");

        Debug.Log("===== Scene Setup Complete =====");
        Debug.Log("The First-Person scene has been set up successfully!");
        Debug.Log("Press Play to test the game.");
    }
#endif
}
