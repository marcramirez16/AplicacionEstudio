DROP DATABASE IF EXISTS AplicacionEstudio;
CREATE DATABASE AplicacionEstudio;


USE AplicacionEstudio;

CREATE TABLE Usuario (
    id INT PRIMARY KEY AUTO_INCREMENT,
    usuario VARCHAR(25),
    contraseña VARCHAR(25),
    correo VARCHAR(25)
);

DELETE FROM Usuario WHERE id = 3;
select * from Usuario;
