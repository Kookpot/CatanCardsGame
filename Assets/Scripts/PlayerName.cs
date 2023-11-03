using MechanX;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public int index;
    private Player _player;
    private TextMeshProUGUI _text;

    private void Start()
    {
        _player = GameController.CatanCardsGame.Players[index];
        _text = gameObject.GetComponent<TextMeshProUGUI>();
        _text.text = _player.Name;
    }

    private void Update()
    {
        if(GameController.CatanCardsGame.State.CurrentPlayer == _player)
        {
            _text.color = Color.red;
        }
        else
        {
            _text.color = Color.white;
        }
    }
}
