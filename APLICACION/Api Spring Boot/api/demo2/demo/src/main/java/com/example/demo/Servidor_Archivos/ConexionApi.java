package com.example.demo.Servidor_Archivos;

import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.StandardCharsets;

public class ConexionApi {

    public void enviarUsuario(String nombre, String contrasena, String email) {
        try {
            URL url = new URL("http://localhost:8080/api/usuarios"); // Tu endpoint REST
            HttpURLConnection con = (HttpURLConnection) url.openConnection();
            con.setRequestMethod("POST");
            con.setRequestProperty("Content-Type", "application/json");
            con.setDoOutput(true);

            // Construir el JSON del usuario
            String jsonInputString = String.format("""
                {
                    "usuario": "%s",
                    "contraseña": "%s",
                    "email": "%s"
                }
                """, nombre, contrasena, email);

            try (OutputStream os = con.getOutputStream()) {
                byte[] input = jsonInputString.getBytes(StandardCharsets.UTF_8);
                os.write(input, 0, input.length);
            }

            int responseCode = con.getResponseCode();
            System.out.println("Respuesta del servidor: " + responseCode);
            con.disconnect();

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
