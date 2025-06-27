package com.example.demo.Entidades;

import jakarta.persistence.*;

@Entity
@Table(name = "Usuario")
public class EUsuario {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String usuario;
    private String contraseña;
    private String email;

    // Constructor vacío necesario para JPA
    public EUsuario() {}

    public EUsuario(Long id, String usuario, String contraseña, String email) {
        this.id = id;
        this.usuario = usuario;
        this.contraseña = contraseña;
        this.email = email;
    }

    // Getters y setters
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getUsuario() {
        return usuario;
    }

    public void setUsuario(String usuario) {
        this.usuario = usuario;
    }

    public String getContraseña() {
        return contraseña;
    }

    public void setContraseña(String contraseña) {
        this.contraseña = contraseña;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }
}