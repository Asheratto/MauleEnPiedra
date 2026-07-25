using UnityEngine;


/// <summary>
/// Represents the visual state of card in the game
///
/// </summary>
public class CardView : MonoBehaviour
{
    private CardInstance card;

    // Set data from a card instance in a card view
    public void Setup(CardInstance instance)
    {
        card = instance;
        
    }
}
