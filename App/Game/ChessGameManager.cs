using App.Game;
using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace App.Game
{
    
    public class ChessGameManager : IChessGameManager
    {
        private UciEngineManager _uciEngine = new UciEngineManager();               
        IHubConnectionContext<dynamic> Clients { get; set; }
                
        //public ChessGameManager(IHubConnectionContext<IChessHub> clients)
        public ChessGameManager()
        {
            Clients = GlobalHost.ConnectionManager.GetHubContext<ChessHub>().Clients;
            //_uciEngine.LoadEngine("stockfish.exe");
              _uciEngine.LoadEngine("murka2.exe");
            //_uciEngine.LoadEngine("HiarcsX50UCI.exe");
            
            _uciEngine.BestMoveFoundEvent += bestmove_Event;
           // StartNewGame();
        }
        
        public void StartNewGame()
        {
            _uciEngine.SendEngineCommand("uci");
            _uciEngine.SendEngineCommand("ucinewgame");          
        }

        public void PassClientMoveToEngine(string fen)
        {
            _uciEngine.StartSearchingMove(fen, 400);   
        }

        public void bestmove_Event(object sender, EventArgs e)
        {      

       //     .move({ from: 'h7', <- where the 'move' is a move object (additional
       //*         to :'h8',      fields are ignored)
       //*         promotion: 'q',
       //*      })
            var move = ((ChessMoveEventDataArgs)e).BestMove;

            var moveClientFormat = new
            {
                from = move.Substring(0, 2),
                to = move.Substring(2, 2),
                promotion = move.Length == 5 ? move.Substring(4, 1) : ""
            };

            Clients.All.sendChessMoveToClient(moveClientFormat);
        }

    }
}