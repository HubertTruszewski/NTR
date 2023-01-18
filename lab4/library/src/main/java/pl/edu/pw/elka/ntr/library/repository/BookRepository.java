package pl.edu.pw.elka.ntr.library.repository;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import pl.edu.pw.elka.ntr.library.model.Book;
import pl.edu.pw.elka.ntr.library.model.User;

@Repository
public interface BookRepository extends JpaRepository<Book, Long> {
    List<Book> findBooksByTitleContainingIgnoreCase(String phrase);

    List<Book> findBooksByAuthorContainingIgnoreCase(String phrase);

    Book getBookById(long bookId);

    boolean existsByUserAndLeasedGreaterThanEqual(User user, LocalDate date);

    List<Book> getBooksByUser(User user);
}
