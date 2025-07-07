package com.example.demo;

import com.example.demo.Controladores.UsuarioRepository;
import com.example.demo.Entidades.EUsuario;
import com.example.demo.Servidor_Archivos.*;
import org.apache.commons.math3.analysis.function.Asin;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.Optional;

@RestController
@RequestMapping("/api")
public class LogicaControllerIn {
//Atributos
    Servidor_Archivo servidor = new Servidor_Archivo();

    /**
     * Poner los controladores
     */
    @Autowired
    private UsuarioRepository usuarioRepository;

//Metodos para crear carpetas y archivos

    /**
     * Metodo para crear una nueva Asignatura
     * @param nombre
     * @return
     */
    @PostMapping("/crearAsignatura")
    public boolean crearAsignatura(String nombre){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        Assignatura asignatura = new Assignatura(nombre, servidor.usuario);

        return asignatura.agregarAssignatura();
    }

    /**
     * Metodo para crear un nuevo Tema
     *
     */
    @PostMapping("/crearTema")
    public boolean crearTema(String nombreAssignatura , String nombreTema){
        //obtener asignatura
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);
        Assignatura asignatura = new Assignatura(servidor.usuario, nombreAssignatura);

        //Obtener Tema
        Tema tema = new Tema(nombreTema, asignatura);

        return tema.agregarTema();
    }

    /**
     * Metodo para agregar un archivo nuevo
      * @param nombreAssignatura
     * @param nombreTema
     * @param nombreArchivo
     * @return
     */
    @PostMapping("/crearArchivo")
    public boolean crearArchivo(String nombreAssignatura , String nombreTema, String nombreArchivo) {

        System.out.println("nombreasignatura: " + nombreAssignatura + " nombretema: " + nombreTema + " nombreArchivo: " + nombreArchivo);
        //obtener id usuario
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        Assignatura asignatura = new Assignatura(servidor.usuario, nombreAssignatura);

        //Obtener Tema
        Tema tema = new Tema(asignatura, nombreTema);

        //Obtener Archivo
        Archivo archivo = new Archivo(nombreArchivo, tema);

            return archivo.agregarArchivo();
    }


//Metodos sql y properties
    //usuario
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
            usuarion.guardaridusuarioiniciado(); //guardar usuario iniciado

            //Retornar el usuario entity al frontend
            return ResponseEntity.ok(usuarioEncontrado.get()); // Devuelve el usuario
        } else {
            //Usuario no iniciado...
            return ResponseEntity
                    .status(HttpStatus.UNAUTHORIZED)
                    .body("Usuario o contraseña incorrectos"); // Devuelve mensaje de error
        }
    }

    /**
     * Agregar un usuario nuevo a la bd.
     * Tambien crear un usuario en el backend "carpeta usuario"
     * @param usuario 'entidad usuario'
     * @return respuesta en insertar
     */
    @PostMapping("/crearusuario")
    public String insertarUsuario(@RequestBody EUsuario usuario) {

        //generar id con los datos de la entidad "usuario para crear carpeta"
        Usuario usuario2 = new Usuario(usuario.getUsuario(), usuario.getContraseña(), usuario.getEmail());


        try {
            EUsuario guardado = usuarioRepository.save(usuario); // guarda en SQL

            System.out.println(usuario.getUsuario() + usuario.getContraseña() + usuario.getEmail() + "id" + usuario.getId());

            usuario2.setIdusuario(guardado.getId()); //agregar id del usuario des de sql...
            servidor.crearCarpetaUsuario(usuario2); // crea carpeta


            return "usuario" + guardado.getUsuario() + " guardado";

        }catch (DataIntegrityViolationException ex) {
            // Detectar si es por clave única duplicada
            if (ex.getCause() != null && ex.getCause().getCause() != null) {
                String message = ex.getCause().getCause().getMessage();
                if (message != null && message.contains("Duplicate entry")) {
                    return "Error: El usuario o correo ya existe.";
                }
            }
            return "Error de integridad de datos: " + ex.getMessage();

        } catch (Exception e) {
            System.out.println("Excepcion: ----" + e.getMessage());

            return "Error al guardar usuario o crear carpeta: " + e.getMessage();
        }

    }

    /**
     * Metodo Cerrar Usuario
     */
    @GetMapping("/cerrarUsuario")
    public void cerrarUsuario() {
        String usuario = servidor.cerrarUsuario();
    }

    /**
     * Metodo para agregar el archivo seleccionado en properties...
     * @param nombreAsignatura
     * @param nombreTema
     * @param nombreArchivo
     * @return
     */
    @PostMapping("/SeleccionarArchivo")
    public boolean SeleccionarArchivo(String nombreAsignatura, String nombreTema, String nombreArchivo){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);


        Assignatura asignatura = new Assignatura(servidor.usuario, nombreAsignatura);

        Tema tema = new Tema(asignatura, nombreTema);
        Archivo archivo= new Archivo(tema, nombreArchivo);

        return archivo.seleccionarArchivo();
    }

    /**
     * Metodo para deseleccionar el archivo actual en properties...
     * @return
     */
    @PostMapping("/DeseleccionarArchivo")
    public String DeSeleccionarArchivo(){

        return servidor.DeseleccionarArchivo();
    }
}
