import {fetchResponse, PostWithBody} from "../helpers/genericFetch";
import {authRouteResponse} from "../auth/authRouteResponse";

export interface IRegisterData {
    username: string;
    password: string;
    repassword: string;
    error: Array<string>;

    passwordRequirementsCheck(): void;
}

export class RegisterData {
    username: string;
    password: string;
    repassword: string;
    error: Array<string>;

    passwordRequirementsCheck(): void {
        this.error = new Array<string>();
        if (this.password !== this.repassword) {
            this.error.push("Passwords do not match");
        }
        if (this.password.length < 8) {
            this.error.push("Password should be 8 characters or longer!");
        }
    }

    constructor() {
        this.username = "";
        this.password = "";
        this.repassword = "";
        this.error = new Array<string>();
    }

}

export interface IUser {
    userName: string;
    password: string;

    registerUserRequest(): Promise<fetchResponse<authRouteResponse<string>>>;
}

export class User {
    userName: string;
    password: string;

    async registerUserRequest(): Promise<fetchResponse<authRouteResponse<string>>> {
        return await PostWithBody<IUser, authRouteResponse<string>>(this, "/Register");
    }

    constructor(uname: string, passwd: string) {
        this.userName = uname;
        this.password = passwd;
    }
}