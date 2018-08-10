import { BoardState } from "./Board";
import { GameState } from "./GameState";

export default interface Game {
    id: string,
    playerA: string,
    playerb: string,
    boardA: BoardState,
    boardB: BoardState,
    state: GameState
}

