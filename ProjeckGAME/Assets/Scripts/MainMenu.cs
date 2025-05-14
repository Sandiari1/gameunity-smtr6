using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // ganti dengan nama scene game kamu
    }

    public void OpenGuide()
    {
        SceneManager.LoadScene("GuideScene"); // ganti dengan nama scene petunjuk
    }

    public void ExitGame()
    {
        Debug.Log("Keluar dari game!");
        Application.Quit(); // hanya bekerja di build
    }
}