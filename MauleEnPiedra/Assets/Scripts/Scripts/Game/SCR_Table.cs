using MC.Modelo;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Table : MonoBehaviour
{

    //Lógica detras del juego
    [SerializeField] private SCR_Player Player;
    [SerializeField] private SCR_Player Ai;
    [SerializeField] private SO_DeckCard _deckCard;
    [SerializeField] private List<SO_Cards> DeckCard = new List<SO_Cards>();
    [SerializeField] private List<SO_Cards> _holeDeck = new List<SO_Cards>();
    [SerializeField] private CartaManager CardManager;
    [SerializeField] private ControladorEscenas SceneManager;
    [SerializeField] private SCR_CoroutineText TextManager;

    [SerializeField] private TextMeshProUGUI pointPlayer;
    [SerializeField] private TextMeshProUGUI pointAI;

    [SerializeField] private TextMeshProUGUI turnActual;
    [SerializeField] private TextMeshProUGUI turnsText;

    [SerializeField] private TextMeshProUGUI turnsProtect;
    [SerializeField] private TextMeshProUGUI turnsBlock;

    [SerializeField] private TextMeshProUGUI turnsProtectAI;
    [SerializeField] private TextMeshProUGUI turnsBlockAI;


    [SerializeField] private Button botonJugar;

    [SerializeField] private int turns = 0;
    //[Serializable]

    bool canIplay = true;

    public SCR_CoroutineQueue coroutineQueue;

    private Turn currentTurn;
    private GameStateFlow currentGameState;

    [SerializeField] private bool hasStartedTurn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GameStateFlow.InGame)
        {
            if (DeckCard.Count == 0)
            {
                foreach (var card in _holeDeck)
                {
                    DeckCard.Add(card);
                    coroutineQueue.Enqueue(CardManager.MoveCard(0, 0, card, CardZone.HoleMaze, CardZone.Maze, false, Turn.Player, 0.1f, 0.1f));
                }
                _holeDeck.Clear();
            }
            if (Player.GetPoints() >= 3)
            {
                currentGameState = GameStateFlow.EndGame;
            }
            else if(Ai.GetPoints() >= 3)
            {
                currentGameState = GameStateFlow.EndGame;
            }

            if (!hasStartedTurn)
            {
                hasStartedTurn = true;
                if (currentTurn == Turn.Player) { StartPlayerTurn(); }
                else { StartPlayerTurn(); }

                //UnityEngine.Debug.Log("Player Protect " + Player.indexProtect);
                
                if (Player.isProtect == true)
                {
                    Player.indexProtect++;
                    if (Player.indexProtect == 1)
                    {
                        Player.isProtect = false;
                        Player.indexProtect = 0;
                    }
                }
                if (Player.lostTurn == true)
                {
                    Player.indexTurn++;
                    if (Player.indexTurn > 2)
                    {
                        Player.lostTurn = false;
                        Player.indexTurn = 0;
                    }
                }
                if (Ai.lostTurn == true)
                {
                    Ai.indexTurn++;
                    if (Ai.indexTurn > 2)
                    {
                        Ai.lostTurn = false;
                        Ai.indexTurn = 0;
                    }
                }
                if (Ai.isProtect == true)
                {

                    Ai.indexProtect++;
                    if (Ai.indexProtect == 1)
                    {
                        Ai.isProtect = false;
                        Ai.indexProtect = 0;
                    }
                }
                if (Player.isLock == true)
                {
                    Player.indexLock++;
                    if (Player.indexLock == 2)
                    {
                        Player.isLock = false;
                        Player.indexLock = 0;
                    }
                }
                if (Ai.isLock == true)
                {
                    Ai.indexLock++;
                    if (Ai.indexLock == 2)
                    {
                        Ai.isLock = false;
                        Ai.indexLock = 0;
                    }
                }
            }
            else
            {
                if (currentTurn == Turn.Player && Player.lostTurn == false)
                {
                    //UnityEngine.Debug.Log("Juega Player ");
                    Player.block = false;
                    Ai.block = true;
                    PlayerPet();

                }
                else if (currentTurn == Turn.AI && Ai.lostTurn == false)
                {
                    botonJugar.interactable = false;
                    //UnityEngine.Debug.Log("Juega Ai");
                    Player.block = true;
                    Ai.block = false;
                    if (canIplay == true)
                    {
                        canIplay = false;
                        StartCoroutine(WaitAndPlayAI());
                    }
                }
                else if (currentTurn == Turn.AI && Ai.lostTurn == true) { 
                    NextTurn();
                }
            }
        }

        if (currentGameState == GameStateFlow.EndGame)
        {
            SceneManager.CambiarEscena("Victoria");
        }

        pointAI.text = Ai.GetPoints().ToString();
        pointPlayer.text = Player.GetPoints().ToString();
        
        turnsText.text = turns.ToString();
        
        turnsProtect.text = Player.indexProtect.ToString();
        turnsBlock.text = Player.indexLock.ToString();

        turnsProtectAI.text = Ai.indexProtect.ToString();
        turnsBlockAI.text = Ai.indexLock.ToString();

    }

    public void DrawRandomCard(Turn turn)
    {

        int index = UnityEngine.Random.Range(0, DeckCard.Count);


        if (turn == Turn.Player)
        {
            if (!Scr_Rules.FullHand(Player.GetHand()))
            {
                SO_Cards drawnCard = DeckCard[index];
                DeckCard.Remove(drawnCard);

                List<SO_Cards> hand = Player.GetHand();
                int insertIndex = -1;

                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i] == null)
                    {
                        insertIndex = i;
                        break;
                    }
                }

                if (insertIndex == -1)
                {
                    insertIndex = hand.Count;
                    hand.Add(null);
                }

                coroutineQueue.Enqueue(CardManager.MoveCard(0, insertIndex, drawnCard, CardZone.Maze, CardZone.Hand, true, Turn.Player, 0.2f, 0.2f));

                hand[insertIndex] = drawnCard;
            }
        }
        if (turn == Turn.AI)
        {
            if (!Scr_Rules.FullHand(Ai.GetHand()))
            {
                SO_Cards drawnCard = DeckCard[index];
                DeckCard.Remove(drawnCard);

                List<SO_Cards> hand = Ai.GetHand();
                int insertIndex = -1;

                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i] == null)
                    {
                        insertIndex = i;
                        break;
                    }
                }

                if (insertIndex == -1)
                {
                    insertIndex = hand.Count;
                    hand.Add(null);
                }

                coroutineQueue.Enqueue(CardManager.MoveCard(0, insertIndex, drawnCard, CardZone.Maze, CardZone.Hand, false, Turn.AI, 0.2f, 0.2f));

                hand[insertIndex] = drawnCard;
            }
        }
    }

    //public OrderHand

    public void DrawSpecificCard(Turn turn, SO_Cards card)
    {
        if (turn == Turn.Player)
        {
            if (!Scr_Rules.FullHand(Player.GetHand().Count))
            {
                coroutineQueue.Enqueue(CardManager.MoveCard(0, Player.GetHand().Count, card, CardZone.Maze, CardZone.Hand, true, Turn.Player));
                Player.DrawCard(card);
                DeckCard.Remove(card);
            }
        }
        if (turn == Turn.AI)
        {
            if (!Scr_Rules.FullHand(Ai.GetHand().Count))
            {
                coroutineQueue.Enqueue(CardManager.MoveCard(0, Ai.GetHand().Count, card, CardZone.Maze, CardZone.Hand, false, Turn.AI));
                Ai.DrawCard(card);
                DeckCard.Remove(card);
            }
        }
    }

    public void GameSetup()
    {
        currentTurn = (Turn)UnityEngine.Random.Range(0, 2);
        
        if (currentTurn == Turn.Player)
        {
            coroutineQueue.EnqueueText(TextManager.AnimarTexto("Turno de Jugador"));
        }
        else
        {
            coroutineQueue.EnqueueText(TextManager.AnimarTexto("Turno de IA"));
        }

        if (currentTurn == Turn.Player)
        {
            for (int i = 0; i < 5; i++)
            {
                DrawRandomCard(Turn.Player);
            }
            for (int i = 0; i < 5; i++)
            {
                DrawRandomCard(Turn.AI);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                DrawRandomCard(Turn.AI);
            }
            for (int i = 0; i < 5; i++)
            {
                DrawRandomCard(Turn.Player);
            }
        }
        currentGameState = GameStateFlow.InGame;
    }

    //Button Next 
    public void NextTurn()
    {
        currentTurn = (currentTurn == Turn.Player) ? Turn.AI : Turn.Player;
        if(Turn.Player == currentTurn)
        {
            canIplay = true;
        }
        turns++;

        
        hasStartedTurn = false;
    }

    private void StartPlayerTurn()
    {
        if (currentTurn == Turn.Player){
            if (!Scr_Rules.FullHand(Player.GetHand())){
                DrawRandomCard(Turn.Player);
                if (Player.isProtect == true)
                {
                    Player.indexProtect++;
                    if (Player.indexProtect == 1)
                    {
                        Player.isProtect = false;
                        Player.indexProtect = 0;
                    }
                }
                if (Ai.isProtect == true)
                {

                    Ai.indexProtect++;
                    if (Ai.indexProtect == 1)
                    {
                        Ai.isProtect = false;
                        Ai.indexProtect = 0;
                    }
                }
                if (Player.isLock == true)
                {
                    Player.indexLock++;
                    if (Player.indexLock == 2)
                    {
                        Player.isLock = false;
                        Player.indexLock = 0;
                    }
                }
                if (Ai.isLock == true)
                {
                    Ai.indexLock++;
                    if (Ai.indexLock == 2)
                    {
                        Ai.isLock = false;
                        Ai.indexLock = 0;
                    }
                }
            }
        }
        else{
            if (!Scr_Rules.FullHand(Ai.GetHand())){
                DrawRandomCard(Turn.AI);
            }
        }
    }

    private void PlayerPet()
    {
        if (Scr_Rules.PetroComplete(Player.GetGroup()))
        {
            foreach (var card in Player.GetGroup())
            {
                _holeDeck.Add(card);
            }

            Player.PetroComplete();
            if (Player.isLock == false)
            {
                Player.AddPlayerPoints();
                coroutineQueue.EnqueueText(TextManager.AnimarTexto("Jugador suma puntos"));                
            }


        }

        if (Scr_Rules.PetroInComplete(Player.GetGroup()))
        {
            foreach (var card in Player.GetGroup())
            {
                _holeDeck.Add(card);
            }
            Player.PetroComplete();
            coroutineQueue.EnqueueText(TextManager.AnimarTexto("Jugador no suma puntos"));

        }
    }

    private void PetAI()
    {
        if (Scr_Rules.PetroComplete(Ai.GetGroup()))
        {
            foreach (var card in Ai.GetGroup())
            {
                _holeDeck.Add(card);
            }

            Ai.PetroComplete();

            if (Player.isLock == false)
            {
                Ai.AddPlayerPoints();
                coroutineQueue.EnqueueText(TextManager.AnimarTexto("IA suma puntos"));
            }
        }

        if (Scr_Rules.PetroInComplete(Ai.GetGroup()))
        {
            foreach (var card in Ai.GetGroup())
            {
                _holeDeck.Add(card);
            }
            Ai.PetroComplete();
            coroutineQueue.EnqueueText(TextManager.AnimarTexto("IA no suma puntos"));

        }
    }

    public bool StartCard(SO_Cards card)
    {
        //Carta Especiales Activas
        switch (card.Code)
        {
            case 11:
                if (currentTurn == Turn.Player)
                {
                    Player.isProtect = true;
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Jugador se ha protegido"));
                }
                else
                {
                    Ai.isProtect = true;
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("IA se ha protegido"));
                }
                return true;
            // Bloquea Amenaza Esta carta es reactiva hacerla activa xdd Ets falta
            case 12:
            //Falta resolver
            // Saca Ultima Carta del pozo
            case 13:
                if (_holeDeck.Count <= 0) { coroutineQueue.EnqueueText(TextManager.AnimarTexto("No se ha activado Museo Virtual")); return false; }
                var lastCard = _holeDeck[_holeDeck.Count - 1];
                
                if (currentTurn == Turn.Player)
                {
                    if (Scr_Rules.FullHand(Player.GetHand())) {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("No se ha activado Museo Virtual"));
                        return false;
                    }
                    _holeDeck.Remove(lastCard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Se ha activado Museo Virtual"));
                    return Player.HoleToHand(lastCard);
                }
                else if (currentTurn == Turn.AI)
                {
                    if (Scr_Rules.FullHand(Ai.GetHand())){
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Se ha activado Museo Virtual"));
                        return false;
                    }
                    _holeDeck.Remove(lastCard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Se ha activado Museo Virtual"));
                    return Ai.HoleToHand(lastCard);
                }
                return false;

            //Cambia una carta con el contrincante. - Intercambio cultura;
            case 21: //
                //Esta bug pero funcionando
                var playerHand = Player.GetHand();
                var aiHand = Ai.GetHand();

                if (playerHand.Count == 0 || aiHand.Count == 0)
                {
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                    return false;
                }

                // Filtrar solo cartas no nulas
                var playerValidIndexes = Enumerable.Range(0, playerHand.Count).Where(i => playerHand[i] != null).ToList(); // [1, 4, 5]
                var aiValidIndexes = Enumerable.Range(0, aiHand.Count).Where(i => aiHand[i] != null).ToList();

                // Verificar si hay al menos una carta válida
                if (playerValidIndexes.Count == 0 || aiValidIndexes.Count == 0)
                {
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                    return false;
                }

                // Elegir una carta al azar de las válidas
                int randhandplayer = playerValidIndexes[UnityEngine.Random.Range(0, playerValidIndexes.Count)]; //[4]
                int randhandAI = aiValidIndexes[UnityEngine.Random.Range(0, aiValidIndexes.Count)];

                if (Player.GetHand()[randhandplayer].Code == 21 || Ai.GetHand()[randhandAI].Code == 21)
                {
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                    return false;
                }

                var cardPlayer = playerHand[randhandplayer]; 
                var cardAI = aiHand[randhandAI];

                // Remover las cartas
                playerHand.Remove(cardPlayer);
                aiHand.Remove(cardAI);

                //Jugar
                coroutineQueue.EnqueueText(TextManager.AnimarTexto("Intercambio cultural"));
                return Player.OponentHand(cardAI, randhandAI) && Ai.OponentHand(cardPlayer, randhandplayer);

            //Roba una carta del deck.
            case 22:
                if (currentTurn == Turn.Player)
                {
                    if (Scr_Rules.FullHand(Player.GetHand()))
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    DrawRandomCard(currentTurn);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Investigador"));
                    return true;
                }
                else if (currentTurn == Turn.AI)
                {
                    if (Scr_Rules.FullHand(Ai.GetHand()))
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    DrawRandomCard(currentTurn);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Investigador"));
                    return true;
                }
                coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                return false;
            //Roba una carta aleatoria del la mano contraria.
            case 23:
                if (currentTurn == Turn.Player)
                {
                    if (Scr_Rules.FullHand(Player.GetHand()))
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    var opoHand = Ai.GetHand();
                    var opovalidindex = Enumerable.Range(0, opoHand.Count).Where(i => opoHand[i] != null).ToList();
                    if (opovalidindex.Count == 0)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    int randhandop = opovalidindex[UnityEngine.Random.Range(0, opovalidindex.Count)];
                    var opocard = opoHand[randhandop];
                    opoHand.Remove(opocard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ladron de antiguedades"));
                    return Player.OponentHand(opocard, randhandop);
                }
                else if (currentTurn == Turn.AI)
                {
                    if (Scr_Rules.FullHand(Ai.GetHand()))
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    var opoHand = Player.GetHand();
                    var opovalidindex = Enumerable.Range(0, opoHand.Count).Where(i => opoHand[i] != null).ToList();
                    if (opovalidindex.Count == 0)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    int randhandop = opovalidindex[UnityEngine.Random.Range(0, opovalidindex.Count)];
                    var opocard = opoHand[randhandop];
                    opoHand.Remove(opocard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ladron de antiguedades"));
                    return Player.OponentHand(opocard, randhandop);
                }
                return false;
            //Descarta una carta aleatoria del armado de petroglifo del contrincante 
            case 31:
                
                if (currentTurn == Turn.Player)
                {
                    if (Ai.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    Ai.lostTurn = true;
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Abandono"));
                    return true;
                    //var opoGro = Ai.GetGroup();
                }
                else if (currentTurn == Turn.AI)
                {
                    if (Player.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    Player.lostTurn = true;
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Abandono"));
                    return true;
                    //return Player.DiscardCardHand(opocard, randhandop);
                }
                coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                return false;
            // Pierde una carta de la mano aleatoria 
            case 32:
                if (currentTurn == Turn.Player)
                {
                    if (Ai.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    var opoHand = Ai.GetHand();
                    var opovalidindex = Enumerable.Range(0, opoHand.Count).Where(i => opoHand[i] != null).ToList();
                    if (opovalidindex.Count == 0)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    int randhandop = opovalidindex[UnityEngine.Random.Range(0, opovalidindex.Count)];
                    var opocard = opoHand[randhandop];
                    opoHand.Remove(opocard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Desarrollo urbano"));
                    return Ai.DiscardCardHand(opocard, randhandop);

                    //var opoGro = Ai.GetGroup();
                }
                else if (currentTurn == Turn.AI)
                {
                    if (Player.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    var opoHand = Player.GetHand();
                    var opovalidindex = Enumerable.Range(0, opoHand.Count).Where(i => opoHand[i] != null).ToList();
                    if (opovalidindex.Count == 0)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    int randhandop = opovalidindex[UnityEngine.Random.Range(0, opovalidindex.Count)];
                    var opocard = opoHand[randhandop];
                    opoHand.Remove(opocard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Desarrollo urbano"));
                    return Player.DiscardCardHand(opocard, randhandop);
                }

                return true;
            // El contrincante pierde un turno
            case 33:
                //
                if (currentTurn == Turn.Player)
                {
                    if (Ai.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    var opoHand = Ai.GetGroup();
                    var opovalidindex = Enumerable.Range(0, opoHand.Count).Where(i => opoHand[i] != null).ToList();
                    if (opovalidindex.Count == 0)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    int randhandop = opovalidindex[UnityEngine.Random.Range(0, opovalidindex.Count)];
                    var opocard = opoHand[randhandop];
                    opoHand.Remove(opocard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Erosion"));
                    return Ai.DiscardGroup(opocard, randhandop);

                }
                else if (currentTurn == Turn.AI)
                {
                    if (Player.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    var opoHand = Player.GetGroup();
                    var opovalidindex = Enumerable.Range(0, opoHand.Count).Where(i => opoHand[i] != null).ToList();
                    if (opovalidindex.Count == 0)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    int randhandop = opovalidindex[UnityEngine.Random.Range(0, opovalidindex.Count)];
                    var opocard = opoHand[randhandop];
                    opoHand.Remove(opocard);
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Erosion"));
                    return Player.DiscardGroup(opocard, randhandop);
                }
                return true;
            // El contrincante no suma puntos por petroglifo por un turno
            case 34:
                if (currentTurn == Turn.Player)
                {
                    if (Ai.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    Ai.isLock = true;
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Vandalismo"));
                    return true;
                }
                else if (currentTurn == Turn.AI)
                {
                    if (Player.isProtect)
                    {
                        coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                        return false;
                    }
                    Ai.isLock = true;
                    coroutineQueue.EnqueueText(TextManager.AnimarTexto("Vandalismo"));
                    return true;
                }
                coroutineQueue.EnqueueText(TextManager.AnimarTexto("Ha fallado la carta"));
                return false;
            default:
                break;
        }
        return false;
    }

    public List<SO_Cards> GetHole()
    {
        return _holeDeck;
    }

    IEnumerator WaitAndPlayAI()
    {
        List<CardMC> _maze = new List<CardMC>();
        List<CardMC> _hole = new List<CardMC>();
        List<CardMC> handp1 = new List<CardMC>();
        List<CardMC> handp2 = new List<CardMC>();
        List<CardMC> handGroup1 = new List<CardMC>();
        List<CardMC> handGroup2 = new List<CardMC>();
        List<CardMC> handSpecial1 = new List<CardMC>();
        List<CardMC> handSpecial2 = new List<CardMC>();

        foreach (var card in DeckCard)
        {
            var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
            _maze.Add(_card);
        }
        foreach (var card in _holeDeck)
        {
            var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
            _hole.Add(_card);
        }
        foreach (var card in Player.GetHand())
        {
            if (card == null)
            {
                continue;
            }
            else
            {
                var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
                handp1.Add(_card);
            }

        }
        foreach (var card in Ai.GetHand())
        {
            if (card == null)
            {
                continue;
            }
            else
            {
                var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
                handp2.Add(_card);
            }

        }
        foreach (var card in Player.GetGroup())
        {
            if (card == null)
            {
                continue;
            }
            else
            {
                var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
                handGroup1.Add(_card);
            }

        }
        foreach (var card in Ai.GetGroup())
        {
            if (card == null)
            {
                continue;
            }
            else
            {
                var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
                handGroup2.Add(_card);
            }

        }
        foreach (var card in Player.GetSpe())
        {
            if (card == null)
            {
                continue;
            }
            else
            {
                var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
                handSpecial1.Add(_card);
            }

        }
        foreach (var card in Ai.GetSpe())
        {
            if (card == null){
                continue;}
            else{
                var _card = new CardMC(card.Code, card.name, card.type, card.zone, card.nump, card.parte);
                handSpecial1.Add(_card);
            }
        }

        PlayerState p1 = new PlayerState(handp1, handGroup1, handSpecial1, Player.GetPoints(), Player.indexProtect, false, Player.lostTurn);
        PlayerState p2 = new PlayerState(handp2, handGroup2, handSpecial2, Ai.GetPoints(), Ai.indexProtect, false, Ai.lostTurn);

        GameState state = new GameState(_maze, _hole, Turn.AI, p1, p2, false);

        
        int index = MonteCarlo.MonteCarloTS(state, 12, 1).mejorJugada;
        UnityEngine.Debug.Log(index);
        for(int i = 0; i < 6; i++) {if(index == i) { if (Ai.GetHand()[i] != null){ break; } index++; }}

        yield return new WaitForSeconds(3f);
        Ai.ClickHand(index);
        PetAI();
        Ai.OrderHand();
        NextTurn();
        botonJugar.interactable = true;
    }
}


