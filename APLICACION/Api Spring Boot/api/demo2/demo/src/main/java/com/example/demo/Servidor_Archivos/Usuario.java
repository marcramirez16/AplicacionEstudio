package com.example.demo.Servidor_Archivos;

import java.io.*;
import java.util.Properties;

import static com.example.demo.Servidor_Archivos.RutaServidor.FILE_NAME;

public class Usuario {

//Atributos
    private long idusuario;
    private String nombreUsuario;
    private String correo;
    private String contraseña;

    private String rutaUsuario;
    private String rutaPadre = RutaServidor.rutaServidor + "Servidor de Archivos";

//constructores sobrecarga

    public Usuario(){}

    /**
     * constructor para crear usuario a partir de parametros
     * @param idusuario
     * @param nombreUsuario
     * @param correo
     * @param contraseña
     */
    //crear usuario por parametros
    public Usuario(long idusuario, String nombreUsuario, String correo, String contraseña) {
        this.idusuario = idusuario;
        this.nombreUsuario = nombreUsuario;
        this.correo = correo;
        this.contraseña = contraseña;

        this.rutaUsuario = RutaServidor.rutaServidor + "Servidor de Archivos\\" + this.getIdusuario();
    }

    /**
     * constructor para recuperar el usuario. Busca a partir del usuario y contraseña en la bd
     * @param nombreUsuario
     * @param contraseña
     */
    //recuperar usuario en la bd
    public Usuario(String nombreUsuario, String contraseña){
        //Comprovar el usuario y contraseña en sql
        //despues comprovar su carpeta y ruta...

    }

    /**
     * constructor para recuperar el usuario. Busca a partir del id en la bd
     * @param idusuario
     */
    //metodo para solo obtener el usuario solo a partir de su id, entrando en la bd
    public Usuario(long idusuario){
        this.idusuario = idusuario;
        this.rutaUsuario = RutaServidor.rutaServidor + "Servidor de Archivos\\" + this.getIdusuario();

    }

    /**
     * Constructor para crear nuevo usuario, crea el id automaticamente
     * @param nombreUsuario
     * @param contraseñaUsuario
     * @param correo
     */
    //crear usuario "crear id automaticamente"
    public Usuario(String nombreUsuario, String contraseñaUsuario, String correo){
        //tambien ver en el sql
        this.idusuario = buscarUltimoId(this.rutaPadre);
        this.nombreUsuario = nombreUsuario;
        this.contraseña = contraseñaUsuario;
        this.correo = correo;


    }



    /**
     * Metodo para guardar el usuario en properties
     */
    public void guardaridusuarioiniciado(){
        Properties props = new Properties();
        String idstring = String.valueOf(this.idusuario);
        props.setProperty("idusuario", idstring);
        try (OutputStream out = new FileOutputStream(FILE_NAME)) {
            props.store(out, "User session");
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    /**
     * Metodo para obtener el id usuario iniciado
     */
    public static String obteneridusuarioiniciado() {
        Properties props = new Properties();
        try (InputStream in = new FileInputStream(FILE_NAME)) {
            props.load(in);
            return props.getProperty("idusuario");
        } catch (IOException e) {
            return null;
        }
    }


    /**
     * Metodo para buscar el ultimo id del usuario maximo, este recorre todas las carpetas de usuario para encontrarlas
     * @param 'ruta del servidor, alli se encuentran todos los usuarios'
     * @return retorna el nuemro del usuario correspondiente al ultimo
     */
    public Long buscarUltimoId(String rutaPadre){
        File carpeta = new File(rutaPadre);

        //Recorrer los directorios y guardar el id mas alto en la variable numeromaximo
        long numeromaximo = 0;

        if (carpeta.exists() && carpeta.isDirectory()) {
            File[] archivos = carpeta.listFiles();
            if (archivos != null) {
                for (File archivo : archivos) {
                    if (archivo.isDirectory()) {
                        String nombre = archivo.getName();

                        String numero;
                        if (nombre.contains(".")) {
                            numero = nombre.split("\\.")[0];
                        } else {
                            numero = nombre;
                        }
                        long num = Long.parseLong(numero);

                        //Long num = Integer.parseInt(numero);
                        if(num > numeromaximo){
                            numeromaximo = num;
                        }
                    }}}}
        if (numeromaximo != 0) { //si no es 0 poner a 1
            numeromaximo++;
        }

        return numeromaximo;
    }



    //getters y setters

    public long getIdusuario() {
        return idusuario;
    }

    public void setIdusuario(long idusuario) {
        this.idusuario = idusuario;
    }

    public String getNombre() {
        return nombreUsuario;
    }

    public void setNombre(String nombreUsuario) {
        this.nombreUsuario = nombreUsuario;
    }

    public String getCorreo() {
        return correo;
    }

    public void setCorreo(String correo) {
        this.correo = correo;
    }

    public String getContraseña() {
        return contraseña;
    }

    public void setContraseña(String contraseña) {
        this.contraseña = contraseña;
    }

    public String getRutaUsuario() {
        return rutaUsuario;
    }
}
