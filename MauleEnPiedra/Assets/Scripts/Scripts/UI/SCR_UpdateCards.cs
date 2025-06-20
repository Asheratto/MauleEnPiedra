using Unity.VisualScripting;
using UnityEngine;

public class SCR_UpdateCards : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int index;
    [SerializeField] SCR_Player Player;

    private void Update()
    {
        /*if(Player.GetHand().Count != 0)
        {
            if (Player.GetHand()[index] != null) { }
            else
            {
                Destroy(transform.GetChild(0));
            }
        }*/
        
    }
}
