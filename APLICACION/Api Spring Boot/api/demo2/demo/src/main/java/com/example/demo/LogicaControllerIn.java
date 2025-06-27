package com.example.demo;

import com.example.demo.Controladores.UsuarioRepository;
import com.example.demo.Entidades.EUsuario;
import com.example.demo.Servidor_Archivos.Servidor_Archivo;
import com.example.demo.Servidor_Archivos.Usuario;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

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

//Metodos para insertar en la bd sql
    //METODOS USUARIO
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
        } catch (Exception e) {
            return "Error al guardar usuario o crear carpeta: " + e.getMessage();
        }

    }


}
