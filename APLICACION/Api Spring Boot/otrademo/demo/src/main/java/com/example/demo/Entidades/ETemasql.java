package com.example.demo.Entidades;

import jakarta.persistence.EmbeddedId;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;

@Entity
@Table(name = "Tema")
public class ETemasql {

    @EmbeddedId
    private TemaId id;
    private String nombre;

    public ETemasql() {}

    public ETemasql(Long id_tema, Long idAsignatura, Long idUsuario, String nombre) {
        this.id = new TemaId(id_tema, idAsignatura, idUsuario);
        this.nombre = nombre;
    }

    public TemaId getId() {
        return id;
    }

    public void setId(TemaId id) {
        this.id = id;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }
}