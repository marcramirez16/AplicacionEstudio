package com.example.ConexionServidor.Entidades;


import jakarta.persistence.EmbeddedId;
import jakarta.persistence.Entity;
import jakarta.persistence.Table;

@Entity
@Table(name = "Resumen")
public class EResumensql {

    @EmbeddedId
    private ResumenId id;
    private String nombre;

    public EResumensql() {}

    public EResumensql(Long id_resumen, Long idTema, Long idAsignatura, Long idUsuario, String nombre) {
        this.id = new ResumenId(id_resumen, idTema, idAsignatura, idUsuario);
        this.nombre = nombre;
    }

    public ResumenId getId() {
        return id;
    }

    public void setId(ResumenId id) {
        this.id = id;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }
}