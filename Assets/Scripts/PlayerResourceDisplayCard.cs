using MechanX.CatanCards.Model;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceDisplayCard : MonoBehaviour
{
    public ResourceType resourceType;

    private void Start()
    {
        var cardImage = GetComponent<Image>();
        cardImage.sprite = CardLibrary.GetByResourceType(resourceType).sprite;
    }
}