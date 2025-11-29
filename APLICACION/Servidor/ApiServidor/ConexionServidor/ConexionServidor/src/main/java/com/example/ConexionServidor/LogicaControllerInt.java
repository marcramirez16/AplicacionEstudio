package com.example.ConexionServidor;

import com.example.ConexionServidor.Controladores.UsuarioRepository;
import com.example.ConexionServidor.Entidades.EUsuario;
import com.example.ConexionServidor.Servidor_Archivos.*;
import jakarta.persistence.EntityManager;
import jakarta.persistence.PersistenceContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Optional;


@RestController
@RequestMapping("/api")
public class LogicaControllerInt {

    Servidor_Archivo servidor = new Servidor_Archivo();

    @PersistenceContext
    private EntityManager entityManager;
    /**
     * Poner los controladores
     */
    @Autowired
    private UsuarioRepository usuarioRepository;

    /**
     * Retornar usuario: Iniciar session de un usuario "usuario, contraseña"
     * @param usuario
     */
    @PostMapping("/iniciarusuario")
    public ResponseEntity<?> iniciarSession(@RequestBody EUsuario usuario) {
        //Buscar usuario por contraseña y usuario
        Optional<EUsuario> usuarioEncontrado = usuarioRepository.findByUsuarioAndContrasena(
                usuario.getUsuario(), usuario.getContrasena()
        );

        System.out.println("conexion");
        System.out.println("usuario: " + usuario.getUsuario() + " contraseña: " + usuario.getContrasena() + " mail: " + usuario.getEmail() + " id: " + usuario.getId());
        if (usuarioEncontrado.isPresent()) {
            //Usuario iniciado con Exito.
            // Guardarlo en properties para hacerlo persistente
            EUsuario usuarioentidadresp = usuarioEncontrado.get();
            Usuario usuarion = new Usuario(usuarioentidadresp.getId(), usuarioentidadresp.getUsuario(), usuarioentidadresp.getEmail(), usuarioentidadresp.getContrasena());
            usuarion.guardaridusuarioiniciado(); //guardar usuario iniciado

            //Retornar el usuario entity al frontend
            System.out.println("usuario encontrado: ----------");
            System.out.println(usuarioEncontrado.get().getUsuario());
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
     *  crear un usuario en el backend "carpeta usuario"
     * @param usuario 'entidad '
     * @return respuesta en insertar
     */
    @PostMapping("/crearusuario")
    public String insertarUsuario(@RequestBody EUsuario usuario) {

        System.out.println("Llega usuario: " + usuario.getUsuario());
        System.out.println("Llega contraseña: " + usuario.getContrasena());
        System.out.println("Llega email: " + usuario.getEmail());

        //generar id con los datos de la entidad "usuario para crear carpeta"
        Usuario usuario2 = new Usuario(usuario.getUsuario(), usuario.getContrasena(), usuario.getEmail());


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
     * Metodo para seleccionar archivo actual
     * @param nombreAsignatura
     * @param nombreTema
     * @param nombreArchivo
     * @return
     */
    @PostMapping("/SeleccionarArchivo")
    public boolean SeleccionarArchivo(@RequestParam String nombreAsignatura, @RequestParam String nombreTema, @RequestParam String nombreArchivo){
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
}
