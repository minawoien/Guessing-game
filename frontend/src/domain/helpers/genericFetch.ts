export type fetchResponse<T> = {
    body: T;
    status: number;
}

//used for gets without a response body
export async function Get(route: string): Promise<number> {
    let response = await fetch(route, {
        method: "GET",
        headers: {
            'Content-Type': 'application/json'
        },
    });
    return response.status;
}

//used for gets where you receive a response body
export async function GetWithBody<T>(route: string): Promise<fetchResponse<T>> {
    let response = await fetch(route, {
        method: "GET",
        headers: {
            'Content-Type': 'application/json'
        },
    });
    let body = await response.json();
    return {body: body, status: response.status};
}

//used for posts without a response body
export async function Post<T>(data: T, route: string): Promise<number> {
    let response = await fetch(route, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    return response.status;
}

//used for posts where you receive a response body
export async function PostWithBody<TIn, TOut>(data: TIn, route: string): Promise<fetchResponse<TOut>> {
    let response = await fetch(route, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    let body = await response.json();
    return {body: body, status: response.status};
}