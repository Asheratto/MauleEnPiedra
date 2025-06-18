using UnityEngine;

public class PausaManager : MonoBehaviour
{
    public GameObject menuPausa; // Panel UI del menú de pausa
    private bool juegoPausado = false;

    void Update()
    {
        // Detectar si se presiona ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void PausarJuego()
    {
        menuPausa.SetActive(true); // Mostrar menú
        Time.timeScale = 0f;       // Detener tiempo
        juegoPausado = true;
    }

    public void ReanudarJuego()
    {
        menuPausa.SetActive(false); // Ocultar menú
        Time.timeScale = 1f;        // Reanudar tiempo
        juegoPausado = false;
    }

    public void VolverAlMenuPrincipal(string nombreEscena)
    {
        Time.timeScale = 1f; // Asegúrate de reanudar el tiempo antes de salir
        UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscena);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}