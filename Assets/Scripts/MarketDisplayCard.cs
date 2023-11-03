using UnityEngine;
using UnityEngine.UI;

public class MarketDisplayCard : MonoBehaviour
{
    public int index;

    private void Update()
    {
        var cardImage = GetComponent<Image>();
        var resourceType = GameController.CatanCardsGame.State.Market[index];
        cardImage.sprite = CardLibrary.GetByResourceType(resourceType).sprite;
    }
}