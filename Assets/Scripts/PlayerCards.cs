using MechanX;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCards : MonoBehaviour
{
    public int index;
    private Player _player;
    private GameObject _prefab;
    private List<GameObject> _gameObjects = new();

    private void Start()
    {
        _prefab = Resources.Load<GameObject>("Prefabs/PlayerCardCard");
        _player = GameController.CatanCardsGame.Players[index];
    }

    // Update is called once per frame
    void Update()
    {
        var cards = GameController.CatanCardsGame.State.PlayerCards[_player];
        
        foreach (var gameObject in _gameObjects)
        {
            Destroy(gameObject);
        }
        _gameObjects.Clear();

        foreach(var card in cards)
        {
            var go = Instantiate(_prefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            var display = go.GetComponent<PlayerCardDisplayCard>();
            display.card = card;
            _gameObjects.Add(go);
        }
    }
}
