using UnityEngine;

public class HumanPlayerController : IPlayerController
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
        //Logica del player durante el turno
        Debug.Log("Se reparten cartas... para" + player);
    }
}
