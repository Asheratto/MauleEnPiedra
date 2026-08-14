using UnityEngine;

public interface IPlayerController
{
    ICommand GetAction(GameState state);
    void StartTurn(Player player); // Start turn for the player
    void MainTurn();  //Player play during this method

    void EndTurn(); //Behaviour behind the once 
}
