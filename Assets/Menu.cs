using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void loadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void quit()
    {
        Application.Quit();         
    }
}
