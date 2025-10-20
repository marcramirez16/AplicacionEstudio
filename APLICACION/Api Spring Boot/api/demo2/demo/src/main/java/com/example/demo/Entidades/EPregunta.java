package com.example.demo.Entidades;

import jakarta.persistence.*;

@Entity
@Table(name = "Pregunta")
public class EPregunta {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id_pregunta;
    private Long id_resumen;
    private Long id_tema;
    private Long id_asignatura;
    private Long id_usuario;
    private String pregunta;
   // private String imagen;

    private String tipo;

    public EPregunta(){

    }
    public EPregunta(Long id_resumen, Long id_tema, Long id_asignatura, Long id_usuario, String pregunta, String tipo) {
        this.id_resumen = id_resumen;
        this.id_tema = id_tema;
        this.id_asignatura = id_asignatura;
        this.id_usuario = id_usuario;
        this.pregunta = pregunta;
        this.tipo = tipo;
    }

    public Long getId_pregunta() {
        return id_pregunta;
    }

    public void setId_pregunta(Long id_pregunta) {
        this.id_pregunta = id_pregunta;
    }

    public Long getId_resumen() {
        return id_resumen;
    }

    public void setId_resumen(Long id_resumen) {
        this.id_resumen = id_resumen;
    }

    public Long getId_tema() {
        return id_tema;
    }

    public void setId_tema(Long id_tema) {
        this.id_tema = id_tema;
    }

    public Long getId_asignatura() {
        return id_asignatura;
    }

    public void setId_asignatura(Long id_asignatura) {
        this.id_asignatura = id_asignatura;
    }

    public Long getId_usuario() {
        return id_usuario;
    }

    public void setId_usuario(Long id_usuario) {
        this.id_usuario = id_usuario;
    }

    public String getPregunta() {
        return pregunta;
    }

    public void setPregunta(String pregunta) {
        this.pregunta = pregunta;
    }

    public String getTipo() {
        return tipo;
    }

    public void setTipo(String tipo) {
        this.tipo = tipo;
    }


}



