using UnityEngine;

public class AIPlayerController : IPlayerController
{
    public void EndTurn()
    {
        //throw new System.NotImplementedException();
    }

    public ICommand GetAction(GameState state)
    {
        throw new System.NotImplementedException();
    }

    public void MainTurn()
    {
        //throw new System.NotImplementedException();
    }

    public void StartTurn(Player player)
    {
        //TODO: 
        //throw new System.NotImplementedException();

        //We are goint to write a simple behavoiur to response a play

        Debug.Log("Juega " + player);

    }
}