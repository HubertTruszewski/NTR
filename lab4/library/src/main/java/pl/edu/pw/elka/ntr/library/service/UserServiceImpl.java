package pl.edu.pw.elka.ntr.library.service;

import java.time.LocalDate;

import javax.transaction.Transactional;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import pl.edu.pw.elka.ntr.library.dto.NewUserDTO;
import pl.edu.pw.elka.ntr.library.dto.UserDTO;
import pl.edu.pw.elka.ntr.library.model.AppUserDetails;
import pl.edu.pw.elka.ntr.library.model.User;
import pl.edu.pw.elka.ntr.library.repository.BookRepository;
import pl.edu.pw.elka.ntr.library.repository.UserRepository;

@Service
public class UserServiceImpl implements UserService {
    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final BookRepository bookRepository;

    public UserServiceImpl(final UserRepository userRepository, final PasswordEncoder passwordEncoder,
            final BookRepository bookRepository) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
        this.bookRepository = bookRepository;
    }

    @Override
    public UserDTO whoAmI() {
        final AppUserDetails user = (AppUserDetails) SecurityContextHolder.getContext()
                .getAuthentication()
                .getPrincipal();
        return new UserDTO(user.getUserId(), user.getUsername());
    }

    public ResponseEntity<UserDTO> register(NewUserDTO newUserDTO) {
        final boolean usernameExists = userRepository.existsByUsername(newUserDTO.username());
        if (usernameExists) {
            return ResponseEntity.badRequest().build();
        }
        final User user = new User(newUserDTO.username(), passwordEncoder.encode(newUserDTO.pwd()));
        userRepository.save(user);
        return ResponseEntity.status(HttpStatus.CREATED).body(UserDTO.of(user));
    }

    @Override
    @Transactional
    public ResponseEntity<Void> deleteUser() {
        final long userId = ((AppUserDetails) SecurityContextHolder.getContext()
                .getAuthentication()
                .getPrincipal()).getUserId();
        final User user = userRepository.getReferenceById(userId);
        final boolean hasLeasedBooks = bookRepository.existsByUserAndLeasedGreaterThanEqual(user, LocalDate.now());
        if (hasLeasedBooks) {
            return ResponseEntity.badRequest().build();
        }
        bookRepository.getBooksByUser(user).forEach(book -> {
            book.setUser(null);
            book.setReserved(null);
        });
        userRepository.delete(user);
        return ResponseEntity.ok().build();
    }
}
