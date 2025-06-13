package legacyloom.timelineservice.dto.response;

import lombok.Builder;
import lombok.Data;

import java.util.List;

@Data
@Builder
public class ServiceResponse<T> {
    private boolean success;
    private T data;
    private int statusCode;
    private String successMessage;
    private String errorMessage;
    private String errorCode;
    private List<String> errors;
}