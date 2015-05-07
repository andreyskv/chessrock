using System;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace App.Game
{

    public class ChessMoveEventDataArgs : EventArgs
    {
        public string BestMove { get; set; }
    }
        

    public class UciEngineManager
    {      
        public event EventHandler BestMoveFoundEvent;
        private Process _engine;
   
        public bool IsEngineAlive()
        {
            return _engine != null && !_engine.HasExited;
        }

        public void LoadEngine(string engineName)
        {
            try
            {
                var procInfo = new ProcessStartInfo()
                {                    
                    CreateNoWindow = true,
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Engines", engineName),
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                _engine = new Process() { StartInfo = procInfo };         
                _engine.OutputDataReceived += new DataReceivedEventHandler(Engine_dataReceived);
                if (_engine.Start())
                {
                    _engine.StandardInput.AutoFlush = true;
                }
            }
            catch(Exception e) 
            {
                Debug.WriteLine("Could not load engine {0}", e.StackTrace);
            }                        
        }

        private string TryFindBestMove(string data)
        {
            if (data.StartsWith("bestmove"))
            {
                var values = data.Split(' ');
                if (values.Length > 0)
                    return values[1];
            }            
            return null;
        }

        private void Engine_dataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Debug.WriteLine("UCI::{0}::{1}", ((Process)sender).Id, e.Data);                
                var move = TryFindBestMove(e.Data);
                if (!string.IsNullOrEmpty(move))
                    BestMoveFoundEvent(this, new ChessMoveEventDataArgs() { BestMove = move });
            }
        }
        
        public void SendEngineCommand(string command)
        {        
            try
            {
                if (IsEngineAlive())             
                   _engine.StandardInput.Write(command + "\n");

            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not send command {0}", e.StackTrace);
            }
        }

        public void ReadEngineOutput()
        {         
            try
            {
                if (IsEngineAlive())     
                    _engine.BeginOutputReadLine();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not read engine output {0}", e.StackTrace);
            }         
        }

        public void TerminateEngine()
        {         
            try
            {
                if (IsEngineAlive())
                {
                    _engine.WaitForExit();
                    _engine.Close();
                    _engine = null;
                }                    
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not terminate engine {0}", e.StackTrace);
            }
        }

        public void StartSearchingMove(string fen, int movetime)
        {
            SendEngineCommand(String.Format("position fen {0}", fen));
            SendEngineCommand(String.Format("go movetime {0}", movetime));            
            ReadEngineOutput();
        }

        public void TestEngine()
        {
            var fen = "rn1qkbnr/pppbpppp/8/3p4/Q2P4/2P5/PP2PPPP/RNB1KBNR b KQkq - 2 3";
            var movetime = 2000;

            LoadEngine("stockfish.exe");                       
            SendEngineCommand("uci");
            SendEngineCommand("ucinewgame");

            BestMoveFoundEvent+= bestmove_Event;
            StartSearchingMove(fen, movetime);           
        }

        void bestmove_Event(object sender, EventArgs e)
        {
            Debug.WriteLine(String.Format("The best move was found. It is {0}", ((ChessMoveEventDataArgs)e).BestMove));
            SendEngineCommand("quit");
            TerminateEngine();
        }

        //uci
        //ucinewgame
        //position fen {pos}
        //go movetime {time ms}
    }
}