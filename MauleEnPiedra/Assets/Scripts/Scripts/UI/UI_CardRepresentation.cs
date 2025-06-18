using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_CardRepresentation : MonoBehaviour
{
    [HideInInspector] public SO_Cards CardData;
    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetCard(SO_Cards data)
    {
        
        CardData = data;
        if (_img == null) _img = GetComponent<Image>();
        _img.enabled = true;
        _img.sprite = CardData.image;
       
    }
}
