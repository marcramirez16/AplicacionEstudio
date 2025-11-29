package com.example.ConexionServidor.Entidades;


import jakarta.persistence.Embeddable;

import java.io.Serializable;
import java.util.Objects;

@Embeddable
public class AsignaturaId implements Serializable {
    private Long id_asignatura;
    private Long idUsuario;

    public AsignaturaId() {}

    public AsignaturaId(Long id_asignatura, Long idUsuario) {
        this.id_asignatura = id_asignatura;
        this.idUsuario = idUsuario;
    }

    // Getters y Setters
    public Long getId_asignatura() {
        return id_asignatura;
    }

    public void setId_asignatura(Long id_asignatura) {
        this.id_asignatura = id_asignatura;
    }

    public Long getIdUsuario() {
        return idUsuario;
    }

    public void setIdUsuario(Long idUsuario) {
        this.idUsuario = idUsuario;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        AsignaturaId that = (AsignaturaId) o;
        return Objects.equals(id_asignatura, that.id_asignatura) &&
                Objects.equals(idUsuario, that.idUsuario);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id_asignatura, idUsuario);
    }
}