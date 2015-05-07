using Autofac;
using Microsoft.AspNet.SignalR;

namespace App.Game
{

    public interface IChessHub
    {
        void SendChessMoveToClient(string move);
    }

    public class ChessHub : Hub<IChessHub>
    {
        private IChessGameManager _game;
        public ChessHub(IChessGameManager game)
        {
            _game = game;
        }

        public void StartGame()
        {
            _game.StartNewGame();
        }

        public void Fen(string fen)
        {
            _game.PassClientMoveToEngine(fen);
        }


      //  public void SendChessMoveToClient(string move)
      //  {
            // Call the broadcastMessage method to update clients.
          //  Clients.All.broadcastMessage(move);
       // }
    }
}