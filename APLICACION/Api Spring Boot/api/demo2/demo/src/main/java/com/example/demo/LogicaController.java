package com.example.demo;

import com.example.demo.Servidor_Archivos.Assignatura;
import com.example.demo.Servidor_Archivos.Servidor_Archivo;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/api")
public class LogicaController {

    private final Ejemplo ejemplo = new Ejemplo();

    Servidor_Archivo servidor = new Servidor_Archivo();

    @GetMapping("/ejecutar")
    public String ejecutar(@RequestParam String param) {
        return ejemplo.ejecutarLogica(param);
    }

    @GetMapping("/sumar")
    public int sumar(@RequestParam int a, @RequestParam int b) {
        return ejemplo.sumar(a, b);
    }

    @GetMapping("/DevolverListaAssignaturas")
    public List<String> DevolverListaAssignaturas(){
        servidor.crearUsuarioyIniciarlo();
        return servidor.DevolverListaAssignaturas();
    }

}