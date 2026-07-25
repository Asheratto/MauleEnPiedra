using UnityEngine;

public class GameLoop : MonoBehaviour
{
    TurnManager turnManager;

    private void Awake()
    {
        turnManager = new TurnManager();
        // = 0;
        turnManager.InitializeTurn(0);
    }

    void Start()
    {
        //Random turn? -> Quien tiene la responsabilidad
        //Comienzo del juego
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
