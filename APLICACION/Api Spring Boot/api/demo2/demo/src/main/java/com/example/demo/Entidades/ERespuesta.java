package com.example.demo.Entidades;

import jakarta.persistence.*;

import javax.annotation.processing.Generated;

@Entity
@Table(name = "Respuesta")
public class ERespuesta {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id_respuesta;
    private Long id_pregunta;
    private String respuesta;

    public ERespuesta(){


    }
    public ERespuesta(Long id_pregunta, String respuesta) {
        this.id_pregunta = id_pregunta;
        this.respuesta = respuesta;
    }

    public Long getId_respuesta() {
        return id_respuesta;
    }

    public void setId_respuesta(Long id_respuesta) {
        this.id_respuesta = id_respuesta;
    }

    public Long getId_pregunta() {
        return id_pregunta;
    }

    public void setId_pregunta(Long id_pregunta) {
        this.id_pregunta = id_pregunta;
    }

    public String getRespuesta() {
        return respuesta;
    }

    public void setRespuesta(String respuesta) {
        this.respuesta = respuesta;
    }
}

