using MC.Modelo;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "SO/Card")]
public class SO_Cards : ScriptableObject
{
    public new string name;
    public Card type;
    public Sprite image;
    public Zona zone;
    public NumPetroglifo nump;
    public PartePetroglifo parte;
    public int Code;
}
