package legacyloom.timelineservice.exception;

import legacyloom.timelineservice.dto.response.ServiceResponse;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(ResourceNotFoundException.class)
    public ResponseEntity<ServiceResponse<Void>> handleResourceNotFoundException(ResourceNotFoundException ex) {
        ServiceResponse<Void> response = ServiceResponse.<Void>builder()
                .success(false)
                .statusCode(HttpStatus.NOT_FOUND.value())
                .errorMessage(ex.getMessage())
                .build();
        return ResponseEntity.status(HttpStatus.NOT_FOUND).body(response);
    }

    @ExceptionHandler(Exception.class)
    public ResponseEntity<ServiceResponse<Void>> handleGeneralException(Exception ex) {
        ServiceResponse<Void> response = ServiceResponse.<Void>builder()
                .success(false)
                .statusCode(HttpStatus.INTERNAL_SERVER_ERROR.value())
                .errorMessage("An unexpected error occurred: " + ex.getMessage())
                .build();
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(response);
    }
}