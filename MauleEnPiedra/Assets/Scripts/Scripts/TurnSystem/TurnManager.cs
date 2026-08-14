using System.Collections.Generic;
using UnityEngine;

//Esta clase maneja directamente los turnos de los personajes
//Quien maneja quien empieza
//No maneja eventos ni nada solo maneja a quien le toca, cuando empieza y cuando termina el turno para cambiar de player
public class TurnManager
{
    private List<Player> players;

    private int currentPlayerIndex;

    public Player CurrentPlayer => players[currentPlayerIndex];

    public TurnPhase phase;

    
    public TurnManager(List<Player> players, int firstPlayer)
    {
        this.players = players;
        this.currentPlayerIndex = firstPlayer;
    }


    public void StartTurn()
    {
        phase = TurnPhase.Start;

        Debug.Log(
            $"Turno de {CurrentPlayer} + {CurrentPlayer.controller}"
        );
        StartMainPhase();
    }


    
    private void StartMainPhase()
    {
        phase = TurnPhase.Main;

        /*/ICommand action = CurrentPlayer.Controller.GetAction(gameState);

        if (action != null)
        {
            action.Execute();
        }*/
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

}

