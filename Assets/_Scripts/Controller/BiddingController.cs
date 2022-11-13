using _Scripts.Model.Enums;

namespace _Scripts.Controller {
    public class BiddingController {
        private static BiddingController _instance;
        public static BiddingController GetInstance() {
            return _instance == null ? new BiddingController() : _instance;
        }

        public BiddingRounds BiddingRound { get; private set; }

        public void SetNextRound() {
            switch (BiddingRound) {
                case BiddingRounds.PreFlop: BiddingRound = BiddingRounds.Flop; break;
                case BiddingRounds.Flop: BiddingRound = BiddingRounds.Tern; break;
                case BiddingRounds.Tern: BiddingRound = BiddingRounds.River; break;
            }
        }
    }
}