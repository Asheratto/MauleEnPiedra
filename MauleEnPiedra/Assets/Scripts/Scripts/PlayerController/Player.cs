using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Player dont realize actions, player save data from his own GameState
/// Se supone que guarda la infromacion del player de la mano
/// </summary>

public class Player
{
    public IPlayerController controller;

    //public PlayerHand hand; // TODO:
    //public BoardHand board;
    //public int points;
    //

    public Player(IPlayerController controller)
    {
        this.controller = controller;
    }
}
