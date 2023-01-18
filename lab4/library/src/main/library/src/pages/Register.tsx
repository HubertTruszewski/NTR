import React, {ChangeEvent, useState} from "react";
import {NewUserDTO} from "../dto/NewUserDTO";

export const Register = () => {
    const [newUserDTO, setNewUserDTO] = useState<NewUserDTO>({pwd: "", username: ""})
    const [errorMessage, setErrorMessage] = useState<string>("");
    const [successMessage, setSuccessMessage] = useState<boolean>(false);

    const handleInput = (event: ChangeEvent<HTMLInputElement>) => {
        setNewUserDTO({...newUserDTO, [event.target.name]: event.target.value})
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        fetch("/api/user/register",
            {method: "POST", body: JSON.stringify(newUserDTO), headers: {'Content-Type': 'application/json'}})
            .then(data => {
                    if (data.status === 201) {
                        setSuccessMessage(true);
                        setErrorMessage("");
                    } else if (data.status === 400) {
                        setErrorMessage("Username is already taken! Please use another one")
                    } else {
                        setErrorMessage("Server error. Try again later")
                    }
                }
            )
    }

    return <div className="card col-4 m-md-auto ">
        <div className="card-body">
            <h3 className="card-title mb-3">Register form</h3>
            <form onSubmit={handleSubmit}>
                <div className="form-floating mb-2">
                    <input type="text" className="form-control" id="username" name="username" placeholder="Username"
                           onInput={handleInput}/>
                    <label htmlFor="username">Username</label>
                </div>
                <div className="form-floating mb-2">
                    <input type="password" className="form-control" id="pwd" name="pwd" placeholder="Password"
                           onInput={handleInput}/>
                    <label htmlFor="pwd">Password</label>
                </div>
                <button type="submit" className="btn btn-primary">Register</button>
                {successMessage &&
                    <div className="alert alert-success d-flex align-items-start alert-dismissible mt-2" role="alert">
                        <div>
                            Success! You can go to <a href="/login">login page</a>
                            <br/>
                            <a className="fw-bold link-success" href="/login">Login</a>
                        </div>
                        <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>}
                {errorMessage.length > 0 &&
                    <div className="alert alert-danger d-flex align-items-start alert-dismissible mt-2" role="alert">
                        {errorMessage}
                        <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                }
            </form>
        </div>
    </div>
}
