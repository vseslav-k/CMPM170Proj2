using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadMainMenu() { SceneManager.LoadScene("MainMenu"); }
    public void LoadRoles() { SceneManager.LoadScene("Roles"); }
    public void LoadScore() { SceneManager.LoadScene("Score"); }
    public void LoadOgDrawer() { SceneManager.LoadScene("OgDrawer"); }
    public void LoadCloner() { SceneManager.LoadScene("Cloner"); }
    public void LoadJudge() { SceneManager.LoadScene("Judge"); }
}
