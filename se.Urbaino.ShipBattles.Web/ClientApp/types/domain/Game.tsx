import { BoardState } from "./Board";

export default interface GameSummary {
    id: string,
    playerA: string,
    playerb: string,
    boardA: BoardState,
    boardB: BoardState,
    state: GameState
}

enum GameState {
    PlayerAPlaceShip4,
    PlayerAPlaceShip3,
    PlayerAPlaceShip2,
    PlayerAPlaceShip1,

    PlayerBPlaceShip4,
    PlayerBPlaceShip3,
    PlayerBPlaceShip2,
    PlayerBPlaceShip1,

    PlayerAFire,
    PlayerBFire,

    PlayerAWin,
    PlayerBWin
}
