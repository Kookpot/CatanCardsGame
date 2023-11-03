using MechanX.CatanCards.Model;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public static Dictionary<int, CardInfo> Cards = new();

    void Awake()
    {
        Cards.Add(1, new CardInfo(Resources.Load<Sprite>("Cards/Grain")));
        Cards.Add(2, new CardInfo(Resources.Load<Sprite>("Cards/Wood")));
        Cards.Add(3, new CardInfo(Resources.Load<Sprite>("Cards/Sheep")));
        Cards.Add(4, new CardInfo(Resources.Load<Sprite>("Cards/ore")));
        Cards.Add(5, new CardInfo(Resources.Load<Sprite>("Cards/brick")));
        Cards.Add(6, new CardInfo(Resources.Load<Sprite>("Cards/hijack")));
        Cards.Add(7, new CardInfo(Resources.Load<Sprite>("Cards/marketday")));
        Cards.Add(8, new CardInfo(Resources.Load<Sprite>("Cards/village")));
        Cards.Add(9, new CardInfo(Resources.Load<Sprite>("Cards/cityhall")));
        Cards.Add(10, new CardInfo(Resources.Load<Sprite>("vcitywall")));
        Cards.Add(11, new CardInfo(Resources.Load<Sprite>("Cards/library")));
        Cards.Add(12, new CardInfo(Resources.Load<Sprite>("Cards/university")));
        Cards.Add(13, new CardInfo(Resources.Load<Sprite>("Cards/guild")));
        Cards.Add(14, new CardInfo(Resources.Load<Sprite>("Cards/knightA")));
        Cards.Add(15, new CardInfo(Resources.Load<Sprite>("Cards/knightB")));
        Cards.Add(16, new CardInfo(Resources.Load<Sprite>("Cards/streetA")));
        Cards.Add(17, new CardInfo(Resources.Load<Sprite>("Cards/streetB")));
        Cards.Add(18, new CardInfo(Resources.Load<Sprite>("Cards/church")));
    }

    public static CardInfo GetByResourceType(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Grain:
                return Cards[1];
            case ResourceType.Wood:
                return Cards[2];
            case ResourceType.Sheep:
                return Cards[3];
            case ResourceType.Ore:
                return Cards[4];
            default:
                return Cards[5];
        }
    }

    public static CardInfo GetByMechanXCard(Card card)
    {
        switch (card.CardType)
        {
            case CardType.City:
                if (card is Village village && village.IsHijack)
                {
                    return Cards[6];
                }
                else
                {
                    return Cards[7];
                }
            case CardType.Village:
                return Cards[8];
            case CardType.CityHall:
                return Cards[9];
            case CardType.CityWall:
                return Cards[10];
            case CardType.Library:
                return Cards[11];
            case CardType.University:
                return Cards[12];
            case CardType.Guild:
                return Cards[13];
            case CardType.Knight:
                if (card is Knight knight && !knight.BackSide)
                {
                    return Cards[14];
                }
                else
                {
                    return Cards[15];
                }
            case CardType.Street:
                if (card is Street street && !street.BackSide)
                {
                    return Cards[16];
                }
                else
                {
                    return Cards[17];
                }
            default:
                return Cards[18];
        }
    }
}