import {gameRouteResponse} from "./gameRouteResponse"
import {fetchResponse, Get, GetWithBody, PostWithBody} from "../helpers/genericFetch"

export type Lobby = {
    id: number;
    userNames: Array<string>;
    gameType: number;
    hostRole: number;
    gameId: number;
}
export type Lobbies = {
    Lobby: Array<Lobby>;
}
export type LobbyData = {
    Id: number;
    Type: number;
    Role: number;
}
export type LobbyStatus = {
    lobbyId: number;
    gameId: number
}

export type LobbyChoices = {
    gametype: number
    game: string
    role: number,
    gameId: number,
    lobbyId: number,

}


export async function postLobby(postLobby: LobbyData): Promise<fetchResponse<gameRouteResponse<LobbyStatus>>> {
    let response = await PostWithBody<LobbyData, gameRouteResponse<LobbyStatus>>(postLobby, "/Lobby/Join");
    return response
}

export async function getLobby(): Promise<fetchResponse<gameRouteResponse<Lobby>>> {
    let response = await GetWithBody<gameRouteResponse<Lobby>>("/Lobby/");
    return response
}

export async function getLobbiesByType(type: number): Promise<fetchResponse<gameRouteResponse<Lobbies>>> {
    let response = await GetWithBody<gameRouteResponse<Lobbies>>("/Lobby/Type/" + type)
    return response
}

export async function startGame(): Promise<number> {
    let response = await Get("/Lobby/Start/");
    return response
}
