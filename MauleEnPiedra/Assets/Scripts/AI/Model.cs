using System.Collections.Generic;
using UnityEngine;



//public enum
public enum ZoneType { ZoneOne, ZoneTwo, ZoneThree }
public enum PetroglyphNumber { FirstPetro, SecondPetro, ThirdFragment }
public enum PetroglyphFragment { LowFragment, MidFragment, TopFragment}
    

//
public class CardMC
{
    public int Id;
    public string Name;
    public CardType Type;
    public ZoneType Zona;
    public PetroglyphNumber numPetroglifo;
    public PetroglyphFragment Parte;

    public CardMC(int id, string name, CardType type, ZoneType zona = ZoneType.ZoneOne, PetroglyphNumber num = PetroglyphNumber.FirstPetro,PetroglyphFragment parte = PetroglyphFragment.TopFragment)
    {
        Id = id;
        Name = name;
        Type = type;
        Zona = zona;
        numPetroglifo = num;
        Parte = parte;
    }


}

public class PlayerState
{
    public List<CardMC> Hand = new();
    public List<CardMC> ZoneArmado = new();
    public List<CardMC> ZoneAccion = new();
    public int Puntos = 0;
    public int TurnosProtegido = 0;
    public bool MuseoVirtual = false;
    public bool PierdeTurno = false;

    public PlayerState Clone()
    {
        return new PlayerState
        {
            Hand = new List<CardMC>(Hand),
            ZoneArmado = new List<CardMC>(ZoneArmado),
            ZoneAccion = new List<CardMC>(ZoneAccion),
            Puntos = Puntos,
            TurnosProtegido = TurnosProtegido,
            PierdeTurno = PierdeTurno
        };
    }
    public PlayerState()
    {

    }

    public PlayerState(List<CardMC> hand, List<CardMC> group, List<CardMC> special, int point, int protect, bool museo, bool lost) 
    {
        Hand = hand;
        ZoneArmado = group;
        ZoneAccion = special;
        Puntos = point;
        TurnosProtegido = protect;
        PierdeTurno = lost;
    }
}


public class GameState
{
    public List<CardMC> Deck = new();
    public List<CardMC> DiscardPile = new();
    public PlayerState Player1 = new();
    public PlayerState Player2 = new();
    public bool IsPlayer1Turn = true;
    public bool JuegoTerminado = false;
    public int goodHand = 0;


    public GameState Clone()
    {
        return new GameState
        {
            Deck = new List<CardMC>(Deck),
            DiscardPile = new List<CardMC>(DiscardPile),
            Player1 = Player1.Clone(),
            Player2 = Player2.Clone(),
            IsPlayer1Turn = IsPlayer1Turn,
            JuegoTerminado = JuegoTerminado,
            goodHand = goodHand
                
        };
    }

    public GameState() { }

    public GameState(List<CardMC> Maze, List<CardMC> Discard, Turn currenTurn, PlayerState player1, PlayerState player2, bool ended)
    {

        Deck = new List<CardMC>(Maze);
        DiscardPile = new List<CardMC>(DiscardPile);
        Player1 = player1;
        Player2 = player2;
        IsPlayer1Turn = currenTurn == Turn.Player ? true : false;
        JuegoTerminado = ended;
        goodHand = 0;
    }

}

    



