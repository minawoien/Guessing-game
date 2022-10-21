import * as Gen from "../helpers/genericFetch";


export async function logoutUserRequest(): Promise<number> {
    return (await Gen.Get("/logout"));

}

