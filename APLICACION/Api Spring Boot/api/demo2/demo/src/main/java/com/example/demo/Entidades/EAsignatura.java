package com.example.demo.Entidades;

import jakarta.persistence.*;

import java.io.Serializable;

@Entity
@Table(name = "Asignatura")
public class EAsignatura  {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private Long id_asignatura;
    private Long idUsuario;
    private String nombre;

    public EAsignatura() {}
    public EAsignatura(Long id_asignatura, Long idUsuario, String nombre) {
        this.id_asignatura = id_asignatura;
        this.idUsuario = idUsuario;
        this.nombre = nombre;
    }

    //Getters y Setters
    public Long getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(Long idUsuario) {
        this.idUsuario = idUsuario;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public Long getId_asignatura() {
        return id_asignatura;
    }

    public void setId_asignatura(Long id_asignatura) {
        this.id_asignatura = id_asignatura;
    }
}

