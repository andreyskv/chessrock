using App.Game;
using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace App.Game
{
    
    public class ChessGameManager : IChessGameManager
    {
        private UciEngineManager _uciEngine = new UciEngineManager();       
        IHubConnectionContext<IChessHub> _clients;

        public ChessGameManager(IHubConnectionContext<IChessHub> clients)
        {
            _clients = clients;
            _uciEngine.LoadEngine("stockfish.exe");
            _uciEngine.BestMoveFoundEvent += bestmove_Event;
        }
        
        public void StartNewGame()
        {
            _uciEngine.SendEngineCommand("uci");
            _uciEngine.SendEngineCommand("ucinewgame");
        }

        public void PassClientMoveToEngine(string fen)
        {
            _uciEngine.StartSearchingMove(fen, 2000);   
        }

        public void bestmove_Event(object sender, EventArgs e)
        {
            //var context = GlobalHost.ConnectionManager.GetHubContext<ChessHub, IChessHub>();
            _clients.All.SendChessMoveToClient(((ChessMoveEventDataArgs)e).BestMove);
        }

    }
}