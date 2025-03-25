using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("JUGAR");
    }

    public void Exit()
    {
        Debug.Log("SALIR");

        Application.Quit();
    }
}
