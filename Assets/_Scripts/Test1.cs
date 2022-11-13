using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace _Scripts {
    public class Test1 : MonoBehaviour {
        [SerializeField] private Sprite[] cardSprites;
        [SerializeField] private Image[] cardPositions;

        private Random _random = new Random();
        private List<Sprite> _cardsInDeck;

        private void Start() {
            SetDeck();
        }

        public void SetDeck() {
            _cardsInDeck  = new (cardSprites);
        }

        public Sprite[] CardsDealing() {
            var cards = new Sprite[5];
            for (int i = 0; i < _cardsInDeck.Count; i++) {
                var index = _random.Next(0, _cardsInDeck.Count);
                cards[i] = _cardsInDeck[index];
                _cardsInDeck.RemoveAt(index);
            }

            return cards;
        }
    }
}

