package com.example.demo.Entidades;
import jakarta.persistence.*;

@Entity
@Table(name = "Tema")
public class ETema {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long id;
    private Long id_tema;
    private Long idAsignatura;
    private String nombre;

    public ETema() {}

    public ETema(Long id_tema, Long idAsignatura, String nombre) {
        this.id_tema = id_tema;
        this.idAsignatura = idAsignatura;
        this.nombre = nombre;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

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

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }
}
