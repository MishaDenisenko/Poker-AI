using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Controller;
using _Scripts.Model;
using _Scripts.Model.Classes;
using _Scripts.Model.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.View {
    public class GameView : MonoBehaviour {
        [SerializeField] private Sprite[] cardSprites;
        [SerializeField] private Image[] cardPositions;
        [SerializeField] private GameObject playersCards;

        private List<Card> _cardsInDeck = new();
        private DeckController _deckController;
        private CombinationController _combinationController;

        private Player _player1;
        private Player _player2;
        private Player _player3;

        private Image[][] _playerCardsImages = new Image[3][];
        private bool _complete;

        private void Awake() {
            _player1 = new Player(100);
            _player2 = new Player(100);
            _player3 = new Player(100);
            
            _deckController = DeckController.GetInstance(cardSprites);
            _combinationController = CombinationController.GetInstance(_cardsInDeck);
            _deckController.SetDeck();
        }

        private void Start() {
            for (int i = 0; i < 3; i++) {
                _playerCardsImages[i] = playersCards.transform.GetChild(i).GetComponentsInChildren<Image>();
            }
        }

        private void DealCards() {
            
        }

        public void PressBtn() {
            // if (!_complete) return;
            //
            // if (c == 0) {
            //     // _player.PlayerCards = _deckController.GetCardsSet(2);
            //     // var set2 = _deckController.GetCardsSet(2);
            //     // var set3 = _deckController.GetCardsSet(2);
            //     // for (int i = 0; i < _player.PlayerCards.Count; i++) {
            //     //     _player1[i].sprite = _player.PlayerCards[i].Sprite;
            //     // }
            //     // for (int i = 0; i < set2.Count; i++) {
            //     //     _player2[i].sprite = set2[i].Sprite;
            //     // }
            //     // for (int i = 0; i < set3.Count; i++) {
            //     //     _player3[i].sprite = set3[i].Sprite;
            //     // }
            // }
            // // if (c == 1) _cardsInDeck = _cardsInDeck.Concat(_deckController.GetCardsSet(3)).ToList();
            // // else if (c == 2 || c == 3) _cardsInDeck = _cardsInDeck.Concat(_deckController.GetCardsSet(1)).ToList();
            // // else if (c == 4) {
            // //     _combinstionController.SetCombinationToPlayer(_player.PlayerCards, _player);
            // //     // print(_player.ToString());
            // // }
            // else if (c == 1) {
            //     _combinationController.SetCombinationToPlayer(_player.PlayerCards, _player);
            //     print(_player.ToString());
            //     _player.PlayerCardCombination.ForEach(el => print(el));
            //     
            //     for (int i = 0; i < _combinationController.CardsOnTable.Count; i++) {
            //         cardPositions[i].sprite = _combinationController.CardsOnTable[i].Sprite;
            //     }
            //     for (int i = 0; i < 2; i++) {
            //         _player1[i].sprite = _player.PlayerCards[i].Sprite;
            //     }
            // }
            //
            //
            // c++;
        }
    }
}