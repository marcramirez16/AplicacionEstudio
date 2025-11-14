package com.example.demo.Entidades;


import jakarta.persistence.*;

@Entity
@Table(name = "Operacionmates")
public class EOperacion {
@Id
@GeneratedValue(strategy = GenerationType.IDENTITY)
private Long id_operacion;
private Long id_paso;
private String operacion;
    private Long numero;

    public EOperacion(Long id_paso, String operacion, Long numero) {
        this.id_paso = id_paso;
        this.operacion = operacion;
        this.numero = numero;
    }

    public EOperacion(){}

    public Long getId_operacion() {
        return id_operacion;
    }

    public void setId_operacion(Long id_operacion) {
        this.id_operacion = id_operacion;
    }

    public Long getId_paso() {
        return id_paso;
    }

    public void setId_paso(Long id_paso) {
        this.id_paso = id_paso;
    }

    public String getOperacion() {
        return operacion;
    }

    public void setOperacion(String operacion) {
        this.operacion = operacion;
    }

    public Long getNumero() {
        return numero;
    }

    public void setNumero(Long numero) {
        this.numero = numero;
    }
}

