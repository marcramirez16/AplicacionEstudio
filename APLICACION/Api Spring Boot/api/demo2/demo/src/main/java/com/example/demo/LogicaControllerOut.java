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
        //obtener id del usuario seleccionado
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        return servidor.DevolverListaAssignaturas();
    }

    /**
     * Metodo para devolver los Temas
     * @return assignatura
     */
    @GetMapping("/DevolverListaTemas")
    public List<String> DevolverListaTemas(String Assignatura){
        //obtener id del usuario seleccionado
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        return servidor.DevolverListaTemas(Assignatura);
    }

    /**
     * Metodo para devolver archivos
     * @param Assignatura
     * @param Tema
     * @return
     */
    @GetMapping("/DevolverListaArchivos")
    public List<String> DevolverListaArchivos(String Assignatura, String Tema){
        //obtener id del usuario seleccionado
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        return servidor.DevolverListaArchivos(Assignatura, Tema);
    }

//metodos sql y Properties
    /**
     * Retornar usuario: Retornar el usuario iniciado
     */
    @GetMapping("/usuarioiniciado")
    public boolean usuarioIniciado() {
        String usuario = servidor.obteneridusuarioiniciado();
        return usuario != null;
    }

}