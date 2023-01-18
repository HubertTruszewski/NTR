package pl.edu.pw.elka.ntr.library.service;

import org.springframework.http.ResponseEntity;

import pl.edu.pw.elka.ntr.library.dto.NewUserDTO;
import pl.edu.pw.elka.ntr.library.dto.UserDTO;

public interface UserService {
    UserDTO whoAmI();

    ResponseEntity<UserDTO> register(NewUserDTO newUserDTO);

    ResponseEntity<Void> deleteUser();
}
