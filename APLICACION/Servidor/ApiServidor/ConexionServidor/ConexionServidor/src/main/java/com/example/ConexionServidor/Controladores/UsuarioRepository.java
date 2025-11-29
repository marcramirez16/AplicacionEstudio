package com.example.ConexionServidor.Controladores;



import com.example.ConexionServidor.Entidades.EUsuario;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface UsuarioRepository extends JpaRepository<EUsuario, Long> {
    // Aquí puedes agregar consultas personalizadas si quieres
    Optional<EUsuario> findByUsuarioAndContrasena(String usuario, String contrasena);


}