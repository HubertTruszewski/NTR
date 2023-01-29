package pl.edu.pw.elka.ntr.library.model;

import java.time.LocalDate;
import java.util.Objects;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;
import javax.persistence.Version;

@Entity(name = "BOOK")
@Table(name = "BOOKS")
public class Book {
    @Id
    @Column(name = "BOOK_ID")
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    @Column(nullable = false)
    private String author;
    @Column(nullable = false)
    private String title;
    private int date;
    @Column(nullable = false)
    private String publisher;
    @ManyToOne
    @JoinColumn(name = "USER_ID")
    private User user;
    private LocalDate reserved;
    private LocalDate leased;

    @Version
    private int version;

    public Book() {
    }

    public Book(final String author, final String title, final int date, final String publisher) {
        this.author = author;
        this.title = title;
        this.date = date;
        this.publisher = publisher;
    }

    public Long getId() {
        return id;
    }

    public String getAuthor() {
        return author;
    }

    public String getTitle() {
        return title;
    }

    public int getDate() {
        return date;
    }

    public String getPublisher() {
        return publisher;
    }

    public User getUser() {
        return user;
    }

    public LocalDate getReserved() {
        return reserved;
    }

    public LocalDate getLeased() {
        return leased;
    }

    public void setId(final Long id) {
        this.id = id;
    }

    public void setAuthor(final String author) {
        this.author = author;
    }

    public void setTitle(final String title) {
        this.title = title;
    }

    public void setDate(final int date) {
        this.date = date;
    }

    public void setPublisher(final String publisher) {
        this.publisher = publisher;
    }

    public void setUser(final User user) {
        this.user = user;
    }

    public void setReserved(final LocalDate reserved) {
        this.reserved = reserved;
    }

    public void setLeased(final LocalDate leased) {
        this.leased = leased;
    }

    public void setVersion(final int version) {
        this.version = version;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (o == null || getClass() != o.getClass()) {
            return false;
        }
        Book book = (Book) o;
        return id == book.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }

    public int getVersion() {
        return version;
    }
}
