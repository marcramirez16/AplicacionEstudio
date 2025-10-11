import subprocess
from sys import activate_stack_trampoline

import ollama
import re
import json

class chatbot_llama:
    def activar_ollama(self):
        subprocess.Popen(["ollama", "start"], stdout=subprocess.PIPE, stderr=subprocess.PIPE)

    def preguntar(self, prompt):
        model = 'llama3'


        stream = ollama.chat(
            model=model,
            messages=[{'role': 'user', 'content': prompt}],
            stream=False  # Cambiado a False para obtener toda la respuesta a la vez
        )

        texto = stream['message']['content']
        return texto


    def dividir_por_temas(self, resumen):
        model = 'llama3'

        instrucciones = (
            "Eres un asistente para una aplicación de estudio. Tu tarea es analizar un resumen académico "
            "y separarlo en partes temáticas claras, sin modificar el contenido original. "
            "Identifica los diferentes temas presentes en el texto y agrupa las partes del resumen que pertenecen a cada uno. "
            "No resumas, no reformules, no inventes contenido. Solo divide el texto original por temas de forma coherente. "
            "Devuelve únicamente un array JSON de strings (sin texto adicional antes o después). "
            "Formato esperado: [\"Texto del tema 1...\", \"Texto del tema 2...\", \"Texto del tema 3...\"]"
        )

        messages = [
            {'role': 'system', 'content': instrucciones},
            {'role': 'user', 'content': resumen}
        ]

        respuesta = ollama.chat(
            model=model,
            messages=messages,
            stream=False
        )

        contenido = respuesta['message']['content']

        partes = []
        arrays = re.findall(r'\[\s*".*?"\s*(?:,\s*".*?")*\s*\]', contenido, re.DOTALL)

        if arrays:
            for arr in arrays:
                try:
                    elementos = json.loads(arr)
                    partes.extend(elementos)
                except json.JSONDecodeError:
                    continue

        # Si no se pudo extraer nada válido, devolver el contenido completo
        if not partes:
            partes = [contenido]

        return partes

