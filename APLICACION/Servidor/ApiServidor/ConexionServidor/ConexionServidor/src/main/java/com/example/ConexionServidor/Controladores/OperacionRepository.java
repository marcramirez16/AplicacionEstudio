package com.example.ConexionServidor.Controladores;


import com.example.ConexionServidor.Entidades.EOperacion;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;


@Repository
public interface OperacionRepository extends JpaRepository<EOperacion, Long> {

    @Query("SELECT o FROM EOperacion o WHERE o.id_paso = :id_paso")
    List<EOperacion> findById_paso(@Param("id_paso") Long id_paso);

    @Modifying
    @Transactional
    @Query("DELETE FROM EOperacion o WHERE o.id_paso = :id_paso")
    void deleteAllById_paso(@Param("id_paso") Long id_paso);

}