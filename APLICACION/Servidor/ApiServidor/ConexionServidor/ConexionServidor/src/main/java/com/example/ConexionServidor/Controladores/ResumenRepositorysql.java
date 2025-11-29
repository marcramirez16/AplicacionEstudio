package com.example.ConexionServidor.Controladores;

import com.example.ConexionServidor.Entidades.EResumensql;
import com.example.ConexionServidor.Entidades.ResumenId;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;


@Repository
public interface ResumenRepositorysql extends JpaRepository<EResumensql, ResumenId> {

    @Modifying
    @Query("DELETE FROM EResumensql r WHERE r.id.idTema = :idTema AND r.id.idAsignatura = :idAsignatura AND r.id.idUsuario = :idUsuario")
    void deleteByTemaYAsignaturaYUsuario(@Param("idTema") Long idTema, @Param("idAsignatura") Long idAsignatura, @Param("idUsuario") Long idUsuario);

    @Modifying
    @Query("DELETE FROM EResumensql r WHERE r.id.idUsuario = :idUsuario")
    void deleteByIdUsuario(@Param("idUsuario") Long idUsuario);

    //borrar resumen por su primary key
    @Modifying
    @Transactional
    @Query("DELETE FROM EResumensql r WHERE r.id.id_resumen = :id_resumen AND r.id.idTema = :idTema AND r.id.idAsignatura = :idAsignatura AND r.id.idUsuario = :idUsuario")
    void deleteByIdResumen(@Param("id_resumen") Long id_resumen,
                           @Param("idTema") Long idTema,
                           @Param("idAsignatura") Long idAsignatura,
                           @Param("idUsuario") Long idUsuario);

}