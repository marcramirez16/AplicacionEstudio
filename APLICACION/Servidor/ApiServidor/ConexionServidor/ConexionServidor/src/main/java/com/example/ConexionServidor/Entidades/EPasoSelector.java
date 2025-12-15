package com.example.ConexionServidor.Entidades;

import jakarta.persistence.*;

@Entity
@Table(name = "Pasoselector")
public class EPasoSelector {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id_paso;

    private long id_respuesta;
    private String numero;
    private String texto;

    public EPasoSelector() {}
    public EPasoSelector(long id_respuesta, String numero, String texto) {
        this.id_respuesta = id_respuesta;
        this.numero = numero;
        this.texto = texto;
    }

    public long getId_paso() {
        return id_paso;
    }

    public void setId_paso(long id_paso) {
        this.id_paso = id_paso;
    }

    public long getId_respuesta() {
        return id_respuesta;
    }

    public void setId_respuesta(long id_respuesta) {
        this.id_respuesta = id_respuesta;
    }

    public String getNumero() {
        return numero;
    }

    public void setNumero(String numero) {
        this.numero = numero;
    }

    public String getTexto() {
        return texto;
    }

    public void setTexto(String texto) {
        this.texto = texto;
    }
}

