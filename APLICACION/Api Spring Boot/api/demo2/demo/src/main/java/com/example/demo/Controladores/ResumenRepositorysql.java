package com.example.demo.Controladores;

import com.example.demo.Entidades.EResumensql;
import com.example.demo.Entidades.ResumenId;
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
}