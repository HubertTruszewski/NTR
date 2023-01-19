import {useEffect, useState} from "react";
import {BookDTO} from "../dto/BookDTO";
import {UserDTO} from "../dto/UserDTO";
import {BookActionDTO} from "../dto/BookActionDTO";

export const Reservations = () => {
    const [user, setUser] = useState<UserDTO | null>();
    const [bookList, setBookList] = useState<BookDTO[]>([]);
    const [successMessage, setSuccessMessage] = useState<string>("");
    const [errorMessage, setErrorMessage] = useState<string>("");

    const fetchBooks = (): void => {
        fetch("/api/book/all").then(result => result.json())
            .then((bookList: BookDTO[]) => {
                    const today: string = new Date().toISOString().split("T")[0];
                    return bookList.filter(
                        book => book.reserved >= today && (user?.username === "librarian" || user?.username === book.user.username));
                }
            )
            .then(books => setBookList(books));
    }

    useEffect(() => {
        fetch("/api/user/whoami").then(result => result.json()).then(user => setUser(user));
    }, [])

    useEffect(() => {
        fetchBooks();
    }, [user])

    const handleCancelReservation = (bookId: number, version: number): void => {
        const requestBody: BookActionDTO = {bookId: bookId, version: version};
        fetch("/api/book/cancel",
            {method: "PUT", body: JSON.stringify(requestBody), headers: {'Content-Type': 'application/json'}})
            .then(response => {
                if (response.ok) {
                    setSuccessMessage(`You canceled reservation for book: ${bookList.find(book => book.id === bookId)?.title}`);
                } else if (response.status === 409) {
                    setErrorMessage("The book's record was changed by another user. Try again.");
                } else {
                    setErrorMessage("Server error, please try again later");
                }
            })
            .then(() => fetchBooks());
    }

    const handleBorrow = (bookId: number, version: number): void => {
        const requestBody: BookActionDTO = {bookId: bookId, version: version};
        fetch("/api/book/borrow",
            {method: "PUT", body: JSON.stringify(requestBody), headers: {'Content-Type': 'application/json'}})
            .then(response => {
                if (response.ok) {
                    setSuccessMessage(`You borrowed book: ${bookList.find(book => book.id === bookId)?.title}`);
                } else if (response.status === 409) {
                    setErrorMessage("The book's record you try to reserve was changed by another user. Try again.");
                } else {
                    setErrorMessage("Server error, please try again later");
                }
            })
            .then(() => fetchBooks());
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
        <table className="table table-striped table-hover">
            <thead>
            <tr>
                <th scope={"col"}>#</th>
                <th scope={"col"}>User</th>
                <th scope={"col"}>Book title</th>
                <th scope={"col"}>Author</th>
                <th scope={"col"}>End date</th>
                <th scope={"col"}>Action</th>
            </tr>
            </thead>
            <tbody>
            {bookList.map((book, index) => <tr key={book.id}>
                    <th scope={"row"}>{index + 1}</th>
                    <td>{book.user.username}</td>
                    <td>{book.title}</td>
                    <td>{book.author}</td>
                    <td>{book.reserved}</td>
                    <td>
                        {user?.username === "librarian" &&
                            <button className="btn btn-info"
                                    onClick={() => handleBorrow(book.id, book.version)}>Borrow</button>}
                        <button className="btn btn-danger"
                                onClick={() => handleCancelReservation(book.id, book.version)}>Cancel
                            reservation
                        </button>
                    </td>
                </tr>
            )}
            </tbody>
        </table>
    </>;
}
