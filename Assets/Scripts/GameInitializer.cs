using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    private string message = "GameInitializer: Main scene loaded successfully.";

    private void Start()
    {
        Debug.Log(message);
    }
}
