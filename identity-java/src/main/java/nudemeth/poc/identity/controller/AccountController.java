package nudemeth.poc.identity.controller;

import java.util.UUID;

import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;

import nudemeth.poc.identity.model.UserModel;
import nudemeth.poc.identity.service.AccountService;

@RestController
public class AccountController {

    private AccountService accountService;

    @Autowired
    public AccountController(AccountService accountService) {
        this.accountService = accountService;
    }

    @GetMapping(path = "/users/login/{login}", produces = { MediaType.APPLICATION_JSON_VALUE })
    public UserModel getUserByLogin(@PathVariable(required = true) String login) {
        return accountService.getUserByLogin(login);
    }

    @GetMapping(path = "/users/{id}", produces = { MediaType.APPLICATION_JSON_VALUE })
    public UserModel getUser(@PathVariable(required = true) String id, HttpServletResponse response) {
        UUID uuid;
        try {
            uuid = UUID.fromString(id);
        } catch (IllegalArgumentException ex) {
            response.setStatus(HttpStatus.BAD_REQUEST.value());
            return null;
        }
        return accountService.getUser(uuid);
    }

    @ResponseStatus(code = HttpStatus.CREATED)
    @PostMapping(path = "/users", consumes = { MediaType.APPLICATION_JSON_VALUE }, produces = { MediaType.APPLICATION_JSON_VALUE })
    public UUID createUser(@RequestBody UserModel model) {
        return accountService.createUser(model);
    }
}