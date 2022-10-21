import {Post} from "../helpers/genericFetch";

export interface ILoginData {
    username: string;
    passwd: string;
    error: Array<string>;

    fieldsNotEmpty(): void;
}

export class LoginData {
    username: string;
    passwd: string;
    error: Array<string>;

    fieldsNotEmpty(): void {
        this.error = new Array<string>();
        if (this.username.length < 1) {
            this.error.push("Username is required");
        }

        if (this.passwd.length < 1) {
            this.error.push("Password is required!");
        }
    }

    constructor() {
        this.username = "";
        this.passwd = "";
        this.error = new Array<string>();
    }

}

export interface IUser {
    userName: string;
    password: string;

    loginUserRequest(): Promise<number>;
}

export class User {
    userName: string;
    password: string;

    async loginUserRequest(): Promise<number> {
        return Post<IUser>(this, "/Login");
    }

    constructor(uname: string, passwd: string) {
        this.userName = uname;
        this.password = passwd;
    }
}
