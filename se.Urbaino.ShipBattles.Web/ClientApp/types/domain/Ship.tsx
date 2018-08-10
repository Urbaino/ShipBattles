import Coordinate from "./Coordinate";

export default interface Ship {
    origin : Coordinate,
    length : number,
    heading : Direction
}

export enum Direction{
    North,
    East,
    West,
    South
}