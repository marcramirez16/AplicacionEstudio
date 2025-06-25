package com.example.demo;

import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api")
public class LogicaController {

    private final Ejemplo ejemplo = new Ejemplo();

    @GetMapping("/ejecutar")
    public String ejecutar(@RequestParam String param) {
        return ejemplo.ejecutarLogica(param);
    }

    @GetMapping("/sumar")
    public int sumar(@RequestParam int a, @RequestParam int b) {
        return ejemplo.sumar(a, b);
    }
}