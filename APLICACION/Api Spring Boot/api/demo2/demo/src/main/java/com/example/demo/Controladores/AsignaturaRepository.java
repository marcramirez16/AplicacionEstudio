package com.example.demo.Controladores;

import com.example.demo.Entidades.AsignaturaId;
import com.example.demo.Entidades.EAsignatura;
import com.example.demo.Entidades.EAsignaturasql;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface AsignaturaRepository extends JpaRepository<EAsignatura, Long> {
    // Borrar todas las asignaturas de un usuario por su id
    void deleteByIdUsuario(Long idUsuario);
}

