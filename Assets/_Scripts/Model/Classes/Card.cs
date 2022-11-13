using System.Linq;
using _Scripts.Model.Enums;
using UnityEngine;

namespace _Scripts.Model.Classes {
    public class Card {
        private Sprite _sprite;
        private CardValue _cardValue;
        private CardSuits _cardSuits;

        public Card(Sprite sprite, int cardValue, int cardSuit) {
            _sprite = sprite;
            _cardValue = GetValue(cardValue);
            _cardSuits = GetSuit(cardSuit);
        }

        private CardValue GetValue(int index) {
            var values = typeof(CardValue)
                .GetFields()
                .Where(fi => fi.IsLiteral)
                .Select(fi => fi.GetRawConstantValue())
                .Cast<CardValue>()
                .ToArray();
            
            return values[index];
        }
        private CardSuits GetSuit(int index) {
            var suits = typeof(CardSuits)
                .GetFields()
                .Where(fi => fi.IsLiteral)
                .Select(fi => fi.GetRawConstantValue())
                .Cast<CardSuits>()
                .ToArray();
            
            return suits[index];
        }

        public Sprite Sprite => _sprite;

        public CardValue CardValue => _cardValue;

        public CardSuits CardSuits => _cardSuits;

        public override string ToString() {
            return $"value - {CardValue}\n" +
                   $"suit = {CardSuits}";
        }
    }
}