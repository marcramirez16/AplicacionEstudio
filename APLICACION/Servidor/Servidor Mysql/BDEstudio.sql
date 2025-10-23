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

CREATE TABLE Pregunta(           /*no pongo foreign key para que no se borren...*/
	id_pregunta BIGINT PRIMARY KEY auto_increment,
	id_resumen BIGINT,
    id_tema BIGINT,
    id_asignatura BIGINT,
    id_usuario BIGINT,
    Tipo ENUM('mates', 'normal') NOT NULL,
    pregunta VARCHAR(800),
	imagen TEXT);

CREATE TABLE Respuesta(
	id_respuesta BIGINT PRIMARY KEY auto_increment,
    id_pregunta BIGINT,
    respuesta VARCHAR(800),
	FOREIGN KEY (id_pregunta) REFERENCES Pregunta(id_pregunta) ON DELETE CASCADE
);

CREATE TABLE PasoMates(
    id_paso INT PRIMARY KEY auto_increment,
	id_respuesta BIGINT,
    numero INT,
    TextoPaso VARCHAR(200),
    FOREIGN KEY(id_respuesta) REFERENCES Respuesta(id_respuesta) ON DELETE CASCADE
);

CREATE TABLE OperacionMates(
	id_operacion INT PRIMARY KEY auto_increment,
	id_paso INT,
    operacion VARCHAR(200),
    FOREIGN KEY(id_paso) REFERENCES PasoMates(id_paso) ON DELETE CASCADE
);

CREATE TABLE PasoNormal(
    id_paso INT PRIMARY KEY,
    id_respuesta BIGINT,
    Texto VARCHAR(200),
    FOREIGN KEY(id_respuesta) REFERENCES Respuesta(id_respuesta) ON DELETE CASCADE
);

-- El resto de tus tablas...
INSERT INTO Usuario (id, usuario, contraseña, email) VALUES (1, "marcr", "1234", "m@gmail.com");
select * from Usuario;
#DELETE FROM Usuario WHERE id = 3;
select * from Asignatura;
select * from Tema;
select * from Resumen;
select * from Pregunta;
select * from Respuesta;


/*
para crear preguntas de un resumen, recuerda seleccionar la parte del resumen del que quieres que haga la pregunta con chatgpt
*/

/*
matematicas: poner formula, "con calculadora escribir operacion por operacion para que el programa lo traduzca"
"al resolver la respuesta tambien con calculadora paso a paso, aver si se parece a la traduccion del programa"
*/