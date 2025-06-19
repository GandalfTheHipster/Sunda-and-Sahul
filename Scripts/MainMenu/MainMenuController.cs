using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    [Tooltip("Scene to load when starting a brand new game")]
    [SerializeField] private string newGameScene = "TutorialWorld";

    [Tooltip("Scene to load when loading an existing save (change to your save-game scene)")]
    [SerializeField] private string loadGameScene = "LoadGameWorld";

    [Tooltip("Scene to load for settings, or leave empty if you just enable a panel instead")]
    [SerializeField] private string settingsScene = "Settings";

    /// <summary>
    /// Called by the “New Game” button
    /// </summary>
    public void OnNewGamePressed()
    {
        SceneManager.LoadScene(newGameScene);
    }

    /// <summary>
    /// Called by the “Load Game” button
    /// </summary>
    public void OnLoadGamePressed()
    {
        SceneManager.LoadScene(loadGameScene);
    }

    /// <summary>
    /// Called by the “Settings” button
    /// </summary>
    public void OnSettingsPressed()
    {
        if (!string.IsNullOrEmpty(settingsScene))
        {
            SceneManager.LoadScene(settingsScene);
        }
        else
        {
            Debug.LogWarning("Settings scene name is empty – maybe you meant to open a UI panel instead?");
        }
    }

    /// <summary>
    /// Called by the “Exit Game” button
    /// </summary>
    public void ExitGame()
    {
        // If we're running in the editor, stop Play Mode.
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}