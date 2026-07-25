using System.Collections.Generic;
using UnityEngine;

//Esta clase maneja directamente los turnos de los personajes
//Quien maneja quien empieza
public class TurnManager
{
    private List<IPlayerController> players;

    private int currentPlayerIndex;

    //La lista de player no es player controller...
    public IPlayerController CurrentPlayer => players[currentPlayerIndex];

    public TurnPhase phase;

    

    public void StartTurn()
    {
        phase = TurnPhase.Start;

        Debug.Log(
            $"Turno de {CurrentPlayer}"
        );

        StartMainPhase();
    }


    private void StartMainPhase()
    {
        phase = TurnPhase.Main;

        CurrentPlayer.TakeTurn();
    }


    public void EndTurn()
    {
        phase = TurnPhase.End;

        NextPlayer();
    }


    private void NextPlayer()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
        }

        StartTurn();
    }

    public void InitializeTurn(int i)
    {
        currentPlayerIndex = i;
    }
}