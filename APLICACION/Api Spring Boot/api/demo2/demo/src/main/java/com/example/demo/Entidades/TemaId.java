package com.example.demo.Entidades;

import jakarta.persistence.Embeddable;
import jakarta.persistence.EmbeddedId;

import java.io.Serializable;
import java.util.Objects;

@Embeddable
public class TemaId implements Serializable {
    private Long id_tema;
    private Long idAsignatura;
    private Long idUsuario;

    public TemaId() {}
    public TemaId(Long id_tema, Long idAsignatura, Long idUsuario) {
        this.id_tema = id_tema;
        this.idAsignatura = idAsignatura;
        this.idUsuario = idUsuario;
    }

    // Getters y Setters
    public Long getId_tema() {
        return id_tema;
    }

    public void setId_tema(Long id_tema) {
        this.id_tema = id_tema;
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
        TemaId temaId = (TemaId) o;
        return Objects.equals(id_tema, temaId.id_tema) &&
                Objects.equals(idAsignatura, temaId.idAsignatura) &&
                Objects.equals(idUsuario, temaId.idUsuario);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id_tema, idAsignatura, idUsuario);
    }
}