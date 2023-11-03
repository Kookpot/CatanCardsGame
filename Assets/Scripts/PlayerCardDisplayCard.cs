using MechanX.CatanCards.Model;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardDisplayCard : MonoBehaviour
{
    public Card card;

    private void Start()
    {
        var cardImage = GetComponent<Image>();
        cardImage.sprite = CardLibrary.GetByMechanXCard(card).sprite;
    }
}