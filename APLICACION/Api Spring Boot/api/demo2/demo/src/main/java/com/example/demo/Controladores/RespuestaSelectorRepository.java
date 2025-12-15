package com.example.demo.Controladores;

import com.example.demo.Entidades.ERespuestaSelector;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface RespuestaSelectorRepository extends JpaRepository<ERespuestaSelector, Long> {

    @Query("SELECT r FROM ERespuestaSelector r WHERE r.id_paso = :id_paso")
    List<ERespuestaSelector> findById_paso(@Param("id_paso") Long id_paso);
}
