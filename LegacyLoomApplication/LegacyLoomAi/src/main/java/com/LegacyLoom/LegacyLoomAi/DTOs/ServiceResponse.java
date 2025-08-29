package com.LegacyLoom.LegacyLoomAi.DTOs;

import java.util.ArrayList;
import java.util.List;

public class ServiceResponse<T> {
    private boolean success;
    private T data;
    private String message;
    private String error;

    public ServiceResponse(T data) {
        this.success = true;
        this.data = data;
        this.message = "Request processed successfully";
        this.error = null;
    }

    public ServiceResponse(String error, String message) {
        this.success = false;
        this.data = null;
        this.error = error;
        this.message = message;
    }

    private ServiceResponse() {

    }
    public static <T> ServiceResponse<T> success(T data) {
        return new ServiceResponse<>(data);
    }

    public static <T> ServiceResponse<T> error(String error, String message) {
        return new ServiceResponse<>(error, message);
    }

    public boolean isSuccess() {
        return success;
    }

    public void setSuccess(boolean success) {
        this.success = success;
    }

    public T getData() {
        return data;
    }

    public void setData(T data) {
        this.data = data;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getError() {
        return error;
    }

    public void setError(String error) {
        this.error = error;
    }
}
