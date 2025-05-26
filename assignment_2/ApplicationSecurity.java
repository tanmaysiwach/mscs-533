import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import javax.validation.constraints.Pattern;
import javax.validation.Valid;
import org.springframework.validation.annotation.Validated;
import org.springframework.http.HttpStatus;

@SpringBootApplication
@RestController
@Validated
public class SecureAppLayerExample {

    private boolean authenticate(String apiKey) {
        return "secret-api-key".equals(apiKey);
    }

    public static class InputRequest {
        @Pattern(regexp = "^[a-zA-Z0-9 ]+$", message = "Input must be alphanumeric and spaces only")
        public String userInput;

        public String getUserInput() {
            return userInput;
        }
    }

    @PostMapping("/postSomething")
    public ResponseEntity<String> processInput(
            @RequestHeader("X-API-KEY") String apiKey,
            @Valid @RequestBody InputRequest request) {
        
        if (!authenticate(apiKey)) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Unauthorized");
        }
        String sanitizedInput = request.getUserInput().trim();
        return ResponseEntity.ok("Processed input safely: " + sanitizedInput);
    }

    public static void main(String[] args) {
        SpringApplication.run(SecureAppLayerExample.class, args);
    }
}
