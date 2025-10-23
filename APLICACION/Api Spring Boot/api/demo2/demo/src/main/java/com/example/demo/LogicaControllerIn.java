package com.example.demo;

import com.example.demo.Controladores.*;
import com.example.demo.Entidades.*;
import com.example.demo.OtrosProyectos.EjecutarTeclado;
import com.example.demo.Servidor_Archivos.*;
import jakarta.persistence.EntityManager;
import jakarta.persistence.PersistenceContext;
import jakarta.transaction.Transactional;
import org.apache.commons.math3.analysis.function.Asin;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.data.repository.query.Param;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Base64;
import java.util.List;
import java.util.Map;
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

    @Autowired
    private PreguntaRepository preguntaRepository;

    @Autowired
    private RespuestaRepository respuestaRepository;

    @Autowired
    private PasoRepository pasoRepository;

    @Autowired
    private OperacionRepository operacionRepository;

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

        System.out.println("-------------------");
        System.out.println("archivo seleccionado: " + archivo.getIdArchivo() + " nombre: " + archivo.getNombreArchivo() + " ruta: " + archivo.getRutaArchivo());
        System.out.println("-------------------");

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

        // Limpiar datos existentes en orden inverso (por dependencias de FK)
        resumenRepositorysql.deleteByIdUsuario(longid);
        temaRepositorysql.deleteByIdUsuario(longid);
        asignaturaRepositorysql.deleteByIdUsuario(longid);

        List<String> asignaturas = servidor.DevolverListaAssignaturas();

        if (asignaturas.isEmpty()) {
            System.out.println("¡ADVERTENCIA: No se encontraron asignaturas!");
            return; // Salir si no hay datos
        }

        for(String asignatura : asignaturas){
            String[] partes = asignatura.split("\\.", 2);

            // Verifica que el split funcione correctamente
            if (partes.length < 2) {
                continue;
            }

            Long numerolong = Long.parseLong(partes[0]);
            String nombre = partes[1];
            Long usuariolong = longid;


            // Crear y guardar asignatura
            EAsignaturasql asignaturaEntity = new EAsignaturasql(numerolong, usuariolong, nombre);
            EAsignaturasql savedAsignatura = asignaturaRepositorysql.save(asignaturaEntity);

            // Procesar temas...
            List<String> temas = servidor.DevolverListaTemas(asignatura);

            for (String tema : temas){
                String[] partes2 = tema.split("\\.", 2);

                if (partes2.length < 2) {
                    continue;
                }

                Long numerotemalong = Long.parseLong(partes2[0]);
                String nombretema = partes2[1];


                ETemasql temaEntity = new ETemasql(numerotemalong, numerolong, usuariolong, nombretema);
                ETemasql savedTema = temaRepositorysql.save(temaEntity);

                // Procesar resúmenes...
                List<String> resumenes = servidor.DevolverListaArchivos(asignatura, tema);

                int numerolista = 0;
                for (String resumen : resumenes){
                    numerolista++;
                    String[] partes3 = resumen.split("\\.", 2);

                    if (partes3.length < 2) {
                        continue;
                    }

                    Long numeroresumenlong = (long) numerolista;
                    String nombreresumen = partes3[1];

                    EResumensql resumenEntity = new EResumensql(numeroresumenlong, numerotemalong, numerolong, usuariolong, nombreresumen);
                    EResumensql savedResumen = resumenRepositorysql.save(resumenEntity);
                }
            }
        }
    }

    /**
     * Metodo apra agregar pregunta nueva
     * @param preguntat "texto de la pregunta"
     * @param tipo "tipo de pregunta"
     * @return retorna el id generado de la nueva pregunta
     */
    @PostMapping("/AgregarPregunta")
    public Long agregarpregunta(@RequestParam String preguntat, @RequestParam String tipo, @RequestParam String imagen) {
        //usuario
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        //RetorarArchivoRuta
        Archivo archivo = new Archivo();
        archivo = archivo.RetorarArchivoRuta(servidor, servidor.retornarArchivoSeleccionado());

        EPregunta pregunta = new EPregunta(archivo.getIdArchivo(), archivo.getIdTema(), archivo.getIdAssignatura(), longid, preguntat, tipo);

        pregunta.setImagen(imagen);

        EPregunta preg = preguntaRepository.save(pregunta);

        return preg.getId_pregunta();
        }

    /**
     * Metodo para borrar la pregunta a partir de su id
     * @param idpregunta "el id de la pregunta a borrar"
     */
    @PostMapping("/BorrarPregunta")
    public void borrarpregunta(@RequestParam("idpregunta") Long idpregunta) {
        // usuario actual
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        // archivo activo
        Archivo archivo = new Archivo();
        archivo.RetorarArchivoRuta(servidor, servidor.retornarArchivoSeleccionado());

        // borrar
        preguntaRepository.deleteByIdPregunta(idpregunta);
    }

    /**
     * Metodo para remplazar pregunta a partir de su id
     */
    @PostMapping("/EditarPregunta")
    public ResponseEntity<String> editarPregunta(@RequestParam Long idpregunta, @RequestParam String pregunta, @RequestParam String tipo, @RequestParam String imagen) {

        preguntaRepository.updatePregunta(idpregunta, pregunta, tipo, imagen);
        return ResponseEntity.ok("Pregunta actualizada correctamente");
    }

    /**
     * Metood para agregar respuesta
     */
    @PostMapping("/AgregarRespuesta")
    public Long agregarrespuesta(@RequestParam Long idpregunta, @RequestParam String texto) {
        ERespuesta respuesta = new ERespuesta(idpregunta, texto);

        ERespuesta resp = respuestaRepository.save(respuesta);

        return resp.getId_respuesta();
    }

    /**
     * Metodo para borrar la respuesta a partir de su id pregunta
     * @param idpregunta "el id de la pregunta para orrar su respuesta"
     */
    @PostMapping("/BorrarRespuesta")
    public boolean borrarrespuesta(@RequestParam("idpregunta") Long idpregunta) {

        // borrar
        respuestaRepository.deleteByIdPregunta(idpregunta);
        return true;
    }

    /**
     * Metodo para editar la respuesta a partir de su id pregunta
     * @param idpregunta
     * @param nuevaRespuesta
     * @return
     */
    @PostMapping("/EditarRespuesta")
    public ResponseEntity<String> editarRespuesta(@RequestParam Long idpregunta, @RequestParam String nuevaRespuesta) {
        ERespuesta respuesta = respuestaRepository.findByIdPregunta(idpregunta);


        respuesta.setRespuesta(nuevaRespuesta);  // O el nombre del campo que uses para la respuesta
        respuestaRepository.save(respuesta);

        return ResponseEntity.ok("Respuesta actualizada correctamente");
    }


    /**
     * Metodo para agregar la imagen de la ecuacion en la carpeta Imagenes de la api
     */
    @PostMapping("/SubirImagenBase64")
    public ResponseEntity<String> subirImagenBase64(@RequestBody Map<String, String> payload) {
        try {
            String nombre = payload.get("nombre");
            String base64 = payload.get("imagen");

            byte[] bytes = Base64.getDecoder().decode(base64);
            Path ruta = Paths.get("imagenes/" + nombre);
            Files.createDirectories(ruta.getParent());
            Files.write(ruta, bytes);

            // Aquí podrías notificar al frontend, o simplemente devolver la imagen
            return ResponseEntity.ok(base64);

        } catch (Exception e) {
            e.printStackTrace();
            return ResponseEntity.status(500).body("Error al recibir imagen: " + e.getMessage());
        }
    }

    /**
     * Metood para agregar Paso
     */
    @PostMapping("/AgregarPaso")
    public Long agregarPaso(@RequestParam Long id_respuesta, @RequestParam Long numero, @RequestParam String texto) {
        EPaso paso = new EPaso(id_respuesta, numero, texto);

        EPaso resp = pasoRepository.save(paso);

        return resp.getId_respuesta();
    }

    /**
     * Metodo para agregar operacion
     */
    @PostMapping("/AgregarOperacion")
    public Long agregarOperacion(@RequestParam Long id_operacion, @RequestParam Long id_paso, @RequestParam String operacion) {
        EOperacion operaciono = new EOperacion(id_operacion, id_paso, operacion);

        EOperacion resp = operacionRepository.save(operaciono);

        return resp.getId_operacion();
    }


}

