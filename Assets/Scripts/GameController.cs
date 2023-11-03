using MechanX;
using MechanX.CatanCards;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static CatanCardsGame CatanCardsGame;

    void Awake()
    {
        var p1 = new Player { Name = "Player1" };
        var p2 = new Player { Name = "Player2" };
        var lst = new List<Player> { p1, p2 };
        CatanCardsGame = new CatanCardsGame(lst);
        //game.TakeAction
    }
}
