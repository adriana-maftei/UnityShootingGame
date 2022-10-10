using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
        Application.Quit();
        Debug.Log("You quit the game!");
        }      
    }
}
