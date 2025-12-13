package com.example.ConexionServidor.Controladores;

import com.example.ConexionServidor.Entidades.ErrorLogEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ErrorLogRepository extends JpaRepository<ErrorLogEntity, Long> {
}
