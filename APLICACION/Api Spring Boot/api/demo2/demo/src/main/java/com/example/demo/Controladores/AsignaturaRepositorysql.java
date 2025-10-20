package com.example.demo.Controladores;

import com.example.demo.Entidades.AsignaturaId;
import com.example.demo.Entidades.EAsignatura;
import com.example.demo.Entidades.EAsignaturasql;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface AsignaturaRepositorysql extends JpaRepository<EAsignaturasql, AsignaturaId> {

    @Modifying
    @Query("DELETE FROM EAsignaturasql a WHERE a.id.idUsuario = :idUsuario")  // ← CORREGIDO: EAsignaturasql
    void deleteByIdUsuario(@Param("idUsuario") Long idUsuario);

    List<EAsignaturasql> findByIdIdUsuario(Long idUsuario);  // ← CORREGIDO: EAsignaturasql

    //metodo para borrar asignatura por su idassignatura y idusuario
    @Modifying
    @Transactional
    @Query("DELETE FROM EAsignaturasql a WHERE a.id.id_asignatura = :id_asignatura AND a.id.idUsuario = :idUsuario")
    void deleteByIdAsignatura(@Param("id_asignatura") Long id_asignatura, @Param("idUsuario") Long idUsuario);
}
