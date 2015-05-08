using Autofac;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace App.Game
{

    public interface IChessHub
    {
        void sendChessMoveToClient(string move);
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

        public override Task OnConnected()
        {
            // Do what you want here
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool val)
        {
            // Do what you want here
            return base.OnDisconnected(val);
        }

      //  public void SendChessMoveToClient(string move)
      //  {
            // Call the broadcastMessage method to update clients.
          //  Clients.All.broadcastMessage(move);
       // }
    }
}