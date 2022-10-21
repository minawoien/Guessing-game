import {gameRouteResponse} from "./gameRouteResponse"
import {fetchResponse, Get, GetWithBody, Post} from "../helpers/genericFetch"
import {dimension} from "./clickedPixel"

export type Game = {
    id: number;
    status: number;
    unlockedFragments: number;
    guesses: string[];
    role: number;
    imageLabel: string;
    oracle: boolean;
    type: number;
    winner: string;

}
export type Guess = {
    Value: string
}

export type Fragment = {
    fileName: string;
    unlocked: boolean;
}


export async function getGame(): Promise<fetchResponse<gameRouteResponse<Game>>> {
    let response = await GetWithBody<gameRouteResponse<Game>>("/Game/");
    return response
}

export async function getImage(gameId: number): Promise<fetchResponse<gameRouteResponse<Fragment[]>>> {
    let response = await GetWithBody<gameRouteResponse<Fragment[]>>("/Game/Image/" + gameId);
    return response;
}

export async function unlocktFragment(): Promise<number> {
    let response = await Get("/Game/Unlock/");
    return response;
}

export async function postGuess(guess: Guess): Promise<number> {
    let response = await Post<Guess>(guess, "/Game/Guess/");
    return response
}

export async function proposeFragment(coordinates: dimension): Promise<number> {
    let response = await Post<dimension>(coordinates, "/Game/Propose/");
    return response
}
