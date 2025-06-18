using UnityEngine;

public class ConfiguracionesGlobales : MonoBehaviour
{
    public GameObject panelAjustes; // Arrastra aquí tu Canvas/panel de ajustes

    private static ConfiguracionesGlobales instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MostrarAjustes()
    {
        panelAjustes.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    public void OcultarAjustes()
    {
        panelAjustes.SetActive(false);
        Time.timeScale = 1f; // Reanuda el juego
    }
}
