DROP DATABASE IF EXISTS AplicacionEstudio;
CREATE DATABASE AplicacionEstudio;
USE AplicacionEstudio;

-- Tu script SQL original aquí...
CREATE TABLE Usuario (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    usuario VARCHAR(25) UNIQUE,
    contraseña VARCHAR(25),
    email VARCHAR(25) UNIQUE
);

CREATE TABLE Asignatura(
    id_asignatura BIGINT,
    id_usuario BIGINT,
    nombre VARCHAR(200),
    PRIMARY KEY (id_asignatura, id_usuario),
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id) ON DELETE CASCADE
);

CREATE TABLE Tema(
    id_tema BIGINT,
    id_asignatura BIGINT,
    id_usuario BIGINT,
    nombre VARCHAR(200),
    PRIMARY KEY (id_tema, id_asignatura, id_usuario),
    FOREIGN KEY (id_asignatura, id_usuario) REFERENCES Asignatura(id_asignatura, id_usuario) ON DELETE CASCADE
);

CREATE TABLE Resumen(
    id_resumen BIGINT,
    id_tema BIGINT,
    id_asignatura BIGINT,
    id_usuario BIGINT,
    nombre VARCHAR(200),
    PRIMARY KEY (id_resumen, id_tema, id_asignatura, id_usuario),
    FOREIGN KEY (id_tema, id_asignatura, id_usuario) REFERENCES Tema(id_tema, id_asignatura, id_usuario) ON DELETE CASCADE
);

-- El resto de tus tablas...
INSERT INTO Usuario (id, usuario, contraseña, email) VALUES (1, "marcr", "1234", "m@gmail.com");
select * from Usuario;
#DELETE FROM Usuario WHERE id = 3;
select * from Asignatura;
select * from Tema;
select * from Resumen;