package com.example.demo;

import com.example.demo.Controladores.*;
import com.example.demo.Entidades.*;
import com.example.demo.OtrosProyectos.EjecutarTeclado;
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
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Base64;
import java.util.List;
import java.util.Map;
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

    @Autowired
    private PreguntaRepository preguntaRepository;

    @Autowired
    private RespuestaRepository respuestaRepository;

    @Autowired
    private AsignaturaRepositorysql asignaturaRepositorysql;

    @Autowired
    private TemaRepositorysql temaRepositorysql;

    @Autowired
    private ResumenRepositorysql resumenRepositorysql;

    @Autowired
    private PasoRepository pasoRepository;

    @Autowired
    private OperacionRepository operacionRepository;

    @Autowired
    private PasoNormalRepository pasonormalRepository;

    @Autowired
    private PasoNormalRepository pasoNormalRepository;

    @Autowired
    private PasoSelectorRepository pasoSelectorRepository;

    @Autowired
    private RespuestaSelectorRepository respuestaSelectorRepository;
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

        borrarpreguntasassignatura(nombreAsignatura);

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

        borrarpreguntastema(nombreAsignatura, nombreTema);


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

       borrarpregutnasarchivo(nombreAsignatura, nombreTema, nombreArchivo);

            return archivo.borrarArchivo();
    }


