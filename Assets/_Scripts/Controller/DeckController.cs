using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

using _Scripts.Model;
using _Scripts.Model.Classes;
using _Scripts.Model.Enums;
using UnityEngine;

namespace _Scripts.Controller {
    public class DeckController {
        private static DeckController _instance;
        public static DeckController GetInstance(Sprite[] sprites) {
            return _instance == null ? new DeckController(sprites) : _instance;
        }
        
        private List<Card> _cardsInDeck;
        private List<Card> _cardsOnTable;
        private Sprite[] _sprites;
        private Random _random = new ();
        
        private DeckController(Sprite[] sprites) {
            _cardsInDeck = new();
            _cardsOnTable = new();
            _sprites = sprites;
        }
        
        public void SetDeck() {
            for (int i = 0; i < 4; i++) {
                for (int j = 13 * i; j < 13 * (i + 1); j++) {
                    _cardsInDeck.Add(new Card(_sprites[j], j % 13, i));
                }
            }
        }

        public List<Card> GetCardsSet(int count) {
            List<Card> set = new ();
            for (int i = 0; i < count; i++) {
                var index = _random.Next(0, _cardsInDeck.Count);
                set.Add(_cardsInDeck[index]);
                _cardsInDeck.RemoveAt(index);
            }

            return set;
        }

        public Card GetCard(CardValue value, CardSuits suit) {
            return _cardsInDeck.Find(card => card.CardValue == value && card.CardSuits == suit);
        }

        public List<Card> CardsInDeck => _cardsInDeck;

        public List<Card> CardsOnTable => _cardsOnTable;
    }
}