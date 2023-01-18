package pl.edu.pw.elka.ntr.library.controller;

import java.util.List;
import java.util.Set;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import pl.edu.pw.elka.ntr.library.dto.BookActionDTO;
import pl.edu.pw.elka.ntr.library.model.Book;
import pl.edu.pw.elka.ntr.library.service.BookService;

@RestController
@RequestMapping("/api/book")
public class BookController {

    private final BookService bookService;

    public BookController(final BookService bookService) {
        this.bookService = bookService;
    }

    @GetMapping("/all")
    public List<Book> getAllBooks() {
        return bookService.getAllBooks();
    }

    @GetMapping("/{bookId}")
    public ResponseEntity<Book> getBook(@PathVariable long bookId) {
        return bookService.getBook(bookId);
    }

    @GetMapping("/search")
    public Set<Book> searchBook(final @RequestParam String phrase) {
        return bookService.searchBook(phrase);
    }

    @PutMapping("/reserve")
    public ResponseEntity<Void> reserveBook(@RequestBody final BookActionDTO bookActionDTO) {
        return bookService.reserveBook(bookActionDTO);
    }

    @PutMapping("/cancel")
    public ResponseEntity<Void> cancelReservation(@RequestBody final BookActionDTO bookActionDTO) {
        return bookService.cancelReservation(bookActionDTO);
    }

    @PutMapping("/borrow")
    public ResponseEntity<Void> borrowBook(@RequestBody final BookActionDTO bookActionDTO) {
        return bookService.borrowBook(bookActionDTO);
    }

    @PutMapping("/return")
    public ResponseEntity<Void> returnBook(@RequestBody final BookActionDTO bookActionDTO) {
        return bookService.returnBook(bookActionDTO);
    }
}
