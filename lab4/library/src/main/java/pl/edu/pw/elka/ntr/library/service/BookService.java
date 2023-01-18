package pl.edu.pw.elka.ntr.library.service;

import java.util.List;
import java.util.Set;

import org.springframework.http.ResponseEntity;

import pl.edu.pw.elka.ntr.library.dto.BookActionDTO;
import pl.edu.pw.elka.ntr.library.model.Book;

public interface BookService {
    List<Book> getAllBooks();

    ResponseEntity<Book> getBook(long bookId);

    Set<Book> searchBook(String phrase);

    ResponseEntity<Void> reserveBook(BookActionDTO bookAction);

    ResponseEntity<Void> cancelReservation(BookActionDTO bookAction);

    ResponseEntity<Void> borrowBook(BookActionDTO bookAction);

    ResponseEntity<Void> returnBook(BookActionDTO bookAction);
}
