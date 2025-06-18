using UnityEngine;

public class BotonAjustes : MonoBehaviour
{
    public void AbrirAjustes()
    {
        ConfiguracionesGlobales config = FindObjectOfType<ConfiguracionesGlobales>();
        if (config != null)
        {
            config.MostrarAjustes();
        }
    }

    public void CerrarAjustes()
    {
        ConfiguracionesGlobales config = FindObjectOfType<ConfiguracionesGlobales>();
        if (config != null)
        {
            config.OcultarAjustes();
        }
    }
}
