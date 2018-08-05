import Player from "./Player";

export default interface Challenge {
    playerA : string,
    playerB : string,
    timestamp : Date,
    state : string
}