package com.example.demo.Entidades;

import jakarta.persistence.*;

@Entity
@Table(name = "Resumen")
public class EResumen {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private Long id_resumen;
    private Long idTema;
    private String nombre;

    public EResumen(){}

    public EResumen(Long id_resumen, Long idTema, String nombre) {
        this.id_resumen = id_resumen;
        this.idTema = idTema;
        this.nombre = nombre;
    }

    //getters y setters
    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getIdTema() {
        return idTema;
    }

    public void setIdTema(Long idTema) {
        this.idTema = idTema;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public Long getId_resumen() {
        return id_resumen;
    }

    public void setId_resumen(Long id_resumen) {
        this.id_resumen = id_resumen;
    }
}
