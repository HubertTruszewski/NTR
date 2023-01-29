import React, {ChangeEvent, useState} from "react";
import {NewBookDTO} from "../dto/NewBookDTO";

export const NewBookPage = () => {
    const [newBookDTO, setNewBookDTO] = useState<NewBookDTO>({author: "", date: "", publisher: "", title: ""});
    const [successMessage, setSuccessMessage] = useState<string>("");
    const [errorMessage, setErrorMessage] = useState<string>("");

    const handleInput = (event: ChangeEvent<HTMLInputElement>) => {
        setNewBookDTO({...newBookDTO, [event.target.name]: event.target.value})
    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        fetch("/api/book/add",
            {method: "POST", body: JSON.stringify(newBookDTO), headers: {'Content-Type': 'application/json'}})
            .then(data => {
                    if (data.ok) {
                        setSuccessMessage("Book added");
                        setNewBookDTO({author: "", date: "", publisher: "", title: ""})
                    } else {
                        setErrorMessage("Server error. Try again later")
                    }
                }
            )
    }

    return <div className="card col-4 m-md-auto ">
        <div className="card-body">
            <h3 className="card-title mb-3">New book</h3>
            <form onSubmit={handleSubmit}>
                <div className="form-floating mb-2">
                    <input type="text" className="form-control" id="title" name="title" placeholder="Title"
                           onInput={handleInput} value={newBookDTO.title}/>
                    <label htmlFor="title">Title</label>
                </div>
                <div className="form-floating mb-2">
                    <input type="text" className="form-control" id="author" name="author" placeholder="Author"
                           onInput={handleInput} value={newBookDTO.author}/>
                    <label htmlFor="author">Author</label>
                </div>
                <div className="form-floating mb-2">
                    <input type="text" className="form-control" id="date" name="date" placeholder="Date"
                           onInput={handleInput} value={newBookDTO.date}/>
                    <label htmlFor="date">Date</label>
                </div>
                <div className="form-floating mb-2">
                    <input type="text" className="form-control" id="publisher" name="publisher" placeholder="Publisher"
                           onInput={handleInput} value={newBookDTO.publisher}/>
                    <label htmlFor="publisher">Publisher</label>
                </div>
                <button type="submit" className="btn btn-primary">Add</button>
                {successMessage.length > 0 &&
                    <div className="alert alert-success d-flex align-items-start alert-dismissible mt-2" role="alert">
                        {successMessage}
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
