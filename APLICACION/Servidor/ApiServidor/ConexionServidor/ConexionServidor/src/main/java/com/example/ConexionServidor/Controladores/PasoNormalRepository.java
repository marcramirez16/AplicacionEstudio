package com.example.ConexionServidor.Controladores;

import com.example.ConexionServidor.Entidades.EPasonormal;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface PasoNormalRepository extends JpaRepository<EPasonormal, Long> {

    //@Query("SELECT p FROM EPasonormal p WHERE p.id_respuesta = :id_respuesta")
    //Optional<EPasonormal> findByIdRespuesta(@Param("id_respuesta") Long id_respuesta);

    @Query("SELECT p FROM EPasonormal p WHERE p.id_respuesta = :id_respuesta")
    Optional<EPasonormal> findByIdRespuesta(@Param("id_respuesta") Long id_respuesta);

}

