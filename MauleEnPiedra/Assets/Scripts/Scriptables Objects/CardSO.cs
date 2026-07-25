using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "SO/Card")]
public class CardSO : ScriptableObject
{
    public new string name;
    public int id;
    public CardType type;
    public Sprite image;
    public ZoneType zone;
    public PetroglyphNumber nump;
    public PetroglyphFragment parte;
    
}
