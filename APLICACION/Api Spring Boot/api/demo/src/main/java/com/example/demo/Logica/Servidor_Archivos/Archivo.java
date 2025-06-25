package com.example.demo.Logica.Servidor_Archivos;

import org.apache.poi.xwpf.usermodel.XWPFDocument;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.regex.Pattern;

public class Archivo extends Tema{

//Atributos
    private int idArchivo;

    private String nombreArchivo;
    private String solonombreArchivo;

    private String rutaPadreArchivo;
    private String rutaArchivo;


//Sobrecarga Constructores
    /**
     * constructor para crear archivo nuevo a partir de parametros
     * @param idusuario
     * @param idAssignatura
     * @param nombreAssignatura
     * @param idTema
     * @param nombreTema
     * @param idArchivo
     * @param rutaArchivo
     * @param nombreArchivo
     */
    //constructor crear archivo por parametros "vijila el id, que no se sobrescriban..."
    public Archivo(int idusuario, int idAssignatura, String nombreAssignatura, int idTema, String nombreTema, int idArchivo, String rutaArchivo, String nombreArchivo) {
        super(idusuario, idAssignatura, nombreAssignatura, idTema, nombreTema);
        this.idArchivo = idArchivo;
        this.nombreArchivo = nombreArchivo;

        String[] partes = nombreArchivo.split("\\.");
        this.solonombreArchivo = partes[1];

        this.rutaPadreArchivo = super.getRutaTema();
        this.rutaArchivo = this.rutaPadreArchivo + this.nombreArchivo;

    }

    /**
     * Constructor para recuperar archivo
     * @param tema
     * @param nombreArchivo 'id + nombreArchivo'
     */
    //constructor recuperar archivo
    public Archivo(Tema tema, String nombreArchivo){
        super(tema.getIdusuario(), tema.getIdAssignatura(), tema.getNombreAssignatura(), tema.getIdTema(), tema.getNombreTema());

        String[] partes = nombreArchivo.split("\\.");
        this.idArchivo = Integer.parseInt(partes[0]);
        this.solonombreArchivo = partes[1];

        this.nombreArchivo = nombreArchivo;
        this.rutaPadreArchivo = super.getRutaTema();
        this.rutaArchivo = this.rutaPadreArchivo + this.nombreArchivo;

    }

    /**
     * Constructor para Crear una rchivo nuevo 'crea el id automaticamente'
     * @param solonombreArchivo
     * @param tema
     */
    //constructor, util cuando cuando no hay nombre y "id". Se crea automaticamente...
    //constructor para crear archivo nuevo "crea id automaticamente"
    public Archivo(String solonombreArchivo, Tema tema){
        super(tema.getIdusuario(), tema.getIdAssignatura(), tema.getNombreAssignatura(), tema.getIdTema(), tema.getNombreTema());

        this.solonombreArchivo = solonombreArchivo;

        this.nombreArchivo = crearNombreArchivoNuevo();
        String[] partes = nombreArchivo.split("\\.");
        this.idArchivo = Integer.parseInt(partes[0]);

        this.rutaPadreArchivo = super.getRutaTema();
        this.rutaArchivo = this.rutaPadreArchivo + this.nombreArchivo;

    }
//Metodos

    /**
     * lamar al metodo buscarUltimoId para crear nuevo id maximo
     * @return nombre 'retornar nombre crado "id + nombre"'
     */
    //Crear un nuevo nombre con un id superior al maximo...
    public String crearNombreArchivoNuevo(){
        //Saver el id de la ultima Assignatura
        int ultimoid = buscarUltimoId(this.getRutaTema());

        //agregar ruta con su +id.nombre+
        String nombre = ultimoid + "." + this.solonombreArchivo;

        return nombre;
    }

    /**
     * Agregar un archivo docx, comprovar antes si existe y su nombre no contiene errores...
     * @param 'archivo'
     **/
    public boolean agregarArchivo(Archivo archivo) {
        //Comprovar que el archivo no existe y no contenga nombres extranyos....
        File archivoDoc = new File(this.rutaArchivo);
        if (archivoDoc.exists()) {
            return false;
        }else{
            //No existe y el archivo puede agregarse!

            // Crear documento Word
            XWPFDocument documento = new XWPFDocument();

            // Ruta completa del archivo
            File archivodoc = new File(this.rutaArchivo);
            try (FileOutputStream out = new FileOutputStream(archivodoc)) {

                documento.write(out);
                return true;

            } catch (IOException e) {
                return false;
            }}
    }

    /**
     * Metodo para comprovar archivo, si tiene le formato adecuado
     * @return booleano si esta bien formado o no
     */
    public boolean comprovarArchivo(){
        String regex = "^[a-zA-Z0-9_-]+$";
        Pattern pattern = Pattern.compile(regex);
        return pattern.matcher(nombreArchivo).matches();
    }

    /**
     * @param 'pasar la ruta del tema para despues listar id de los archivos'
     * @return retorna el id nuevo que toca, uno superior al maximo
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
                    if (archivo.isFile()) {
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

    //Getters Y setters
    public String getNombreArchivo() {
        return nombreArchivo;
    }

    public void setNombreArchivo(String nombreArchivo) {
        this.nombreArchivo = nombreArchivo;
    }
}
