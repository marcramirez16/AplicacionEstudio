package com.example.demo.Servidor_Archivos;

import java.io.File;

public class Tema extends Assignatura{

//atributos
    private long idTema;
    private String nombreTema;
    private String solonombreTema;
    private String rutaPadreTema;
    private String rutaTema;

//Sobrecarga constructores

    /**
     * constructor apra crear Tema a partir de parametros
     * @param idusuario
     * @param idAssignatura
     * @param nombreAssignatura
     * @param idTema
     * @param nombreTema
     */
    //constructor sin nombre y "id", se han de añadir por parametro
    //constructor crear tema por parametro "vijila el id agregado..."
    public Tema(long idusuario, long idAssignatura, String nombreAssignatura, long idTema, String nombreTema) {
        super(idusuario, idAssignatura, nombreAssignatura);
        this.idTema = idTema;
        this.nombreTema =  idTema +"." + this.nombreTema;

        String[] partes = nombreTema.split("\\.");
        this.solonombreTema = partes[1];

        this.rutaPadreTema = super.getrutaAssignatura();
        this.rutaTema = this.rutaPadreTema + "\\" + this.nombreTema;
    }

    /**
     * constructor para recuperar el tema
     * @param assignatura
     * @param nombreTema 'id + nombre'
     */
    //constructor, util cuando ya tenemos el nombre y "id"
    //constructor recuperar tema
    public Tema(Assignatura assignatura, String nombreTema){
        super(assignatura.getIdusuario(), assignatura.getIdAssignatura(), assignatura.getNombreAssignatura());
        String[] partes = nombreTema.split("\\.");
        this.idTema = Long.parseLong(partes[0]);
        this.solonombreTema = partes[1];
        this.nombreTema = nombreTema;

        this.rutaPadreTema = super.getrutaAssignatura();
        this.rutaTema = this.rutaPadreTema + "\\" + this.nombreTema;
    }

    /**
     * Constructor para crear un nuevo Tema 'crea el id automaticamente'
     * @param solonombreTema
     * @param assignatura
     */
    //constructor, util cuando cuando no hay nombre y "id". Se crea automaticamente...
    //constructor crear tema "crea id automaticamente"
    public Tema(String solonombreTema, Assignatura assignatura){
        super(assignatura.getIdusuario(), assignatura.getIdAssignatura(), assignatura.getNombreAssignatura());

        this.solonombreTema = solonombreTema;

        this.nombreTema = crearNombreTemaNuevo();
        String[] partes = nombreTema.split("\\.");
        this.idTema = Integer.parseInt(partes[0]);

        this.rutaPadreTema = super.getrutaAssignatura();
        this.rutaTema = this.rutaPadreTema + "\\" + this.nombreTema;
        System.out.println("-----La ruta del tema es la siguiente:" + this.rutaTema);
    }

    /**
     * Metodo para crear el nuevo nombre con su id y nombre
     * @return retornar nombre '+id nombre+'
     */
    //Crear un nuevo nombre con un id superior al maximo...
    public String crearNombreTemaNuevo(){
        //Saver el id de la ultima Assignatura
        long ultimoid = buscarUltimoId(this.getrutaAssignatura());

        //agregar ruta con su +id.nombre+
        String nombre = ultimoid + "." + this.solonombreTema;

        return nombre;
    }
    /**
     * Agregar un Tema
     * @param 'Assignatura del Tema'
     * @param 'nombre Tema'
     **/
    public boolean agregarTema(){
        File carpeta = new File(this.getRutaTema());
        if (carpeta.mkdirs()) {
            System.out.println("Carpeta creada exitosamente en: " + carpeta.getAbsolutePath());
            return true;
        } else {
            System.out.println("No se pudo crear la carpeta o ya existe.");
            return false;
        }
    }

    /**
     * Metodo para buscar el ultimo id del tema maximo
     * @param 'ruta assignaturas, donde estan los temas'
     * @return id para el nuevo tema
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

                        String numero = nombre.split("\\.")[0];
                        long num = Long.parseLong(numero);
                        if(num > numeromaximo){
                            numeromaximo = num;
                        }
                    }}}}
        if(numeromaximo == 0){ //si es 0 poner a 1
            numeromaximo++;
        }
        return numeromaximo;
    }

    //getters y setters
    public long getIdTema() {
        return idTema;
    }

    public void setIdTema(long idTema) {
        this.idTema = idTema;
    }

    public String getNombreTema() {
        return nombreTema;
    }

    public void setNombreTema(String nombreTema) {
        this.nombreTema = nombreTema;
    }


    public String getRutaPadreTema() {
        return rutaPadreTema;
    }

    public String getRutaTema() {
        return rutaTema;
    }
}
