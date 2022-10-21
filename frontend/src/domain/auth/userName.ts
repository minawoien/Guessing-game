import {GetWithBody} from "../helpers/genericFetch";
import {authRouteResponse} from "./authRouteResponse";

export async function getUsername(): Promise<string> {
    let response = await GetWithBody<authRouteResponse<string>>("/UserName");
    return response.body.data;
}
