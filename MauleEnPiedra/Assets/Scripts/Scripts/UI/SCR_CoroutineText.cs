using System.Collections;
using UnityEngine;
using TMPro;

public class SCR_CoroutineText : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    public float duracion = 2f; // duración total de la animación
    public float alturaMovimiento = 50f; // cuántos píxeles hacia arriba se mueve

    public IEnumerator AnimarTexto(string mensaje)
    {
        float tiempo = 0f;
        textoUI.alpha = 0f;
        textoUI.text = mensaje;
        textoUI.gameObject.SetActive(true);
        Vector3 posicionInicial = textoUI.rectTransform.anchoredPosition;
        Vector3 posicionFinal = posicionInicial + new Vector3(0, alturaMovimiento, 0);

        while (tiempo < duracion)
        {
            float t = tiempo / duracion;

            // Alpha suave tipo "ping-pong" (sube y baja un poco)
            float alpha = Mathf.Lerp(0.3f, 1f, Mathf.PingPong(t * 2f, 1f));
            textoUI.alpha = alpha;

            // Movimiento hacia arriba
            textoUI.rectTransform.anchoredPosition = Vector3.Lerp(posicionInicial, posicionFinal, t);

            tiempo += Time.deltaTime;
            yield return null;
        }

        textoUI.alpha = 0f;
        textoUI.gameObject.SetActive(false);
        textoUI.rectTransform.anchoredPosition = posicionInicial; // reinicia posición
    }
}
