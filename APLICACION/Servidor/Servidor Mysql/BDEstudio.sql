DROP DATABASE IF EXISTS AplicacionEstudio;
CREATE DATABASE AplicacionEstudio;


USE AplicacionEstudio;

CREATE TABLE Usuario (
    id INT PRIMARY KEY AUTO_INCREMENT,
    usuario VARCHAR(25) UNIQUE,
    contraseña VARCHAR(25),
    email VARCHAR(25) UNIQUE
);


#DELETE FROM Usuario WHERE id = 3;
select * from Usuario;
