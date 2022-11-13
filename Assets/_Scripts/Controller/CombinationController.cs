using System.Collections.Generic;
using System.Linq;
using _Scripts.Model;
using _Scripts.Model.Classes;
using _Scripts.Model.Enums;
using UnityEngine;

namespace _Scripts.Controller {
    public class CombinationController {
        private static CombinationController _instance;

        public static CombinationController GetInstance(List<Card> cardsOnTable) {
            return _instance == null ? new CombinationController(cardsOnTable) : _instance;
        }

        private List<Card> _cardsOnTable;
        private Dictionary<CardsCombinations, int> _cardsCombination;
        private readonly List<CardValue> _royalFlush = new() {CardValue.Ace, CardValue.King, CardValue.Qween, CardValue.Jack, CardValue.Ten};

        public CombinationController(List<Card> cardsOnTable) {
            _cardsOnTable = cardsOnTable;
            _cardsCombination = new Dictionary<CardsCombinations, int>();
        }

        public List<Card> CardsOnTable {
            get => _cardsOnTable;
            set => _cardsOnTable = value;
        }

        public void SetCombinationToPlayer(List<Card> playerCards, Player player) {
            SetHighCardToPlayer(playerCards, player);

            var allCards = _cardsOnTable.Concat(playerCards).ToList();
            var sortedCardsList = SortCardsList(allCards);
            sortedCardsList.ForEach(el=>Debug.Log(el.ToString()));

            var royalFlush = GetRoyalFlush(sortedCardsList);
            if (royalFlush.Count == 5) {
                player.PlayerCardsStuck = royalFlush;
                player.CardCombination = CardsCombinations.RoyalFlush;
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
                return;
            }

            var straightFlush = GetStraightFlush(sortedCardsList);
            if (straightFlush.Count == 5) {
                player.PlayerCardsStuck = straightFlush;
                player.CardCombination = CardsCombinations.StraightFlush;
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);;
                return;
            }

            var fourOfKind = GetFourOfKind(sortedCardsList);
            if (fourOfKind.Count == 5) {
                player.PlayerCardsStuck = fourOfKind;
                player.CardCombination = CardsCombinations.FourOfKind;
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
                return;
            }

            var fullHouse = GetFullHouse(sortedCardsList);
            if (fullHouse.Count == 5) {
                player.PlayerCardsStuck = fullHouse;
                player.CardCombination = CardsCombinations.FullHouse;
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
                return;
            }

            var flush = GetFlush(sortedCardsList);
            if (flush.Count == 5) {
                player.PlayerCardsStuck = flush;
                player.CardCombination = CardsCombinations.Flush;
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
                return;
            }

            var straight = GetStaight(sortedCardsList);
            if (straight.Count == 5) {
                player.PlayerCardsStuck = straight;
                player.CardCombination = CardsCombinations.Straight;
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
                return;
            }

            var triplets = GetTriplets(sortedCardsList);
            if (triplets.Count > 0) {
                if (triplets.Count == 3) {
                    player.PlayerCardsStuck = triplets
                        .Concat(GetHightCards(sortedCardsList.Except(triplets).ToList(), 2))
                        .ToList();
                }

                else if (triplets.Count == 6) {
                    player.PlayerCardsStuck = GetBetterCombination(triplets);
                    player.PlayerCardsStuck = player.PlayerCardsStuck
                        .Concat(GetHightCards(sortedCardsList.Except(player.PlayerCardsStuck).ToList(), 2))
                        .ToList();
                }
                player.CardCombination = CardsCombinations.Triplet;
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
                return;
            }
            
            var pairs = GetPairs(sortedCardsList);
            if (pairs.Count > 0) {
                if (pairs.Count == 2) {
                    player.PlayerCardsStuck = pairs
                        .Concat(GetHightCards(sortedCardsList.Except(pairs).ToList(), 3))
                        .ToList();
                    player.CardCombination = CardsCombinations.OnePair;
                }
                else if (pairs.Count == 4) {
                    player.PlayerCardsStuck = pairs
                        .Concat(GetHightCards(sortedCardsList.Except(pairs).ToList()))
                        .ToList();
                    player.CardCombination = CardsCombinations.TwoPairs;
                }
                else if (pairs.Count == 6) {
                    player.PlayerCardsStuck = GetBetterCombination(pairs);
                    player.PlayerCardsStuck = player.PlayerCardsStuck
                        .Concat(GetBetterCombination(pairs.Except(player.PlayerCardsStuck).ToList()))
                        .ToList();
                    player.PlayerCardsStuck = player.PlayerCardsStuck
                        .Concat(GetHightCards(sortedCardsList.Except(player.PlayerCardsStuck).ToList()))
                        .ToList();
                    player.CardCombination = CardsCombinations.TwoPairs;
                }
                player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
                return;
            }

            var hightCards = GetHightCards(sortedCardsList, 5);
            player.CardCombination = CardsCombinations.HighCard;
            player.PlayerCardsStuck = hightCards;
            player.CardSumValue = GetSumValues(player.PlayerCardsStuck);
        }

        private void SetHighCardToPlayer(List<Card> playerCards, Player player) {
            player.CardCombination = CardsCombinations.HighCard;
            player.HighCardValue = playerCards.Max(card => card.CardValue);

            if (IsPair(playerCards)) player.CardCombination = CardsCombinations.OnePair;

            player.PlayerCardSumValue = GetSumValues(playerCards);
        }

        private List<Card> SortCardsList(List<Card> cards) {
            return cards.OrderBy(card => card.CardValue).ToList();
        }

