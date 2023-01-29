import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min'
import {useEffect, useState} from "react";
import {BookDTO} from "../dto/BookDTO";
import {UserDTO} from "../dto/UserDTO";
import {BookActionDTO} from "../dto/BookActionDTO";

export const Books = () => {
    const [user, setUser] = useState<UserDTO | null>();
    const [bookList, setBookList] = useState<BookDTO[]>([]);
    const [successMessage, setSuccessMessage] = useState<string>("");
    const [errorMessage, setErrorMessage] = useState<string>("");
    const [searchQuery, setSearchQuery] = useState<string>("");
    const [showOnlyAvailable, setShowOnlyAvailable] = useState<boolean>(false);

    const fetchBooks = () => {
        fetch("/api/book/all").then(result => result.json()).then(books => setBookList(books));
    }

    useEffect(() => {
        fetchBooks();
        fetch("/api/user/whoami").then(result => result.json()).then(user => setUser(user));
    }, [])

    const shouldShowBookButton = (book: BookDTO) => {
        const today: string = (new Date()).toISOString().split('T')[0];
        return book.user === null || (book.reserved !== null && today > book.reserved) || (book.leased !== null && today > book.leased);
    }

    const handleBookReserve = (bookId: number, version: number) => {
        const requestBody: BookActionDTO = {bookId: bookId, version: version};

        fetch("/api/book/reserve",
            {method: "PUT", body: JSON.stringify(requestBody), headers: {'Content-Type': 'application/json'}})
            .then(response => {
                if (response.ok) {
                    setSuccessMessage(`You reserved book: ${bookList.find(book => book.id === bookId)?.title}`);
                } else if (response.status === 409) {
                    setErrorMessage("The book's record you try to reserve was changed by another user. Try again.");
                } else {
                    setErrorMessage("Server error, please try again later");
                }
            })
            .then(() => fetchBooks());
    }

    const searchBooks = () => {
        if (searchQuery !== "") {
            fetch(`/api/book/search?phrase=${searchQuery}`)
                .then(response => response.json())
                .then(books => setBookList(books));
        } else {
            fetchBooks();
        }
    }

    return <>
        {successMessage.length > 0 && <div className="alert alert-success alert-dismissible" role="alert">
            <div>{successMessage}</div>
            <button type="button" className="btn-close" aria-label="Close"
                    onClick={() => setSuccessMessage("")}></button>
        </div>}
        {errorMessage.length > 0 && <div className="alert alert-danger alert-dismissible" role="alert">
            <div>{errorMessage}</div>
            <button type="button" className="btn-close" aria-label="Close"
                    onClick={() => setErrorMessage("")}></button>
        </div>}
        <div className="input-group">
            <input type="text" name="phrase" value={searchQuery} className="form-control" placeholder="Title"
                   aria-label="Title" onInput={event => setSearchQuery(event.currentTarget.value)}/>
            <button className="btn btn-outline-primary" onClick={searchBooks}>Search</button>
        </div>
        {user?.username === "librarian" &&
            <button type="button" className="btn btn-success" onClick={() => window.location.href = "/newBook"}>Add new
                book</button>}
        {user?.username !== "librarian" &&
            <div>
                <input className="form-check-input" type="checkbox" checked={showOnlyAvailable}
                       onClick={() => setShowOnlyAvailable(prevState => !prevState)} id="flexCheckDefault"/>
                <label className="form-check-label" htmlFor="flexCheckDefault">
                    Show only available books
                </label>
            </div>}
        <table className="table table-striped table-hover">
            <thead>
            <tr>
                <th scope={"col"}>#</th>
                <th scope={"col"}>Title</th>
                <th scope={"col"}>Author</th>
                <th scope={"col"}>Date</th>
                {user?.username !== "librarian" && <th scope={"col"}>Action</th>}
            </tr>
            </thead>
            <tbody>
            {bookList.filter(book => {
                    if (showOnlyAvailable) {
                        return shouldShowBookButton(book);
                    }
                    return true;
                })
                .map((book, index) => <tr key={book.id}>
                        <th scope={"row"}>{index + 1}</th>
                        <td>{book.title}</td>
                        <td>{book.author}</td>
                        <td>{book.date}</td>
                        {user?.username !== "librarian" && <td>
                            {shouldShowBookButton(book) &&
                                <button className="btn btn-info"
                                        onClick={() => handleBookReserve(book.id, book.version)}>
                                    Book
                                </button>}
                        </td>}
                    </tr>
                )}
            </tbody>
        </table>
    </>
}
