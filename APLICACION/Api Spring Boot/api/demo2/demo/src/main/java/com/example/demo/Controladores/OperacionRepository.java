package com.example.demo.Controladores;

import com.example.demo.Entidades.EOperacion;
import com.example.demo.Entidades.EPregunta;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;


@Repository
public interface OperacionRepository extends JpaRepository<EOperacion, Long> {



}