        private bool IsPair(List<Card> cards) {
            return cards.GroupBy(card => card.CardValue).Any(card => card.Count() == 2);
        }

        private List<Card> GetRoyalFlush(List<Card> cards) {
            List<Card> royalFlushCards = GetStraightFlush(cards);
            if (royalFlushCards.Count == 0) return royalFlushCards;
            
            var cardsValues = royalFlushCards.Select(card => card.CardValue).ToList();
            var isEqual = Enumerable.SequenceEqual(cardsValues.OrderBy(c => c), _royalFlush.OrderBy(c => c));

            if (!isEqual) royalFlushCards.Clear();
            return royalFlushCards;
        }

        private List<Card> GetStraightFlush(List<Card> cards) {
            CardSuits suit = BiggestCountOfSiut(cards);
            List<Card> straightFlushCards = cards.FindAll(card => card.CardSuits == suit);
            
            if (straightFlushCards.Count >= 5) {
                straightFlushCards = GetHightCards(straightFlushCards, 5);
                
                for (int i = 0; i < 4; i++) {
                    if (straightFlushCards[i].CardValue != straightFlushCards[i + 1].CardValue - 1) {
                        straightFlushCards.Clear();
                        break;
                    }
                }
            }
            

            return straightFlushCards;
        }

        private List<Card> GetFourOfKind(List<Card> cards) {
            List<Card> fourOfKindCards = GetHightCards(cards, 4);
            
            for (int i = 0; i < fourOfKindCards.Count - 1; i++) {
                if (fourOfKindCards[i].CardValue != fourOfKindCards[i + 1].CardValue) {
                    fourOfKindCards.Clear();
                    break;
                }
            }
            
            if (fourOfKindCards.Count == 4) {
                var kicker = cards.Find(card => card.CardValue == cards
                                                                            .Except(fourOfKindCards)
                                                                            .Max(c => c.CardValue));
                fourOfKindCards.Add(kicker);
            }

            return fourOfKindCards;
        }

        private List<Card> GetFullHouse(List<Card> cards) {
            List<Card> hightTriplet;
            List<Card> hightPair;

            var triplets = GetTriplets(cards);
            if (triplets.Count == 6) hightTriplet = GetBetterCombination(triplets);
            else hightTriplet = triplets;

            var otherCards = cards.Except(hightTriplet).ToList();

            var pairs = GetPairs(otherCards);
            if (pairs.Count > 2) hightPair = GetBetterCombination(pairs);
            else hightPair = pairs;

            List<Card> fullHouseCards = hightPair.Concat(hightTriplet).ToList();

            return fullHouseCards;
        }

        private List<Card> GetFlush(List<Card> cards) {
            CardSuits suit = BiggestCountOfSiut(cards);
            List<Card> flush = cards.FindAll(card => card.CardSuits == suit);
            
            if (flush.Count == 6) {
                flush.Remove(flush.Find(card => card.CardValue == flush.Min(c => c.CardValue)));
            }
            else if (flush.Count == 7) {
                flush.Remove(flush.Find(card => card.CardValue == flush.Min(c => c.CardValue)));
                flush.Remove(flush.Find(card => card.CardValue == flush.Min(c => c.CardValue)));
            }

            return flush;
        }

        private List<Card> GetStaight(List<Card> cards) {
            List<Card> staight = new();

            int i = cards.Count - 1;
            staight.Add(cards[i]);

            while (i > 0) {
                Debug.Log(i);
                var prev = cards[i];
                var next = cards[i - 1];
                
                if (prev.CardValue == next.CardValue) {
                    if (i - 2 < 0) break;
                    
                    next = cards[i - 2];
                    i--;
                }
                Debug.Log($"{prev} | {next}");
                Debug.Log(prev.CardValue == next.CardValue + 1);
                if (prev.CardValue == next.CardValue + 1) staight.Add(next);
                else staight.Clear();
                Debug.Log($"c = {staight.Count}");
                if (staight.Count == 5) break;
                
                i--;
            }

            return staight;
        }

        public List<Card> GetPairs(List<Card> cards) {
            return cards
                .GroupBy(c => c.CardValue)
                .Where(g => g.Count() == 2)
                .SelectMany(g => g.Select(c => c).ToList())
                .ToList();
        }

        public List<Card> GetTriplets(List<Card> cards) {
            return cards
                .GroupBy(c => c.CardValue)
                .Where(g => g.Count() == 3)
                .SelectMany(g => g.Select(c => c).ToList())
                .ToList();
        }

        private int GetSumValues(List<Card> cards) {
            return cards.Sum(card => (int) card.CardValue);
        }

        private List<Card> GetHightCards(List<Card> cards, int count = 1) {
            var start = cards.Count - count; 
            return cards.OrderBy(card => card.CardValue).ToList().GetRange(start, count);
        }

        private List<Card> GetBetterCombination(List<Card> cards) {
            return cards.FindAll(card => card.CardValue == cards.Max(c => c.CardValue));
        }

        private CardSuits BiggestCountOfSiut(List<Card> cards) {
            Dictionary<CardSuits, int> suitsCount = new();
            suitsCount.Add(CardSuits.Clubs, cards.Count(card => card.CardSuits == CardSuits.Clubs));
            suitsCount.Add(CardSuits.Diamonds, cards.Count(card => card.CardSuits == CardSuits.Diamonds));
            suitsCount.Add(CardSuits.Hearts, cards.Count(card => card.CardSuits == CardSuits.Hearts));
            suitsCount.Add(CardSuits.Spades, cards.Count(card => card.CardSuits == CardSuits.Spades));

            return suitsCount.OrderByDescending(x => x.Value).First().Key;
        }
        
    }
}