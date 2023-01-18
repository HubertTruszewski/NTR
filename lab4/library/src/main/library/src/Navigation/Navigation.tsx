import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faHome} from "@fortawesome/free-solid-svg-icons";
import {Link} from "react-router-dom";
import {useEffect, useState} from "react";
import {UserDTO} from "../dto/UserDTO";

export const Navigation = () => {
    const [user, setUser] = useState<UserDTO | null>(null);

    useEffect(() => {
        fetch("/api/user/whoami").then(data => data.json()).then(user => setUser(user));
    }, [])

    return <nav className="navbar navbar-expand-lg nav-fill w-100 bg-light">
        <div className="container-fluid">
            <Link className="navbar-brand" to={"/"}>
                MyLibrary
            </Link>
            <button className="navbar-toggler" type="button" data-bs-toggle="collapse"
                    data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarSupportedContent">
                <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                    <li className="nav-item">
                        <a className="nav-link active" aria-current="page" href="/">
                            <FontAwesomeIcon icon={faHome}/>
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/books">Books</a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/reservations">Reservations</a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/borrowings">Borrowings</a>
                    </li>
                </ul>
                {user ? <>Hi, <Link to={"/profile"}>{user.username}</Link>
                    <a href="/perform_logout"
                       style={{marginLeft: "10px"}}>
                        <button className="btn btn-danger">Logout</button>
                    </a>
                </> : <>
                    <a href="/login" className="link-primary" style={{marginRight: "10px"}}>Login</a>
                    <Link to="/register" className="link-primary">Register</Link>
                </>}
            </div>
        </div>
    </nav>
}
