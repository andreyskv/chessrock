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

        public class GameVM
        {
            public int id { get; set; }
            public string pgn { get; set; }
            public string boardorientation { get; set; }
        }

        [RoutePrefix("api/PgnGame")]
        public class CurrentGameController : ApiController
        {

        IRavenRepository _repository;
        public CurrentGameController(IRavenRepository repository) 
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("Current")]
        public IHttpActionResult GetCurrent()
        {
            ChessGame currentGame = null;
            using (var session = _repository.Store.OpenSession())
            {
                var results = from game in session.Query<ChessGame>() where game.IsCurrent select game;
                currentGame = results.FirstOrDefault();
            }
                      
            if (currentGame != null)
                return Ok(new GameVM() { id = currentGame.Id, pgn = currentGame.Pgn, boardorientation = currentGame.BoardOrientation });

            return NotFound();            
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            List<ChessGame> results = null;
            using (var session = _repository.Store.OpenSession())
            {
                results = (from game in session.Query<ChessGame>() select game).ToList();
            }

            if (results != null)
                return Ok(results);

            return NotFound();
        }


        // GET: api/CurrentGame/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CurrentGame
        [Route("Current")]
        public void PostCurrent([FromBody]GameVM value)
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
                session.SaveChanges();
            }
        }

        [HttpPost]
        [Route("")]
        public void Post([FromBody]GameVM value)
        {
            
            using (var session = _repository.Store.OpenSession())
            {
                var game = new ChessGame() 
                { 
                    BoardOrientation = value.boardorientation,
                    Pgn = value.pgn,
                    IsCurrent = false
                };
                session.Store(game);                               
                session.SaveChanges();
            }
        }

        // PUT: api/CurrentGame/5
        public void Put(int id, [FromBody]dynamic value)
        {

            Debug.WriteLine("CurrentGameController:Put {0}", value);
        }

        // DELETE: api/CurrentGame/5
        [HttpDelete]
        [Route("{id:int}")]
        public void Delete(int id)
        {
            using (var session = _repository.Store.OpenSession())
            {
                var game = session.Load<ChessGame>(id);
                session.Delete(game);
                session.SaveChanges();
            }
        }
    }
}
