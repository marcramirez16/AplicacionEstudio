package com.example.ConexionServidor.Entidades;

import jakarta.persistence.*;

@Entity
@Table(name = "error_logs")
public class ErrorLogEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String message;

    @Column(length = 5000)
    private String stacktrace;

    public ErrorLogEntity() {}

    public ErrorLogEntity(String message, String stacktrace) {
        this.message = message;
        this.stacktrace = stacktrace;
    }

    // getters y setters
    public Long getId() { return id; }
    public String getMessage() { return message; }
    public void setMessage(String message) { this.message = message; }
    public String getStacktrace() { return stacktrace; }
    public void setStacktrace(String stacktrace) { this.stacktrace = stacktrace; }
}
