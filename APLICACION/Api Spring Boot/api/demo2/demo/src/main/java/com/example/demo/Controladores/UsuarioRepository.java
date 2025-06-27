package com.example.demo.Controladores;


import com.example.demo.Entidades.EUsuario;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface UsuarioRepository extends JpaRepository<EUsuario, Long> {
    // Aquí puedes agregar consultas personalizadas si quieres
    Optional<EUsuario> findByUsuarioAndContraseña(String usuario, String contraseña);

}