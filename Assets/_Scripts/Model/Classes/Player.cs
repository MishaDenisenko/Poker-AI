using System.Collections.Generic;
using _Scripts.Model.Enums;
using UnityEngine.UI;

namespace _Scripts.Model.Classes {
    public class Player {
        private float _cash;
        private List<Card> _playerCards;

        public List<Card> PlayerCards {
            get => _playerCards;
            set => _playerCards = value;
        }
        
        public List<Card> PlayerCardsStuck { get; set; }

        public int CardSumValue { get; set; }
        public int PlayerCardSumValue { get; set; }
        public CardsCombinations CardCombination { get; set; }
        public CardValue HighCardValue { get; set; }

        public Player(float cash) {
            _cash = cash;
        }

        public float Cash {
            get => _cash;
            set => _cash = value > 0 ? value : 0;
        }

        public void Check() {
            
        }

        public void Bet(int count) {
            
        }

        public void Fold() {
            
        }

        public void Call(int count) {
            
        }

        public void Rise(int count) {
            
        }

        public override string ToString() {
            return $"cards - {PlayerCards[0]}{PlayerCards[1]}\n" +
                   $"combination - {CardCombination}\n" +
                   $"card combination - {PlayerCardsStuck}\n" +
                   $"high card - {HighCardValue}\n" +
                   $"all sum - {CardSumValue}\n" +
                   $"sum - {PlayerCardSumValue}";
        }
    }
}