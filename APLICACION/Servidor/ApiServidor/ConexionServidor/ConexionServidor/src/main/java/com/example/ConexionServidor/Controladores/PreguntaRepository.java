package com.example.ConexionServidor.Controladores;

import com.example.ConexionServidor.Entidades.EPregunta;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface PreguntaRepository extends JpaRepository<EPregunta, Long> {
    @Query("SELECT p FROM EPregunta p WHERE p.id_resumen = :idResumen")
    List<EPregunta> findByIdResumen(@Param("idResumen") Long idResumen);

    @Modifying
    @Transactional
    @Query("DELETE FROM EPregunta p WHERE p.id_pregunta = :idPregunta")
    void deleteByIdPregunta(@Param("idPregunta") Long idPregunta);

    /*
    @Modifying
    @Transactional
    @Query("UPDATE EPregunta p SET p.pregunta = :pregunta, p.tipo = :tipo WHERE p.id_pregunta = :idPregunta")
    void updatePregunta(@Param("idPregunta") Long idPregunta, @Param("pregunta") String pregunta, @Param("tipo") String tipo);
*/
    @Modifying
    @Transactional
    @Query("UPDATE EPregunta p SET p.pregunta = :pregunta, p.tipo = :tipo, p.imagen = :imagen WHERE p.id_pregunta = :idPregunta")
    void updatePregunta(
            @Param("idPregunta") Long idPregunta,
            @Param("pregunta") String pregunta,
            @Param("tipo") String tipo,
            @Param("imagen") String imagen);

    //buscar preguntas por assignaturao tema:
    //obtener todos los id de preguntas de una asignatura
    @Query("SELECT p.id_pregunta FROM EPregunta p WHERE p.id_asignatura = :idAsignatura")
    List<Long> findIdsByIdAsignatura(@Param("idAsignatura") Long idAsignatura);

    // Buscar todos los IDs de preguntas de un tema
    @Query("SELECT p.id_pregunta FROM EPregunta p WHERE p.id_tema = :idTema")
    List<Long> findIdsByIdTema(@Param("idTema") Long idTema);

    //Buscar todos los ids de pregutnas de una resumen
    @Query("SELECT p.id_pregunta FROM EPregunta p WHERE p.id_resumen = :idResumen")
    List<Long> findIdsByIdResumen(@Param("idResumen") Long idResumen);

}


