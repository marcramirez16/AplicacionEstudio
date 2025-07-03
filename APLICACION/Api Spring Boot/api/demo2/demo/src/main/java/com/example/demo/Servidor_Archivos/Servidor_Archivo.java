package com.example.demo.Servidor_Archivos;

import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

import static com.example.demo.Servidor_Archivos.RutaServidor.FILE_NAME;

public class Servidor_Archivo {

/**
 * CLASSE PARA INTERACTUAR CON EL SERVIDOR DE ARCHIVOS
 * ARCHIVOS = RESUMENES DE MATERIAS
 * */


//Atributos
    public Usuario usuario;
    ConexionApi conexion;

//Constructores

    /**
     * constructor vacio
     */
    public Servidor_Archivo() {
        usuario = new Usuario();
        conexion = new ConexionApi();
    }

//Metodos
    /**
     * Metodo para obtener el id usuario iniciado
     */
    public String obteneridusuarioiniciado() {
        Properties props = new Properties();
        try (InputStream in = new FileInputStream(FILE_NAME)) {
            props.load(in);
            return props.getProperty("idusuario");
        } catch (IOException e) {
            return null;
        }
    }

    /**
     * Metodo para Cerrar sesion, borrar el id usuario
     */
    public String cerrarUsuario() {
        Properties props = new Properties();

        try (InputStream in = new FileInputStream(FILE_NAME)) {
            props.load(in);
        } catch (IOException e) {
            return "No se pudo cargar el archivo de sesión.";
        }
        props.remove("idusuario");

        try (OutputStream out = new FileOutputStream(FILE_NAME)) {
            props.store(out, "User session - ID eliminado");
            return "Sesión cerrada correctamente.";
        } catch (IOException e) {
            return "Error al guardar los cambios.";
        }
    }

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

        // Ordenar la lista por numeros
        nombresArchivos.sort((a, b) -> {
            try {
                // Extraer el número antes del primer punto
                int numA = Integer.parseInt(a.substring(0, a.indexOf('.')));
                int numB = Integer.parseInt(b.substring(0, b.indexOf('.')));
                return Integer.compare(numA, numB);
            } catch (Exception e) {
                // Si falla la conversión o no tiene punto, poner al final o al inicio
                return a.compareTo(b);
            }
        });

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
     * Metodo para crear carpeta usuario en el servidor de archvios usuario.
     * @param usuario
     */
    public boolean crearCarpetaUsuario(Usuario usuario){
        String ruta = RutaServidor.rutaServidor + "Servidor de Archivos\\" + usuario.getIdusuario();

        File carpeta = new File(ruta);

        if (!carpeta.exists()) {  // Comprueba si ya existe la carpeta
            boolean creada = carpeta.mkdirs(); // Crea la carpeta
            if (creada) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
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


