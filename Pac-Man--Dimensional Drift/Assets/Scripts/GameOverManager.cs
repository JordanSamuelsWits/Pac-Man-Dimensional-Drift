using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("MainGame"); // Replace with your main game scene's name
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // This will quit the game if you're running a build
    }
}