//Metodos para borrar preguntas de una assignatura
    /**
     * Metodos para borrar las assignaturas y sus preguntas y respuesta del sql
     * @param nombreassignatura
     */
    public void borrarpreguntasassignatura(String nombreassignatura){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        String[] partes = nombreassignatura.split("\\.", 2);


        Long numerolong = Long.parseLong(partes[0]);
        String nombre = partes[1];
        Long usuariolong = longid;

        List<Long> idspreguntas = preguntaRepository.findIdsByIdAsignatura(numerolong);

        //borrar preguntas y respuestas
        for(Long id : idspreguntas){
            respuestaRepository.deleteByIdPregunta(id);
            preguntaRepository.deleteByIdPregunta(id);
        }
        //borrar asignatura, temas y resumenes de sql...
        asignaturaRepositorysql.deleteByIdAsignatura(numerolong, usuariolong);

    }

    /**
     * Metodo para borrar las preguntas y respuestas del tema + el tema y sus resumenes del sql
     * @param nombretema
     */
    public void borrarpreguntastema(String nombreassignatura, String nombretema){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        String[] partes = nombretema.split("\\.", 2);
        String[] partesass = nombreassignatura.split("\\.", 2);


        Long numerolong = Long.parseLong(partes[0]);
        Long numerolong2 = Long.parseLong(partesass[0]);

        String nombre = partes[1];
        Long usuariolong = longid;

        List<Long> idspreguntas = preguntaRepository.findIdsByIdTema(numerolong);

        //borrar preguntas y respuestas
        for(Long id : idspreguntas){
            respuestaRepository.deleteByIdPregunta(id);
            preguntaRepository.deleteByIdPregunta(id);
        }
        //borrar temas y resumenes de sql...
        temaRepositorysql.deleteByIdTema(numerolong, numerolong2, usuariolong);
    }

    /**
     * Metodo para borrar las preguntas y respuestas de un resumen
     * @param nombreassignatura
     * @param nombretema
     * @param nombrearchivo
     */
    public void borrarpregutnasarchivo(String nombreassignatura, String nombretema, String nombrearchivo){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        String[] partes = nombretema.split("\\.", 2);
        String[] partesass = nombreassignatura.split("\\.", 2);
        String[] partesarch = nombreassignatura.split("\\.", 2);

        Long numerolong = Long.parseLong(partes[0]); //tema
        Long numerolong2 = Long.parseLong(partesass[0]); //assignatura
        Long numerolong3 = Long.parseLong(partesarch[0]); //resumen

        String nombre = partes[1];
        Long usuariolong = longid;

        List<Long> idspreguntas = preguntaRepository.findIdsByIdResumen(numerolong);

        //borrar preguntas y respuestas
        for(Long id : idspreguntas){
            respuestaRepository.deleteByIdPregunta(id);
            preguntaRepository.deleteByIdPregunta(id);
        }

        //borrar temas y resumenes de sql...
        resumenRepositorysql.deleteByIdResumen(numerolong3, numerolong, numerolong2, usuariolong);
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

    /**
     * Metodo para retornar todas las preguntas del archivo
     * @return lista de preguntas EPregunta
     */
    /*
    @PostMapping("/ObtenerPreguntas")
    public List<EPregunta> obtenerPreguntas(){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        //RetorarArchivoRuta
        Archivo archivo = new Archivo();
        archivo = archivo.RetorarArchivoRuta(servidor, servidor.retornarArchivoSeleccionado());

        System.out.println("---------------------AQUI ESTA EL DEBUG");
        System.out.println("el archivo seleccionado es: " + " id: " + archivo.getIdArchivo() + " id asignatura: " + archivo.getIdAssignatura());
        List<EPregunta> preguntas = preguntaRepository.findByIdResumen(archivo.getIdArchivo());

        System.out.println("-------------------------------");
        System.out.println("idarchivo: " + archivo.getIdArchivo() + " usuario: " + longid);
        System.out.println(preguntas);
        return preguntas;
    }*/

    @PostMapping("/ObtenerPreguntas")
    public List<EPregunta> obtenerPreguntas(){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        //RetorarArchivoRuta
        Archivo archivo = new Archivo();
        archivo = archivo.RetorarArchivoRuta(servidor, servidor.retornarArchivoSeleccionado());

        Long idResumen = archivo.getIdArchivo();
        Long idTema = archivo.getIdTema();
        Long idAsignatura = archivo.getIdAssignatura();
        Long idUsuario = servidor.usuario.getIdusuario();
        List<EPregunta> preguntas = preguntaRepository.findPreguntas(
                idResumen,
                idTema,
                idAsignatura,
                idUsuario
        );
        System.out.println("-------------------------------");
        System.out.println("idarchivo: " + archivo.getIdArchivo() + " usuario: " + longid);
        System.out.println(preguntas);
        return preguntas;
    }



    /**
     * Metodo para obtener respuesta de la bd
     * @param idpregunta
     * @return
     */
    @PostMapping("/ObtenerRespuesta")
    public String obtenerRespuesta(Long idpregunta){

        ERespuesta respuestas = respuestaRepository.findByIdPregunta(idpregunta);


        if (respuestas == null) {
            return "_";
        }


        return respuestas.getRespuesta();
    }

    /**
     * Metodo para obtener entidad respuesta
     * @param idpregunta
     * @return
     */
    @PostMapping("/ObtenerRespuestaCompleta")
    public ERespuesta obtenerRespuestaCompleta(Long idpregunta){
        ERespuesta respuesta = respuestaRepository.findByIdPregunta(idpregunta);

        if (respuesta == null) {
            return new ERespuesta(); // O null, según tu lógica
        }

        return respuesta; // Esto se serializará automáticamente como JSON
    }

    /**
     * Metodo para obtener la imagen de la ecuacion echa con latex que esta en la carpeta Imagenes de la api
     * @param nombre
     * @return
     */
    @GetMapping("/ObtenerImagenBase64")
    public ResponseEntity<String> obtenerImagenBase64(@RequestParam String nombre) {
        try {
            Path ruta = Paths.get("imagenes/" + nombre);

            if (!Files.exists(ruta)) {
                return ResponseEntity.status(404).body("No se encontró la imagen.");
            }

            byte[] bytes = Files.readAllBytes(ruta);
            String base64 = Base64.getEncoder().encodeToString(bytes);


            return ResponseEntity.ok(base64);

        } catch (Exception e) {
            e.printStackTrace();
            return ResponseEntity.status(500).body("Error al obtener imagen: " + e.getMessage());
        }
    }
    /**
     * Metodo para abrir el creador de formulas LATEX echo en python para agregar ecuacion
     */
    @GetMapping("/abrirCalculadora")
    public ResponseEntity<Long> abrirCalculadora() {
        try {
            EjecutarTeclado teclado = new EjecutarTeclado();
            Process process = teclado.localizaruta();

            if (process != null) {
                long pid = process.pid();
                return ResponseEntity.ok(pid);

            }else{
                return ResponseEntity.status(500).body(0L);
            }
        } catch (Exception e) {
            e.printStackTrace();
            return ResponseEntity.status(500).body(0L);
        }
    }

    /**
     * Metodo para obtener paso
     */
    @PostMapping("/obtenerPasos")
    public List<EPaso> obtenerPasos(@RequestParam Long id_respuesta) {
        return pasoRepository.findById_respuesta(id_respuesta);
    }

    /**
     * Metodo para obtener operacion
     */
    @PostMapping("/obtenerOperaciones")
    public List<EOperacion> obtenerOperaciones(@RequestParam Long id_paso) {
        return operacionRepository.findById_paso(id_paso);
    }

    /**
     * Obtener paso normal
     * @return
     */
    @GetMapping("/ObtenerPasoNormal/{id_respuesta}")
    public String obtenerPasoNormal(@PathVariable Long id_respuesta) {
        Optional<EPasonormal> pasoOpt = pasoNormalRepository.findByIdRespuesta(id_respuesta);

        return pasoOpt.map(EPasonormal::getTexto).orElse(" ");
    }

    /**
     * Metodo para obtener pasos
     * @param id_respuesta
     * @return
     */
    @GetMapping("/obtenerPasosSelector")
    public List<EPasoSelector> obtenerPasosSelectors(@RequestParam Long id_respuesta) {
        return pasoSelectorRepository.findById_respuesta(id_respuesta);
    }

    /**
     * Metodo para obtener espuestas
     * @param id_paso
     * @return
     */
    @GetMapping("/obtenerRespuestasSelector")
    public List<ERespuestaSelector> obtenerRespuestasSelectors(@RequestParam Long id_paso) {
        return respuestaSelectorRepository.findById_paso(id_paso);
    }

}