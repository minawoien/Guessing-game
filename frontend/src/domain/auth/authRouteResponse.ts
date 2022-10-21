export type authRouteResponse<T> = {
    data: T;
    errors: string[];
}