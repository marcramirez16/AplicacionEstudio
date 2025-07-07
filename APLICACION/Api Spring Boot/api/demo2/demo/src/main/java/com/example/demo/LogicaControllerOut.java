package com.example.demo;

import com.example.demo.Controladores.UsuarioRepository;
import com.example.demo.Entidades.EUsuario;
import com.example.demo.Servidor_Archivos.*;
import org.apache.commons.math3.analysis.function.Asin;
import org.apache.coyote.Response;
import org.apache.poi.xwpf.usermodel.XWPFDocument;
import org.apache.poi.xwpf.usermodel.XWPFParagraph;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.FileSystemResource;
import org.springframework.core.io.Resource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
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

    /**
     * Retornar objeto del archivo seleccionado
     * @return
     */
    @PostMapping("/ArchivoSeleccionado")
    public Archivo ArchivoSeleccionado(){


        String rutaArchivo = servidor.retornarArchivoSeleccionado();
        Archivo archivo = new Archivo();
        Archivo archivo2 = archivo.RetorarArchivoRuta(servidor, rutaArchivo);
        return archivo2;
    }

    /**
     * Metodo para borrar la asignatura carpeta
     * @param nombreAsignatura
     * @return
     */
    @PostMapping("/BorrarAsignatura")
    public Boolean borrarAsignatura(@RequestParam String nombreAsignatura) {
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        Assignatura asignatura = new Assignatura(servidor.usuario, nombreAsignatura);

        return asignatura.borrarAsignatura();

    }

    /**
     * Metodo para borrar el tema carpeta
     * @param nombreAsignatura
     * @return
     */
    @PostMapping("/BorrarTema")
    public Boolean borrarTema(@RequestParam String nombreAsignatura, @RequestParam String nombreTema) {
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        Assignatura asignatura = new Assignatura(servidor.usuario, nombreAsignatura);
        Tema tema = new Tema(asignatura, nombreTema);

        return tema.borrarTema();
    }

    /**
     * Metodo para borrar el archivo
     * @param nombreAsignatura
     * @return
     */
    @PostMapping("/BorrarArchivo")
    public Boolean borrarArchivo(@RequestParam String nombreAsignatura, @RequestParam String nombreTema, @RequestParam String nombreArchivo) {
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        Assignatura asignatura = new Assignatura(servidor.usuario, nombreAsignatura);
        Tema tema = new Tema(asignatura, nombreTema);
        Archivo archivo = new Archivo(tema, nombreArchivo);

        return archivo.borrarArchivo();
    }

    /**
     * Metodo para devolver la ruta de una asignatura
     * @param nombreAsignatura
     * @return
     */
    @PostMapping("/RutaAsignatura")
    public String RutaAsignatura(@RequestParam String nombreAsignatura){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        Assignatura asignatura = new Assignatura(servidor.usuario, nombreAsignatura);
        return asignatura.getrutaAssignatura();
    }

}