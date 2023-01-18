package pl.edu.pw.elka.ntr.library.controller;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import pl.edu.pw.elka.ntr.library.dto.NewUserDTO;
import pl.edu.pw.elka.ntr.library.dto.UserDTO;
import pl.edu.pw.elka.ntr.library.service.UserService;

@RestController
@RequestMapping("/api/user")
public class UserController {
    private final UserService userService;

    public UserController(final UserService userService) {
        this.userService = userService;
    }

    @GetMapping("/whoami")
    public UserDTO whoAmI() {
        return userService.whoAmI();
    }

    @PostMapping("/register")
    public ResponseEntity<UserDTO> register(@RequestBody NewUserDTO newUserDTO) {
        return userService.register(newUserDTO);
    }

    @DeleteMapping("/delete")
    public ResponseEntity<Void> deleteUser() {
        return userService.deleteUser();
    }
}
