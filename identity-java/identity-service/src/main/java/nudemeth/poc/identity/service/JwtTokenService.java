package nudemeth.poc.identity.service;

import java.time.Instant;
import java.time.ZoneId;
import java.time.ZonedDateTime;
import java.time.format.DateTimeFormatter;

import com.auth0.jwt.JWT;
import com.auth0.jwt.algorithms.Algorithm;
import com.auth0.jwt.exceptions.JWTVerificationException;
import com.auth0.jwt.interfaces.DecodedJWT;
import com.auth0.jwt.interfaces.JWTVerifier;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import nudemeth.poc.identity.config.CipherConfig;
import nudemeth.poc.identity.model.UserModel;

@Service
public class JwtTokenService implements TokenService {

    private static final int TOKEN_LIFETIME_HOURS = 2;
    private static final DateTimeFormatter DATETIME_FORMAT = DateTimeFormatter.ISO_INSTANT;

    private final Algorithm algorithm;

    @Autowired
    public JwtTokenService(CipherConfig config) {
        this.algorithm = Algorithm.HMAC256(config.getKey());
    }

    @Override
    public String create(UserModel model) {
        String expiryDateTime = ZonedDateTime.now().plusHours(TOKEN_LIFETIME_HOURS).format(DATETIME_FORMAT);
        String token = JWT.create()
            .withClaim("id", model.getId().toString())
            .withClaim("login", model.getLogin())
            .withClaim("exp", expiryDateTime)
            .sign(algorithm);
        return token;
    }

    @Override
    public boolean verify(String token) {
        try {
            JWTVerifier verifier = JWT.require(algorithm)
            .build();
            DecodedJWT jwt = verifier.verify(token);
            String exp = jwt.getClaim("exp").asString();
            ZonedDateTime expiryDateTime = Instant.parse(exp).atZone(ZoneId.systemDefault());
            return isExpired(expiryDateTime);
        } catch (JWTVerificationException ex) {
            return false;
        }
    }

    private boolean isExpired(ZonedDateTime expiryDateTime) {
        return ZonedDateTime.now().compareTo(expiryDateTime) < 0;
    }

}