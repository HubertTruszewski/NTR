import {useState} from "react";
import {UserDTO} from "../dto/UserDTO";

export const ProfilePage = () => {

    const [user, setUser] = useState<UserDTO | null>(null);
    const [error, setError] = useState<boolean>(false);
    fetch("/api/user/whoami").then(data => data.json()).then(user => setUser(user));

    const handleDelete = () => {
        fetch("/api/user/delete", {method: "DELETE"}).then((response) => {
            if (response.ok) {
                window.location.href = "/perform_logout";
            } else {
                setError(true);
            }
        })
    }

    return <>
        {error && <div className="alert alert-danger alert-dismissible" role="alert">
            <div>Cannot delete account with borrowed books</div>
            <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>}
        <div className="col-6">
            <div className="row">Username: {user?.username}</div>
            {user?.username !== "librarian" &&
                <div className="row">
                    <button className="btn btn-danger" onClick={handleDelete}>Delete my account</button>
                </div>}
        </div>
    </>

}
