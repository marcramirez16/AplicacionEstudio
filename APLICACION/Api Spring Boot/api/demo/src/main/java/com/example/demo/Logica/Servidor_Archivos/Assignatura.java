package com.example.demo.Logica.Servidor_Archivos;

import java.io.File;

public class Assignatura extends Usuario{

//atributos
    private int idAssignatura;
    private String nombreAssignatura;

    private String solonombreAssignatura;

    private String rutaPadreAssignatura;
    private String rutaAssignatura;

//Sobrecarga constructores

    /**
     * Constructor para crear la assignatura a partir de parametros
     * @param idusuario
     * @param idAssignatura
     * @param nombreAssignatura
     */
    //constructor crear Assignatura por parametro
    public Assignatura(int idusuario, int idAssignatura, String nombreAssignatura) {
        super(idusuario);
        this.idAssignatura = idAssignatura;
        this.nombreAssignatura = nombreAssignatura;

        String[] partes = nombreAssignatura.split("\\.");
        this.solonombreAssignatura = partes[1];

        this.rutaPadreAssignatura = super.getRutaUsuario();
        this.rutaAssignatura = rutaPadreAssignatura + this.getNombreAssignatura();
    }

    /**
     * Metodo para recuperar la assignatura
     * @param usuario
     * @param nombreAssignatura 'id + nombreassignatura'
     */
    //constructor recuperar Assignatura
    public Assignatura(Usuario usuario, String nombreAssignatura){
        super(usuario.getIdusuario());
        //obtener id del nombre de la carpeta...
        String[] partes = nombreAssignatura.split("\\.");

        this.nombreAssignatura = nombreAssignatura;
        this.idAssignatura = Integer.parseInt(partes[0]);
        this.solonombreAssignatura = partes[1];

        this.rutaPadreAssignatura = super.getRutaUsuario();
        this.rutaAssignatura = rutaPadreAssignatura + this.getNombreAssignatura();

    }

    /**
     * constructor para crear una nueva assignatura
     * @param solonombreAssignatura
     * @param usuario
     */
    //constructor, util cuando cuando no hay nombre y "id". Se crea automaticamente...
    //constructor crear Assignatura "crea id automaticamente"
    public Assignatura(String solonombreAssignatura, Usuario usuario){
        super(usuario.getIdusuario());

        this.solonombreAssignatura = solonombreAssignatura;

        this.nombreAssignatura = crearNombreAssignaturaNueva();
        String[] partes = this.nombreAssignatura.split("\\.");
        this.idAssignatura = Integer.parseInt(partes[0]);

        this.rutaPadreAssignatura = super.getRutaUsuario();
        this.rutaAssignatura = rutaPadreAssignatura + this.getNombreAssignatura();

    }

    /**
     * Metodo para crear nuevo nombre de la assignatura con su id correspondiente
     * Este metodo se utliza con el metodo buscarUltimoId
     * @return nombre '+id y nombre+'
     */
    //Crear un nuevo nombre con un id superior al maximo...
    public String crearNombreAssignaturaNueva(){
        //Saver el id de la ultima Assignatura
        int ultimoid = buscarUltimoId(this.getRutaPadreAssignatura());

        //agregar ruta con su +id.nombre+
        String nombre = ultimoid + "." + this.solonombreAssignatura;

        return nombre;
    }

    /**
     * Agregar un Assignatura
     * @param 'nombre assignatura'
     **/
    public boolean agregarAssignatura(String nombre_assignatura){
        File carpeta = new File(this.rutaAssignatura);
        if (carpeta.mkdirs()) {
            System.out.println("Carpeta creada exitosamente en: " + carpeta.getAbsolutePath());
            return true;
        } else {
            System.out.println("No se pudo crear la carpeta o ya existe.");
            return false;
        }
    }

    /**
     * Metodo para retornar el nuevo id de la assignatura, recorre todas las assignaturas para ver id
     * @param 'ruta del usuario'
     * @return id de la assignatura nueva a crear
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


    //getters y setters
    public int getIdAssignatura() {
        return idAssignatura;
    }

    public void setIdAssignatura(int idAssignatura) {
        this.idAssignatura = idAssignatura;
    }

    public String getNombreAssignatura() {
        return nombreAssignatura;
    }

    public void setNombreAssignatura(String nombreAssignatura) {
        this.nombreAssignatura = nombreAssignatura;
    }


    public String getRutaPadreAssignatura() {
        return rutaPadreAssignatura;
    }

    public String getrutaAssignatura() {
        return rutaAssignatura;
    }
}
