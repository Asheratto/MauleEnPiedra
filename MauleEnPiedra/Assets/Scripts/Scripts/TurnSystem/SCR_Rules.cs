using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Esta clase se encarga de toda la logica de revisar si se cumples las reglas

public static class Scr_Rules
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public static bool FullHand(int numberOfCards)
    {
        if(numberOfCards < 6 ){ return false; }
        return true;
    }
    
    //La mano esta llena no se pueden agregar mas cartas
    public static bool FullHand(List<CardSO> list)
    {
        if (list == null) return false;

        // Si tiene menos de 6 cartas, no está llena
        if (list.Count < 6) return false;

        // Si alguna carta es null, tampoco está llena
        foreach (CardSO card in list)
        {
            if (card == null) return false;
        }

        return true; // Si tiene 6 cartas no nulas
    }

    // El grupo esta lleno por lo tanto no se puede jugar la carta
    public static bool FullGroup(List<CardSO> list)
    {
        if (list == null) return false;

        // Si tiene menos de 6 cartas, no está llena
        if (list.Count < 3) return false;

        // Si alguna carta es null, tampoco está llena
        foreach (CardSO card in list)
        {
            if (card == null) return false;
        }

        return true; // Si tiene 6 cartas no nulas
    }

    public static bool FullSpecial(List<CardSO> list)
    {
        if (list == null) return false;

        // Si tiene menos de 6 cartas, no está llena
        if (list.Count < 2) return false;

        // Si alguna carta es null, tampoco está llena
        foreach (CardSO card in list)
        {
            if (card == null) return false;
        }

        return true; // Si tiene 6 cartas no nulas
    }

    public static bool PetroComplete(List<CardSO> list)
    {
        if (list == null || list.Count != 3)
            return false;

        if (list.Any(card => card == null))
            return false;

        int code = list[0].id;

        return list.All(card => card.id == code);
    }

    public static bool PetroInComplete(List<CardSO> list)
    {
        if (list == null || list.Count != 3)
            return false;

        if (list.Any(card => card == null))
            return false;



        return true;
    }

    
}
