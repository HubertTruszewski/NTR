package pl.edu.pw.elka.ntr.library.dto;

import pl.edu.pw.elka.ntr.library.model.User;

public record UserDTO(long userId, String username) {
    public static UserDTO of(final User user) {
        return new UserDTO(user.getId(), user.getUsername());
    }
}
