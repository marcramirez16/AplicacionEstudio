package com.example.ConexionServidor.Controladores;


import com.example.ConexionServidor.Entidades.EPaso;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PasoRepository extends JpaRepository<EPaso, Long> {
    @Query("SELECT p FROM EPaso p WHERE p.id_respuesta = :id_respuesta")
    List<EPaso> findById_respuesta(@Param("id_respuesta") Long id_respuesta);


}

