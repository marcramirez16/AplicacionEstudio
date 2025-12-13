package com.example.demo.Controladores;

import com.example.demo.Entidades.EPaso;
import com.example.demo.Entidades.EPasonormal;
import com.example.demo.Entidades.ERespuesta;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

import static org.springframework.data.jpa.domain.AbstractPersistable_.id;

@Repository
public interface PasoNormalRepository extends JpaRepository<EPasonormal, Long> {

    //@Query("SELECT p FROM EPasonormal p WHERE p.id_respuesta = :id_respuesta")
    //Optional<EPasonormal> findByIdRespuesta(@Param("id_respuesta") Long id_respuesta);

    @Query("SELECT p FROM EPasonormal p WHERE p.id_respuesta = :id_respuesta")
    Optional<EPasonormal> findByIdRespuesta(@Param("id_respuesta") Long id_respuesta);

}

