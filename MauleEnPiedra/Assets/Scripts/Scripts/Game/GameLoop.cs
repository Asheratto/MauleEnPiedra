using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{

    //Supuestamente este es el loop asi q este verifica si el estado espera a la jugada o q onda
    TurnManager turnManager;

    private void Awake()
    {
        AIPlayerController aiCon = new AIPlayerController();
        HumanPlayerController humanCon = new HumanPlayerController();
        List<Player> players = new List<Player>();
        Player human = new Player(humanCon);
        Player ai = new Player(aiCon);
        players.Add(ai);
        players.Add(human);
        turnManager = new TurnManager(players, 0);
        
    }

    void Start()
    {
        //Random turn? -> Quien tiene la responsabilidad
        //Comienzo del juego
        //Start turn
        //turnManager.StartTurn();
        turnManager.StartTurn();

    }


    float timelapse = 5;
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        //Turn manager no controla el movimiento solo controlla quien juega
        //turnManager.star;

        /// <summary>
        /// Esto seria en el turno del player
        /// if(buttonx) drawcardcommand(player)
        /// if(buttonspace) endturncommand(player)
        /// 
        /// Esto seria en el turno de la is
        /// if(AI == X) endturncommand
        /// </summary>
        if (timer >= timelapse)
        {
            timer = 0;
            turnManager.EndTurn();
            
        }
        timer += Time.deltaTime;
    }
}
