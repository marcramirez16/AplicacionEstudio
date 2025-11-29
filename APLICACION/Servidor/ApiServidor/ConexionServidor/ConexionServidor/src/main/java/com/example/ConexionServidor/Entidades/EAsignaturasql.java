package com.example.ConexionServidor.Entidades;

import jakarta.persistence.EmbeddedId;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;

@Entity
@Table(name = "Asignatura")
public class EAsignaturasql {

    @EmbeddedId
    private AsignaturaId id;
    private String nombre;

    public EAsignaturasql() {}

    public EAsignaturasql(Long id_asignatura, Long idUsuario, String nombre) {
        this.id = new AsignaturaId(id_asignatura, idUsuario);
        this.nombre = nombre;
    }

    // Getters y Setters
    public AsignaturaId getId() {
        return id;
    }

    public void setId(AsignaturaId id) {
        this.id = id;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }
}