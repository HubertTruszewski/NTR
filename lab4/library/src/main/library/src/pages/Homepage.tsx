import {useEffect, useState} from "react";
import {UserDTO} from "../dto/UserDTO";

export const Homepage = () => {
    const [user, setUser] = useState<UserDTO | null>(null);
    useEffect(() => {
        fetch("/api/user/whoami").then(data => data.json()).then(user => setUser(user));
    }, []);

    return <div className="text-center">
        <img src="book.png" alt="Book"/>
        <h1 className="display-4">Welcome</h1>
        <p>
            {user === null ? "Log in or create a new account." : <>Have a nice day, {user.username}!</>}
        </p>
    </div>
}
