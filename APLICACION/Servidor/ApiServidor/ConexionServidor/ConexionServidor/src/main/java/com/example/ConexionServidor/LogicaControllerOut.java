package com.example.ConexionServidor;

import com.example.ConexionServidor.Controladores.PreguntaRepository;
import com.example.ConexionServidor.Controladores.RespuestaRepository;
import com.example.ConexionServidor.Entidades.EPregunta;
import com.example.ConexionServidor.Entidades.ERespuesta;
import com.example.ConexionServidor.Entidades.EUsuario;
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
     */
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


}

