package com.example.ConexionServidor.Entidades;


import jakarta.persistence.*;

@Entity
@Table(name = "Pasomates")
public class EPaso {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id_paso;

    private Long id_respuesta;
    private Long numero;
    private String textopaso;

    public EPaso(Long id_respuesta, Long numero, String textopaso) {
        this.id_respuesta = id_respuesta;
        this.numero = numero;
        this.textopaso = textopaso;
    }

    public EPaso(){}

    public Long getId_paso() {
        return id_paso;
    }

    public void setId_paso(Long id_paso) {
        this.id_paso = id_paso;
    }

    public Long getId_respuesta() {
        return id_respuesta;
    }

    public void setId_respuesta(Long id_respuesta) {
        this.id_respuesta = id_respuesta;
    }

    public Long getNumero() {
        return numero;
    }

    public void setNumero(Long numero) {
        this.numero = numero;
    }

    public String getTextopaso() {
        return textopaso;
    }

    public void setTextopaso(String textopaso) {
        this.textopaso = textopaso;
    }
}
