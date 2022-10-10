using UnityEngine;

public class Menu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You quit the game!");
    }
}
