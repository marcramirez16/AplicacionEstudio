package com.example.demo.Controladores;

import com.example.demo.Entidades.EAsignatura;
import com.example.demo.Entidades.EResumen;
import com.example.demo.Entidades.EResumensql;
import com.example.demo.Entidades.ResumenId;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;


@Repository
public interface ResumenRepository extends JpaRepository<EResumen, Long> {

}


