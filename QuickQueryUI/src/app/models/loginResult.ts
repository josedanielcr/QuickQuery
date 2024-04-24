import { User } from "./User";

export class LoginResult {
    public token : string | undefined;
    public refreshToken : string | undefined;
    public user : User | undefined;
}