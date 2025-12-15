package com.example.ConexionServidor;

import com.example.ConexionServidor.Controladores.*;
import com.example.ConexionServidor.Entidades.*;
import com.example.ConexionServidor.Servidor_Archivos.Archivo;
import com.example.ConexionServidor.Servidor_Archivos.Servidor_Archivo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api")
public class LogicaControllerOut {

    Servidor_Archivo servidor = new Servidor_Archivo();

    @Autowired
    PreguntaRepository preguntaRepository;

    @Autowired
    RespuestaRepository respuestaRepository;

    @Autowired
    PasoRepository pasoRepository;

    @Autowired
    PasoNormalRepository pasoNormalRepository;

    @Autowired
    OperacionRepository operacionRepository;

    @Autowired
    PasoSelectorRepository pasoSelectorRepository;

    @Autowired
    RespuestaSelectorRepository respuestaSelectorRepository;
    /**
     * Retornar usuario: Retornar el usuario iniciado
     */
    @GetMapping("/usuarioiniciado")
    public boolean usuarioIniciado() {
        String usuario = servidor.obteneridusuarioiniciado();
        return usuario != null;
    }

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
    public List<String> DevolverListaTemas(@RequestParam("asignatura") String asignatura) {
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        return servidor.DevolverListaTemas(asignatura);
    }

    /**
     * Metodo para devolver archivos
     * @param Asignatura
     * @param Tema
     * @return
     */
    @GetMapping("/DevolverListaArchivos")
    public List<String> DevolverListaArchivos( @RequestParam("asignatura") String Asignatura,
                                               @RequestParam("tema") String Tema) {
        //obtener id del usuario seleccionado
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        return servidor.DevolverListaArchivos(Asignatura, Tema);
    }


    /**
     * Metodo para retornar todas las preguntas del archivo
     * @return lista de preguntas EPregunta
     *//*
    @PostMapping("/ObtenerPreguntas")
    public List<EPregunta> obtenerPreguntas(){
        String stringid = servidor.obteneridusuarioiniciado();
        long longid = Long.parseLong(stringid);
        servidor.usuario.setIdusuario(longid);

        //RetorarArchivoRuta
        Archivo archivo = new Archivo();
        archivo = archivo.RetorarArchivoRuta(servidor, servidor.retornarArchivoSeleccionado());

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
     * Metodo para obtener paso
    */
    @GetMapping("/obtenerPasos")
    public List<EPaso> obtenerPasos(@RequestParam Long id_respuesta) {
        System.out.println("ID Respuesta recibido: " + id_respuesta);
        List<EPaso> pasos = pasoRepository.findById_respuesta(id_respuesta);
        System.out.println("Cantidad de pasos: " + pasos.size());
        return pasos;
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
     *//*
    @GetMapping("/ObtenerPasoNormal/{id_respuesta}")
    public String obtenerPasoNormal(@PathVariable Long id_respuesta) {
        Optional<EPasonormal> pasoOpt = pasoNormalRepository.findByIdRespuesta(id_respuesta);

        return pasoOpt.map(EPasonormal::getTexto).orElse(" ");
    }
*/

    /**
     * Metodo para obtener entidad respuesta
     * @param idpregunta
     * @return
     */
    @PostMapping("/ObtenerRespuestaCompleta")
    public ERespuesta obtenerRespuestaCompleta(Long idpregunta){
        ERespuesta respuesta = respuestaRepository.findByIdPregunta(idpregunta);

        if (respuesta == null) {
            return new ERespuesta();
        }

        return respuesta;
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

