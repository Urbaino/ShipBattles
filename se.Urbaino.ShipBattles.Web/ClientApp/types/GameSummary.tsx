export default interface GameSummary {
    id : string,
    opponentName : string,
    timestamp: Date,
    resultIsVictory? : boolean
}