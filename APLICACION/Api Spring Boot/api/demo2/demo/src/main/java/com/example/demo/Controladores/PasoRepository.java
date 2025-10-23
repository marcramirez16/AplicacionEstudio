package com.example.demo.Controladores;


import com.example.demo.Entidades.EPaso;
import com.example.demo.Entidades.ERespuesta;
import jakarta.transaction.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

@Repository
public interface PasoRepository extends JpaRepository<EPaso, Long> {



}

