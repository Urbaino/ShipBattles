using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using se.Urbaino.ShipBattles.Domain.GameItems;
using se.Urbaino.ShipBattles.Domain.Games;
using se.Urbaino.ShipBattles.Domain.Repositories;

namespace se.Urbaino.ShipBattles.Domain.Services
{
    public class GameManagerService : IGameManagerService
    {
        private readonly IGameRepository _repository;

        public GameManagerService(IGameRepository repository)
        {
            _repository = repository;
        }

        Game IGameManagerService.NewGame(string playerA, string playerB)
        {
            var game = Game.NewGame(playerA, playerB);
            _repository.Update(game);
            return game;
        }

        Game IGameManagerService.GetGame(string gameId, string playerId)
        {
            return _repository.GetGame(gameId, playerId);
        }

        IEnumerable<Game> IGameManagerService.GetGames(string playerId)
        {
            return _repository.GetListOfGames(playerId);
        }

        Game IGameManagerService.Fire(string gameId, string playerId, Coordinate coordinates)
        {
            var game = _repository.GetGame(gameId, playerId);

            if (playerId == game.PlayerA)
                game.PlayerATakeShot(coordinates);
            else
                game.PlayerBTakeShot(coordinates);
            game.Timestamp = DateTime.Now;

            _repository.Update(game);
            return game;
        }

        Game IGameManagerService.PlaceShip(string gameId, string playerId, Ship ship)
        {
            var game = _repository.GetGame(gameId, playerId);

            if (playerId == game.PlayerA)
                game.PlayerAPlaceShip(ship);
            else
                game.PlayerBPlaceShip(ship);
            game.Timestamp = DateTime.Now;

            _repository.Update(game);
            return game;
        }
    }
}