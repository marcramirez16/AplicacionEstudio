package com.example.demo.Entidades;

import jakarta.persistence.Embeddable;

import java.io.Serializable;
import java.util.Objects;

@Embeddable
public class ResumenId implements Serializable {
    private Long id_resumen;
    private Long idTema;
    private Long idAsignatura;
    private Long idUsuario;

    public ResumenId() {}
    public ResumenId(Long id_resumen, Long idTema, Long idAsignatura, Long idUsuario) {
        this.id_resumen = id_resumen;
        this.idTema = idTema;
        this.idAsignatura = idAsignatura;
        this.idUsuario = idUsuario;
    }

    // Getters y Setters
    public Long getId_resumen() {
        return id_resumen;
    }

    public void setId_resumen(Long id_resumen) {
        this.id_resumen = id_resumen;
    }

    public Long getIdTema() {
        return idTema;
    }

    public void setIdTema(Long idTema) {
        this.idTema = idTema;
    }

    public Long getIdAsignatura() {
        return idAsignatura;
    }

    public void setIdAsignatura(Long idAsignatura) {
        this.idAsignatura = idAsignatura;
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
        ResumenId resumenId = (ResumenId) o;
        return Objects.equals(id_resumen, resumenId.id_resumen) &&
                Objects.equals(idTema, resumenId.idTema) &&
                Objects.equals(idAsignatura, resumenId.idAsignatura) &&
                Objects.equals(idUsuario, resumenId.idUsuario);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id_resumen, idTema, idAsignatura, idUsuario);
    }
}