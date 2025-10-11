package com.example.demo;

import com.example.demo.Controladores.*;
import com.example.demo.Entidades.*;
import com.example.demo.Servidor_Archivos.*;
import jakarta.persistence.EntityManager;
import jakarta.persistence.PersistenceContext;
import jakarta.transaction.Transactional;
import org.apache.commons.math3.analysis.function.Asin;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api")
public class LogicaControllerIn {
//Atributos
    Servidor_Archivo servidor = new Servidor_Archivo();


    @PersistenceContext
    private EntityManager entityManager;
    /**
     * Poner los controladores
     */
    @Autowired
    private UsuarioRepository usuarioRepository;

    @Autowired
    private AsignaturaRepository asignaturaRepository;

    @Autowired
    private TemaRepository temaRepository;

    @Autowired
    private ResumenRepository resumenRepository;

    @Autowired
    private AsignaturaRepositorysql asignaturaRepositorysql;

    @Autowired
    private TemaRepositorysql temaRepositorysql;

    @Autowired
    private ResumenRepositorysql resumenRepositorysql;

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

    //METODOS PARA AGREGAR LAS ASIGNATURAS, TEMAS, RESUMEN DEL USUARIO EN LA BD

    /**
    * Metodo para buscar todas las asignaturas
    * */
    @PostMapping("/InsertarServidorMysql")
    @Transactional
    public void insertarcarpetasusuarioensql(){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        System.out.println("Usuario ID: " + longid);

        // Limpiar datos existentes en orden inverso (por dependencias de FK)
        resumenRepositorysql.deleteByIdUsuario(longid);
        temaRepositorysql.deleteByIdUsuario(longid);
        asignaturaRepositorysql.deleteByIdUsuario(longid);

        List<String> asignaturas = servidor.DevolverListaAssignaturas();
        System.out.println("Asignaturas obtenidas: " + asignaturas.size());
        System.out.println("Contenido de asignaturas: " + asignaturas); // ← AÑADIDO

        if (asignaturas.isEmpty()) {
            System.out.println("¡ADVERTENCIA: No se encontraron asignaturas!");
            return; // Salir si no hay datos
        }

        for(String asignatura : asignaturas){
            System.out.println("Procesando asignatura: " + asignatura); // ← AÑADIDO
            String[] partes = asignatura.split("\\.", 2);

            // Verifica que el split funcione correctamente
            if (partes.length < 2) {
                System.out.println("¡ERROR: Formato incorrecto en asignatura: " + asignatura);
                continue;
            }

            Long numerolong = Long.parseLong(partes[0]);
            String nombre = partes[1];
            Long usuariolong = longid;

            System.out.println("ID Asignatura: " + numerolong + ", Nombre: " + nombre);

            // Crear y guardar asignatura
            EAsignaturasql asignaturaEntity = new EAsignaturasql(numerolong, usuariolong, nombre);
            EAsignaturasql savedAsignatura = asignaturaRepositorysql.save(asignaturaEntity);
            System.out.println("Asignatura guardada: " + savedAsignatura.getNombre());

            // Procesar temas...
            List<String> temas = servidor.DevolverListaTemas(asignatura);
            System.out.println("Temas obtenidos: " + temas.size() + " para asignatura: " + asignatura);

            for (String tema : temas){
                System.out.println("Procesando tema: " + tema);
                String[] partes2 = tema.split("\\.", 2);

                if (partes2.length < 2) {
                    System.out.println("¡ERROR: Formato incorrecto en tema: " + tema);
                    continue;
                }

                Long numerotemalong = Long.parseLong(partes2[0]);
                String nombretema = partes2[1];

                ETemasql temaEntity = new ETemasql(numerotemalong, numerolong, usuariolong, nombretema);
                ETemasql savedTema = temaRepositorysql.save(temaEntity);
                System.out.println("Tema guardado: " + savedTema.getNombre());

                // Procesar resúmenes...
                List<String> resumenes = servidor.DevolverListaArchivos(asignatura, tema);
                System.out.println("Resúmenes obtenidos: " + resumenes.size() + " para tema: " + tema);

                int numerolista = 0;
                for (String resumen : resumenes){
                    numerolista++;
                    System.out.println("Procesando resumen: " + resumen);
                    String[] partes3 = resumen.split("\\.", 2);

                    if (partes3.length < 2) {
                        System.out.println("¡ERROR: Formato incorrecto en resumen: " + resumen);
                        continue;
                    }

                    Long numeroresumenlong = (long) numerolista;
                    String nombreresumen = partes3[1];

                    EResumensql resumenEntity = new EResumensql(numeroresumenlong, numerotemalong, numerolong, usuariolong, nombreresumen);
                    EResumensql savedResumen = resumenRepositorysql.save(resumenEntity);
                    System.out.println("Resumen guardado: " + savedResumen.getNombre());
                }
            }
        }

    }}
