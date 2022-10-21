import {fetchResponse, GetWithBody} from "../helpers/genericFetch"
import {resultRouteResponse} from "./resultRouteResponse"


// Leaderboard is gonna show Rank, Player/s, Score, Guesses
export type Leaderboard = {
    userName: string;
    score: number;
}
export type Leaderboards = {
    Leaderboards: Array<Leaderboard>;
}

// Leaderboard is gonna show Rank, Player/s, Score, Guesses
export type TeamLeaderboard = {
    userNames: Array<string>;
    score: number;
}
export type TeamLeaderboards = {
    TeamLeaderboards: Array<TeamLeaderboard>;
}


//RecentGame is gonna show, Date, GameType, Guesser, Proposer
export type RecentGame = {
    userNames: Array<string>;
    gameType: string;
    startTime: number;


}
export type RecentGames = {
    RecentGames: Array<RecentGame>;
}


//Making a fetch function to get the leaderboards and recentgames 

export async function getLeaderboards(): Promise<fetchResponse<resultRouteResponse<Leaderboards>>> {
    let response = await GetWithBody<resultRouteResponse<Leaderboards>>("/Leaderboard/Single")
    return response
}

export async function getTeamLeaderboards(): Promise<fetchResponse<resultRouteResponse<TeamLeaderboards>>> {
    let response = await GetWithBody<resultRouteResponse<TeamLeaderboards>>("/Leaderboard/Team")
    return response
}

export async function getRecentGames(): Promise<fetchResponse<resultRouteResponse<RecentGames>>> {
    let response = await GetWithBody<resultRouteResponse<RecentGames>>("/RecentGames")
    return response
}