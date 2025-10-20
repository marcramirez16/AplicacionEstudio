package com.example.demo.OtrosProyectos;

import java.io.BufferedReader;
import java.io.File;
import java.io.InputStreamReader;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Arrays;
import java.util.List;

public class EjecutarTeclado {


    public void localizaruta(){
        String raizProyecto = System.getProperty("user.dir");

        Path carpeta2 = Paths.get(raizProyecto);

        File carpet = carpeta2.toFile();
        String carp = carpet.getParent();
        File carpet2 = new File(carp);
        String carp2 = carpet2.getParent();
        File carpet3 = new File(carp2);
        String carp3 = carpet3.getParent();
        File carpet4 = new File(carp3);


        String ruta = carpet4.getPath() + "\\" + "aplicacionesOtras" + "\\" + "TecladoMatesp" + "\\" + "main.py";
        File carpet5 = new File(ruta);
        System.out.println(carpet5.exists());

        lanzarteclado(carpet5);
    }

    public void lanzarteclado(File file){

        try {
            // Comando para ejecutar Python
            List<String> comando = Arrays.asList(
                    "python", // o "python3" según tu sistema
                    file.getPath()
            );


            ProcessBuilder pb = new ProcessBuilder(comando);
            pb.redirectErrorStream(true); // fusiona stdout y stderr
            Process process = pb.start();

            // Leer salida del script Python si quieres (opcional)
            BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));
            String linea;
            while ((linea = reader.readLine()) != null) {
                System.out.println("[Python] " + linea);
            }

            process.waitFor(); // esperar a que termine si quieres

        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    }

