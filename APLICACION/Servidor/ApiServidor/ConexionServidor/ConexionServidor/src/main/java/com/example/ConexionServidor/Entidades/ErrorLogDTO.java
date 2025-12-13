package com.example.ConexionServidor.Entidades;

public class ErrorLogDTO {
    private String message;
    private String stacktrace;

    public ErrorLogDTO() {}

    public String getMessage() { return message; }
    public void setMessage(String message) { this.message = message; }

    public String getStacktrace() { return stacktrace; }
    public void setStacktrace(String stacktrace) { this.stacktrace = stacktrace; }
}
