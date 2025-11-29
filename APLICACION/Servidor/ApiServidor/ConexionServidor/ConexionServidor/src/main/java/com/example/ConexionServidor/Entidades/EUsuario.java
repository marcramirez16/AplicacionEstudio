package com.example.ConexionServidor.Entidades;

import jakarta.persistence.*;

@Entity
@Table(name = "Usuario")
public class EUsuario {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String usuario;

    @Column(name = "contraseña")
    private String contrasena;
    private String email;

    // Constructor vacío necesario para JPA
    public EUsuario() {}

    public EUsuario(Long id, String usuario, String contrasena, String email) {
        this.id = id;
        this.usuario = usuario;
        this.contrasena = contrasena;
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

    public String getContrasena() {
        return contrasena;
    }

    public void setContrasena(String contrasena) {
        this.contrasena = contrasena;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }
}