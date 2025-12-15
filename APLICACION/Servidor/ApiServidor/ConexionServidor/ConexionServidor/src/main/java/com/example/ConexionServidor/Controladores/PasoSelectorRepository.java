package com.example.ConexionServidor.Controladores;

import com.example.ConexionServidor.Entidades.EPasoSelector;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PasoSelectorRepository extends JpaRepository<EPasoSelector, Long> {

    @Query("SELECT p FROM EPasoSelector p WHERE p.id_respuesta = :id_respuesta")
    List<EPasoSelector> findById_respuesta(@Param("id_respuesta") Long id_respuesta);
}

