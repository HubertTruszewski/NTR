package pl.edu.pw.elka.ntr.library.service;

import java.time.LocalDate;
import java.util.Collection;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

import javax.transaction.Transactional;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

import pl.edu.pw.elka.ntr.library.dto.BookActionDTO;
import pl.edu.pw.elka.ntr.library.model.AppUserDetails;
import pl.edu.pw.elka.ntr.library.model.Book;
import pl.edu.pw.elka.ntr.library.model.User;
import pl.edu.pw.elka.ntr.library.repository.BookRepository;
import pl.edu.pw.elka.ntr.library.repository.UserRepository;

@Service
public class BookServiceImpl implements BookService {

    private final BookRepository bookRepository;
    private final UserRepository userRepository;

    public BookServiceImpl(final BookRepository bookRepository, final UserRepository userRepository) {
        this.bookRepository = bookRepository;
        this.userRepository = userRepository;
    }

    @Override
    public List<Book> getAllBooks() {
        return bookRepository.findAll();
    }

    @Override
    public ResponseEntity<Book> getBook(final long bookId) {
        final Book book = bookRepository.getBookById(bookId);
        return ResponseEntity.ok().eTag(String.valueOf(book.getVersion())).body(book);
    }

    @Override
    public Set<Book> searchBook(final String phrase) {
        return Stream.of(bookRepository.findBooksByTitleContainingIgnoreCase(phrase),
                        bookRepository.findBooksByAuthorContainingIgnoreCase(phrase))
                .flatMap(Collection::stream)
                .collect(Collectors.toUnmodifiableSet());
    }

    @Override
    @Transactional
    public ResponseEntity<Void> reserveBook(final BookActionDTO bookAction) {
        final AppUserDetails user = (AppUserDetails) SecurityContextHolder.getContext()
                .getAuthentication()
                .getPrincipal();
        final User userRef = userRepository.getReferenceById(user.getUserId());
        final Book book = bookRepository.getBookById(bookAction.bookId());
        if (book.getVersion() != bookAction.version()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }
        book.setUser(userRef);
        book.setReserved(LocalDate.now().plusDays(1));
        book.setLeased(null);
        return ResponseEntity.ok().build();
    }

    @Override
    @Transactional
    public ResponseEntity<Void> cancelReservation(final BookActionDTO bookAction) {
        final Book book = bookRepository.getBookById(bookAction.bookId());
        if (book.getVersion() != bookAction.version()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }
        book.setReserved(null);
        book.setUser(null);
        return ResponseEntity.ok().build();
    }

    @Override
    @Transactional
    public ResponseEntity<Void> borrowBook(final BookActionDTO bookAction) {
        final Book book = bookRepository.getBookById(bookAction.bookId());
        if (book.getVersion() != bookAction.version()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }
        book.setReserved(null);
        book.setLeased(LocalDate.now().plusDays(14));
        return ResponseEntity.ok().build();
    }

    @Override
    @Transactional
    public ResponseEntity<Void> returnBook(final BookActionDTO bookAction) {
        final Book book = bookRepository.getBookById(bookAction.bookId());
        if (book.getVersion() != bookAction.version()) {
            return ResponseEntity.status(HttpStatus.CONFLICT).build();
        }
        book.setLeased(null);
        book.setUser(null);
        return ResponseEntity.ok().build();
    }
}
