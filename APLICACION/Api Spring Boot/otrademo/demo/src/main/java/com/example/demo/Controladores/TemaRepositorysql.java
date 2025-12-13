package com.example.demo.Controladores;

import com.example.demo.Entidades.ETemasql;
import com.example.demo.Entidades.TemaId;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;


@Repository
public interface TemaRepositorysql extends JpaRepository<ETemasql, TemaId> {

    @Modifying
    @Query("DELETE FROM ETemasql t WHERE t.id.idAsignatura = :idAsignatura AND t.id.idUsuario = :idUsuario")
    void deleteByIdAsignaturaAndIdUsuario(@Param("idAsignatura") Long idAsignatura, @Param("idUsuario") Long idUsuario);

    @Modifying
    @Query("DELETE FROM ETemasql t WHERE t.id.idUsuario = :idUsuario")
    void deleteByIdUsuario(@Param("idUsuario") Long idUsuario);

    @Modifying
    @Transactional
    @Query("DELETE FROM ETemasql t WHERE t.id.id_tema = :id_tema AND t.id.idAsignatura = :idAsignatura AND t.id.idUsuario = :idUsuario")
    void deleteByIdTema(@Param("id_tema") Long id_tema,
                        @Param("idAsignatura") Long idAsignatura,
                        @Param("idUsuario") Long idUsuario);
}