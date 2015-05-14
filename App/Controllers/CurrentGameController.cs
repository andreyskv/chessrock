using App.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.Controllers
{


    public class ChessGame
    {
        public int Id { get; set; }
        public string Pgn { get; set; }
        public bool IsCurrent { get; set; }
        public string BoardOrientation { get; set; }
    }

    public class CurrentGameVM
    {
        public int id { get; set; }
        public string pgn { get; set; }
        public string boardorientation { get; set; }
    }

   [RoutePrefix("api/CurrentGame")]
    public class CurrentGameController : ApiController
    {

       IRavenRepository _repository;
       public CurrentGameController(IRavenRepository repository) 
       {
           _repository = repository;
       }

       public CurrentGameVM Get()
        {
            ChessGame currentGame = null;
            using (var session = _repository.Store.OpenSession())
            {
                var results = from game in session.Query<ChessGame>() where game.IsCurrent select game;
                currentGame = results.FirstOrDefault();
            }
                      
           if (currentGame != null)
                return new CurrentGameVM() { id = currentGame.Id, pgn = currentGame.Pgn, boardorientation = currentGame.BoardOrientation };

           return null;
            //return _repository.LoadEntity<CurrentGameVM>("CurrentGameVms/1");
        }

        // GET: api/CurrentGame/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CurrentGame
        public void Post([FromBody]CurrentGameVM value)
        {            
            ChessGame currentGame = null;
            using (var session = _repository.Store.OpenSession())
            {
                var results = from game in session.Query<ChessGame>() where game.IsCurrent select game;
                currentGame = results.FirstOrDefault();

                if (currentGame == null){
                    currentGame = new ChessGame();
                    session.Store(currentGame);
                }

                currentGame.BoardOrientation = value.boardorientation;
                currentGame.Pgn = value.pgn;
                currentGame.IsCurrent = true;

                //if (currentGame != null)
                //{
                //    currentGame.BoardOrientation = value.boardorientation;
                //    currentGame.Pgn = value.pgn;
                //    currentGame.IsCurrent = true;
                //}
                //else
                //{
                //    currentGame = new ChessGame() { Pgn = value.pgn, IsCurrent = true, BoardOrientation = value.boardorientation };
                //    session.Store(currentGame);
                //}
                session.SaveChanges();
            }
        }

        // PUT: api/CurrentGame/5
        public void Put(int id, [FromBody]dynamic value)
        {

            Debug.WriteLine("CurrentGameController:Put {0}", value);
        }

        // DELETE: api/CurrentGame/5
        public void Delete(int id)
        {
        }
    }
}
