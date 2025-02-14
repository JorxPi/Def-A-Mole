using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false); // Asegurarse de que el panel esté oculto al inicio
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}