using UnityEngine;

public class DrawCard : ICommand
{
    Player player;

    public void Execute()
    {
        Debug.Log(player + ": Draw a card");
        //TODO: Player draw a card
        //player
    }

    public void Undo()
    {
        //TODO: devuelve la carta robada del mazo
        //throw new System.NotImplementedException();
    }
}
