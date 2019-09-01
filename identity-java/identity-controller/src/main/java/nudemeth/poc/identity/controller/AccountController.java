package nudemeth.poc.identity.controller;

import java.util.Optional;
import java.util.UUID;
import java.util.concurrent.CompletableFuture;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.scheduling.annotation.Async;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

import nudemeth.poc.identity.model.UserModel;
import nudemeth.poc.identity.service.AccountService;

@RestController
public class AccountController {

    private AccountService accountService;

    @Autowired
    public AccountController(AccountService accountService) {
        this.accountService = accountService;
    }

    @Async("asyncExecutor")
    @GetMapping(path = "/users/login/{login}", produces = { MediaType.APPLICATION_JSON_UTF8_VALUE })
    public CompletableFuture<Optional<UserModel>> getUserByLogin(@PathVariable(required = true) String login, @RequestParam(required = false) String issuer) {
        return accountService.getUserByLoginAndIssuer(login, issuer);
    }

    @Async("asyncExecutor")
    @GetMapping(path = "/users/email/{email}", produces = { MediaType.APPLICATION_JSON_UTF8_VALUE })
    public CompletableFuture<Optional<UserModel>> getUserByEmail(@PathVariable(required = true) String email) {
        return accountService.getUserByEmail(email);
    }

    @Async("asyncExecutor")
    @GetMapping(path = "/token/user/{id}", produces = { MediaType.APPLICATION_JSON_UTF8_VALUE })
    public CompletableFuture<Optional<String>> getTokenByUserId(@PathVariable(required = true) String id) {
        UUID uuid = getUuidFromString(id);
        return accountService.getTokenByUserId(uuid);
    }

    @Async("asyncExecutor")
    @GetMapping(path = "/users/{id}", produces = { MediaType.APPLICATION_JSON_UTF8_VALUE })
    public CompletableFuture<Optional<UserModel>> getUser(@PathVariable(required = true) String id) {
        UUID uuid = getUuidFromString(id);
        return accountService.getUser(uuid);
    }

    @Async("asyncExecutor")
    @ResponseStatus(code = HttpStatus.CREATED)
    @PostMapping(path = "/users", consumes = { MediaType.APPLICATION_JSON_UTF8_VALUE }, produces = { MediaType.APPLICATION_JSON_UTF8_VALUE })
    public CompletableFuture<UUID> createUser(@RequestBody UserModel model) {
        return accountService.createUser(model);
    }

    @Async("asyncExecutor")
    @ResponseStatus(code = HttpStatus.CREATED)
    @PutMapping(path = "/users/login/{login}", consumes = { MediaType.APPLICATION_JSON_UTF8_VALUE }, produces = { MediaType.APPLICATION_JSON_UTF8_VALUE })
    public CompletableFuture<UUID> createOrUpdateUserByLogin(@PathVariable(required = true) String login, @RequestParam(required = false) String issuer, @RequestBody UserModel model) {
        if (!login.equals(model.getLogin())) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, String.format("Invalid creating or updating login: %s and %s", login, model.getLogin()));
        }
        if (issuer != null && model.getIssuer() != null && !issuer.equals(model.getIssuer())) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, String.format("Invalid creating or updating issuer: %s and %s", issuer, model.getIssuer()));
        }
        return accountService.createOrUpdateUserByLoginAndIssuer(model);
    }

    @Async("asyncExecutor")
    @PutMapping(path = "/users/{id}", consumes = { MediaType.APPLICATION_JSON_UTF8_VALUE }, produces = { MediaType.APPLICATION_JSON_UTF8_VALUE })
    public CompletableFuture<UserModel> updateUser(@PathVariable(required = true) String id, @RequestBody UserModel model) {
        UUID uuid = getUuidFromString(id);
        if (!uuid.equals(model.getId())) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, String.format("Invalid updating id: %s and %s", id, model.getId().toString()));
        }
        return accountService.updateUser(model);
    }

    @Async("asyncExecutor")
    @ResponseStatus(code = HttpStatus.NO_CONTENT)
    @DeleteMapping(path = "/users/{id}")
    public CompletableFuture<Void> deleteUser(@PathVariable(required = true) String id) {
        UUID uuid = getUuidFromString(id);
        return accountService.deleteUser(uuid);
    }

    private UUID getUuidFromString(String id) throws ResponseStatusException {
        try {
            return UUID.fromString(id);
        } catch (IllegalArgumentException ex) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, ex.getMessage(), ex);
        }
    }
}