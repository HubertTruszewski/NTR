import {UserDTO} from "./UserDTO";

export interface BookDTO {
    id: number;
    title: string;
    author: string;
    date: number;
    reserved: string;
    leased: string;
    user: UserDTO;
    version: number;
}
