package com.example.demo.Servidor_Archivos;

import org.apache.poi.xwpf.usermodel.XWPFDocument;
import org.apache.poi.xwpf.usermodel.XWPFParagraph;

import java.awt.*;
import java.io.*;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.List;
import java.util.Properties;
import java.util.regex.Pattern;

import static com.example.demo.Servidor_Archivos.RutaServidor.FILE_NAME;

public class Archivo extends Tema{

//Atributos
    private long idArchivo;

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
    public Archivo(long idusuario, long idAssignatura, String nombreAssignatura, long idTema, String nombreTema, long idArchivo, String rutaArchivo, String nombreArchivo) {
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

        System.out.println("nombre archivo: " + nombreArchivo);
        String[] partes = nombreArchivo.split("\\.");
        this.idArchivo = Long.parseLong(partes[0]);

        this.solonombreArchivo = partes[1];

        this.nombreArchivo = nombreArchivo;
        this.rutaPadreArchivo = tema.getRutaTema();
        this.rutaArchivo = this.rutaPadreArchivo + "\\" +  this.nombreArchivo;
        System.out.println("nombreArchivo: " + this.nombreArchivo + "ruta: " + this.rutaArchivo + " ruta tema: " + this.rutaPadreArchivo);
    }
    public Archivo(){

    }
    /**
     * Metodo para retornar el archivo a partir de su ruta completa
     * @param rutaArchivo
     * @return
     */
    public Archivo RetorarArchivoRuta(Servidor_Archivo servidor, String rutaArchivo){

        System.out.println("Ruta archivo: " + rutaArchivo);
        Path path = Paths.get(rutaArchivo);
        int nameCount = path.getNameCount();

        String dato1 = path.getName(nameCount - 3).toString(); //nombreAsignatura
        String dato2 = path.getName(nameCount - 2).toString(); //nomretema
        String dato3 = path.getName(nameCount - 1).toString(); //nombreArchivo

        String idusuario = servidor.obteneridusuarioiniciado();

        int idusuarioint = Integer.parseInt(idusuario);

        servidor.usuario.setIdusuario(idusuarioint);

        Assignatura asignatura = new Assignatura(servidor.usuario, dato1);
        Tema tema = new Tema(asignatura, dato2);

        Archivo archivo2= new Archivo(tema, dato3);

        return archivo2;
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

        this.nombreArchivo = crearNombreArchivoNuevo(tema);
        String[] partes = nombreArchivo.split("\\.");
        this.idArchivo = Long.parseLong(partes[0]);

        this.rutaPadreArchivo = tema.getRutaTema();
        this.rutaArchivo = this.rutaPadreArchivo + "\\" + this.nombreArchivo;
    }
//Metodos

    /**
     * lamar al metodo buscarUltimoId para crear nuevo id maximo
     * @return nombre 'retornar nombre crado "id + nombre"'
     */
    //Crear un nuevo nombre con un id superior al maximo...
    public String crearNombreArchivoNuevo(Tema tema){
        //Saver el id de la ultima Assignatura
        long ultimoid = buscarUltimoId(tema.getRutaTema());

        //agregar ruta con su +id.nombre+
        String nombre = ultimoid + "." + this.solonombreArchivo;

        return nombre;
    }


    /**
     * Agregar un archivo docx, comprovar antes si existe y su nombre no contiene errores...
     * @param 'archivo'
     **/
    public boolean agregarArchivo() {


        // Comprobar que el archivo no existe y que tiene un nombre válido
        File archivoDoc = new File(this.rutaArchivo);

        //Comprobar que el nombre no contiene "."
        // Validación: el nombre del archivo no debe contener puntos (excepto la extensión)
        String nombreArchivo = archivoDoc.getName(); // solo el nombre con extensión

        // Asegurar que tenga extensión .docx
        if (!nombreArchivo.toLowerCase().endsWith(".docx")) {
            nombreArchivo += ".docx";
            this.rutaArchivo += ".docx";
            archivoDoc = new File(this.rutaArchivo);
        }

        // Contar cuántos puntos (.) hay en el nombre del archivo
        int cantidadPuntos = nombreArchivo.length() - nombreArchivo.replace(".", "").length();

        // Solo permitimos 2 puntos: uno para separar número/título y otro para ".docx"
        if (cantidadPuntos != 2) {
            return false;
        }

        //comprobar si el documento ya existe
        //String[] partes = nombre.split("\\."); comprobarlo asi mejor...
        if (archivoDoc.exists()) {
            return false;
        }

        // Validación opcional: asegurar que termina en .docx
        if (!this.rutaArchivo.toLowerCase().endsWith(".docx")) {
            this.rutaArchivo += ".docx"; // Agrega la extensión si no está
            archivoDoc = new File(this.rutaArchivo);
        }

        // Crear documento Word válido
        XWPFDocument documento = new XWPFDocument();

        // Crear un párrafo vacío o con texto por defecto (opcional)
        XWPFParagraph parrafo = documento.createParagraph();
        parrafo.createRun().setText("Nuevo documento Word generado automáticamente.");

        try (FileOutputStream out = new FileOutputStream(archivoDoc)) {
            documento.write(out);
            return true;
        } catch (IOException e) {
            e.printStackTrace(); // Ayuda a depurar errores
            return false;
        }
    }


