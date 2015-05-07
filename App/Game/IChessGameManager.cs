using System;

namespace App.Game
{
    public interface IChessGameManager
    {
        void StartNewGame();
        void PassClientMoveToEngine(string fen);
        void bestmove_Event(object sender, EventArgs e);
    }
}
