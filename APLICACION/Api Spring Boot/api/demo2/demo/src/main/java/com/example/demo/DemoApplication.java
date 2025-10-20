package com.example.demo;

import com.example.demo.OtrosProyectos.EjecutarTeclado;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class DemoApplication {

	//ejemplo ejecutar sumar:     http://localhost:8080/api/sumar?a=5&b=3
	public static void main(String[] args) {
		SpringApplication.run(DemoApplication.class, args);
	}


}
