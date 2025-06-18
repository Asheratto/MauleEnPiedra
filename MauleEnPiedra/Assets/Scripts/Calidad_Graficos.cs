using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Calidad : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int calidad;

    

    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("numeroDeCalidad", dropdown.value);
        calidad = dropdown.value;
    }
    
}
