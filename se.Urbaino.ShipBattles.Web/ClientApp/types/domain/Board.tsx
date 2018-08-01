import Ship from "./Ship";
import Shot from "./Shot";

export default interface Board {
    Ships : Array<Ship>,
    Shots :  Array<Shot>,
    Width : number,
    Height : number
}