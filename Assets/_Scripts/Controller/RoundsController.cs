using System.Linq;
using _Scripts.Model.Enums;

namespace _Scripts.Controller {
    public class RoundsController {
        private static RoundsController _instance;
        public static RoundsController GetInstance() {
            return _instance == null ? new RoundsController() : _instance;
        }

        public GameRounds GameRound { get; private set; } = GameRounds.PlayersDeal;

        public void SetNextRound() {
            switch (GameRound) {
                case GameRounds.PlayersDeal: GameRound = GameRounds.OwnDeal; break;
                case GameRounds.OwnDeal: GameRound = GameRounds.FirstShowdown; break;
                case GameRounds.FirstShowdown: GameRound = GameRounds.SecondShowdown; break;
                case GameRounds.SecondShowdown: GameRound = GameRounds.ThirdShowDown; break;
                case GameRounds.ThirdShowDown: GameRound = GameRounds.LastShowdown; break;
            }
        }
    }
}