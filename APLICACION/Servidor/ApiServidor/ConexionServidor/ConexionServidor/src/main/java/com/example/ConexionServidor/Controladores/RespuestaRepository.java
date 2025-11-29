package com.example.ConexionServidor.Controladores;

import com.example.ConexionServidor.Entidades.ERespuesta;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface RespuestaRepository extends JpaRepository<ERespuesta, Long> {

    @Transactional
    @Modifying
    @Query("DELETE FROM ERespuesta r WHERE r.id_pregunta = :idpregunta")
    void deleteByIdPregunta(@Param("idpregunta") Long idpregunta);

    @Query("SELECT r FROM ERespuesta r WHERE r.id_pregunta = :idpregunta")
    ERespuesta findByIdPregunta(@Param("idpregunta") Long idpregunta);

}