using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Ô này để kéo Panel vào

    public void PlayGame()
    {
        // Nó sẽ load scene có tên là GameScene trong Build Settings
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true); // Hiện bảng Setting
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Ẩn bảng Setting
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Đã thoát Game!");
    }
}