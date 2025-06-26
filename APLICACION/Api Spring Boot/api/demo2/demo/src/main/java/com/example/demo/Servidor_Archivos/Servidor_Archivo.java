package com.example.demo.Servidor_Archivos;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

public class Servidor_Archivo {

/**
 * CLASSE PARA INTERACTUAR CON EL SERVIDOR DE ARCHIVOS
 * ARCHIVOS = RESUMENES DE MATERIAS
 * */


//Atributos
    public Usuario usuario;

//Constructores

    /**
     * constructor vacio
     */
    public Servidor_Archivo() {
    }

//Metodos
//Devolver lista de los archivos/temas/assignaturas....
    /**
     * Devolver Archvios de un tema
     * @param Assignatura 'nombre completo'
     * @param Tema 'nombre completo'
     * @return "List<int String>" "int = numero del archivo | String = nombre"*/
    public List<String> DevolverListaArchivos(String Assignatura, String Tema) {

        String ruta = RutaServidor.rutaServidor + "Servidor de Archivos\\" + usuario.getIdusuario() + "\\" + Assignatura + "\\" + Tema;

        File carpeta = new File(ruta);
        List<String> nombresArchivos = new ArrayList<>();

        if (carpeta.exists() && carpeta.isDirectory()) {
            File[] archivos = carpeta.listFiles();
            if (archivos != null) {
                for (File archivo : archivos) {
                    if (archivo.isFile()) {
                        nombresArchivos.add(archivo.getName());
                    }
                }
            }
        }

        return nombresArchivos;
    }

    /**
     * Devolver Temas de una Assignatura
     * @param Assignatura 'nombre completo'
     * @return "List<int String>" "int = numero del archivo | String = nombre"*/
    public List<String> DevolverListaTemas(String Assignatura){
        String ruta = RutaServidor.rutaServidor + "Servidor de Archivos\\" + usuario.getIdusuario() + "\\" + Assignatura;

        File carpeta = new File(ruta);
        List<String> nombresTemas = new ArrayList<>();

        if (carpeta.exists() && carpeta.isDirectory()) {
            File[] archivos = carpeta.listFiles();
            if (archivos != null) {
                for (File archivo : archivos) {
                    if (archivo.isDirectory()) {
                        nombresTemas.add(archivo.getName());
                    }
                }
            }
        }

        return nombresTemas;

    }

    /**
     * Devolver Assignaturas de un Usuario
     * @return "List<int String>" "int = numero del archivo | String = nombre"*/
    public List<String> DevolverListaAssignaturas(){
        String ruta = RutaServidor.rutaServidor + "Servidor de Archivos\\" + usuario.getIdusuario();

        System.out.println(ruta);
        File carpeta = new File(ruta);
        List<String> nombresAssignaturas = new ArrayList<>();

        if (carpeta.exists() && carpeta.isDirectory()) {
            File[] archivos = carpeta.listFiles();
            if (archivos != null) {
                for (File archivo : archivos) {
                    System.out.println(archivo.getName());
                    if (archivo.isDirectory()) {
                        nombresAssignaturas.add(archivo.getName());
                    }
                }
            }
        }
        return nombresAssignaturas;
    }

    /**
     * Metodo para crear usuario y gaurdarlo en properties
     */
    public void crearUsuarioyIniciarlo(){
        //Iniciar usuario por defecto
        Usuario usuario = new Usuario("marcrami", "12345", "m@gmail.com");
        usuario.setIdusuario(1);
        usuario.guardaridusuarioiniciado();
        this.usuario = usuario;
    }

//Otros metodos
    /**
     * Metodo para buscar el ultimo id, sirve solo para carpetas
     * @param rutaPadre 'pasar la ruta de la carpeta que contiene los archivos a recorrer'
     */
    //metodo para buscar el ultimo numero de la carpeta:
    public int buscarUltimoId(String rutaPadre){
        File carpeta = new File(rutaPadre);

        //Recorrer los directorios y guardar el id mas alto en la variable numeromaximo
        int numeromaximo = 0;

        if (carpeta.exists() && carpeta.isDirectory()) {
            File[] archivos = carpeta.listFiles();
            if (archivos != null) {
                for (File archivo : archivos) {
                    if (archivo.isDirectory()) {
                        String nombre = archivo.getName();

                        String numero = nombre.split("\\.")[0];
                        int num = Integer.parseInt(numero);
                        if(num > numeromaximo){
                            numeromaximo = num;
                        }
                    }}}}
        if(numeromaximo != 0){ //si no es 0 poner a 1
            numeromaximo++;
        }
        return numeromaximo;
    }


    }


