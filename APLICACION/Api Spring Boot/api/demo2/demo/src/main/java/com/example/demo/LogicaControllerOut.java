package com.example.demo;

import com.example.demo.Controladores.UsuarioRepository;
import com.example.demo.Entidades.EUsuario;
import com.example.demo.Servidor_Archivos.Servidor_Archivo;
import com.example.demo.Servidor_Archivos.Usuario;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

/**
 * Esta Classe, devuelve los metodos del backend java al front end grafico cshar
 */
@RestController
@RequestMapping("/api")
public class LogicaControllerOut {
    //Atributos
    Servidor_Archivo servidor = new Servidor_Archivo();

    /**
     * Poner los controladores
     */
    @Autowired
    private UsuarioRepository usuarioRepository;

//Metodos para retornar servidor archivos
    /**
     * Metodo para devolver las assignaturas
     * @return
     */
    @GetMapping("/DevolverListaAssignaturas")
    public List<String> DevolverListaAssignaturas(){
        //servidor.crearUsuarioyIniciarlo();

        //obtener id del usuario seleccionado
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);


        return servidor.DevolverListaAssignaturas();
    }

//Metodos para retornar de la bd sql
    /**
     * Retornar usuario: Iniciar session de un usuario "usuario, contraseña"
     * @param usuario
     */
    @PostMapping("/iniciarusuario")
    public ResponseEntity<?> iniciarSession(@RequestBody EUsuario usuario) {
        //Buscar usuario por contraseña y usuario
        Optional<EUsuario> usuarioEncontrado = usuarioRepository.findByUsuarioAndContraseña(
                usuario.getUsuario(), usuario.getContraseña()
        );

        if (usuarioEncontrado.isPresent()) {
            //Usuario iniciado con Exito.
            // Guardarlo en properties para hacerlo persistente
            EUsuario usuarioentidadresp = usuarioEncontrado.get();
            Usuario usuarion = new Usuario(usuarioentidadresp.getId(), usuarioentidadresp.getUsuario(), usuarioentidadresp.getEmail(), usuarioentidadresp.getContraseña());
            usuarion.guardaridusuarioiniciado();

            //Retornar el usuario entity al frontend
            return ResponseEntity.ok(usuarioEncontrado.get()); // Devuelve el usuario
        } else {
            //Usuario no iniciado...
            return ResponseEntity
                    .status(HttpStatus.UNAUTHORIZED)
                    .body("Usuario o contraseña incorrectos"); // Devuelve mensaje de error
        }
    }


}