    /**
     * Metodo para borrar archivo docx
     * @return
     */
    public boolean borrarArchivo() {
        long idmax = this.idArchivo; // Obtener el id
        File archivoDocx = new File(this.rutaArchivo);

        try {
            if (archivoDocx.exists() && archivoDocx.delete()) {
                // Luego renombrar los demás archivos en el mismo directorio
                File[] archivos = archivoDocx.getParentFile().listFiles();
                if (archivos != null) {
                    for (File archivo : archivos) {
                        if (archivo.isFile()) {
                            String[] partes = archivo.getName().split("\\.", 2);
                            try {
                                long numeroid = Long.parseLong(partes[0]);
                                if (numeroid > idmax) {
                                    long nuevoNumero = numeroid - 1;
                                    String nuevoNombre = nuevoNumero + "." + partes[1];
                                    File archivoNuevo = new File(archivo.getParent(), nuevoNombre);
                                    archivo.renameTo(archivoNuevo);
                                }
                            } catch (NumberFormatException e) {
                                // Ignorar archivos que no comiencen con un número
                            }
                        }
                    }
                }
                return true;
            } else {
                return false;
            }

        } catch (Exception e) {
            return false;
        }
    }

    /**
     * Metodo para guardar el archivo seleccionado en properties
     */
    public boolean seleccionarArchivo() {
        // Primero deseleccionas
        DeseleccionarArchivo();
        System.out.println("he guardado: " + this.rutaArchivo);

        Properties props = new Properties();

        // Carga las propiedades actuales
        try (InputStream in = new FileInputStream(FILE_NAME)) {
            props.load(in);
        } catch (IOException e) {
            // Puedes ignorar si no existe aún
        }

        // Agrega la nueva propiedad
        String rutaArchivo = String.valueOf(this.rutaArchivo);
        props.setProperty("archivoSeleccionado", rutaArchivo);

        // Guarda todo (incluyendo lo anterior)
        try (OutputStream out = new FileOutputStream(FILE_NAME)) {
            props.store(out, "Archivo Seleccionado");
            return true;
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * Metodo para deseleccionar el archivo actual
     * @return
     */
    public String DeseleccionarArchivo() {
        Properties props = new Properties();

        try (InputStream in = new FileInputStream(FILE_NAME)) {
            props.load(in);
        } catch (IOException e) {
            return "No se pudo cargar el archivo de sesión.";
        }

        props.remove("archivoSeleccionado");

        try (OutputStream out = new FileOutputStream(FILE_NAME)) {
            props.store(out, "Archivo deseleccionado");
            return "archivo deseleccionado correctamente.";
        } catch (IOException e) {
            return "Error al deseleccionar los cambios.";
        }
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
    public Long buscarUltimoId(String rutaPadre){
        File carpeta = new File(rutaPadre);

        //Recorrer los directorios y guardar el id mas alto en la variable numeromaximo
        long numeromaximo = 0;

        if (carpeta.exists() && carpeta.isDirectory()) {
            File[] archivos = carpeta.listFiles();
            if (archivos != null) {
                for (File archivo : archivos) {
                    if (archivo.isFile()) {
                        String nombre = archivo.getName();

                        String numero = nombre.split("\\.")[0];
                        long num = Long.parseLong(numero);
                        if(num > numeromaximo){
                            numeromaximo = num;
                        }
                    }}}}

        numeromaximo++;
        return numeromaximo;
    }



    //Getters Y setters
    public String getNombreArchivo() {
        return nombreArchivo;
    }

    public void setNombreArchivo(String nombreArchivo) {
        this.nombreArchivo = nombreArchivo;
    }

    public String getRutaArchivo() {
        return rutaArchivo;
    }

    public void setRutaArchivo(String rutaArchivo) {
        this.rutaArchivo = rutaArchivo;
    }
}
