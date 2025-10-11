import requests
from transformers import AutoTokenizer, AutoModelForCausalLM
import torch
import os

'''Si el resumen es muy largo, separar el resumen en diferentes partes.
primera parte: Obtener los temas, que retorne la posicion de la letra donde termina el penultimo tema
segunda parte: A partir de donde termino el penultimo tema... Recordarle el titulo de los temas anteriores
'''
class TokenitzarArchivoCHATGPT():

    def separarTemas(self, texto):
        # Ruta local al modelo
        model_path = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))),
                                 "Mistral-7B-Instruct-v0.1")




        # Cargar tokenizer y modelo local
        tokenizer = AutoTokenizer.from_pretrained(model_path)
        model = AutoModelForCausalLM.from_pretrained(
            model_path,
            device_map="auto",
            torch_dtype=torch.float16,
            local_files_only=True
        )

        # Texto de entrada
        '''
        texto = """
        La inteligencia artificial se divide en varios campos como el aprendizaje automático, 
        la visión por computador y el procesamiento del lenguaje natural. 
        El aprendizaje automático permite a las máquinas aprender de datos. 
        La visión por computador analiza imágenes y vídeos. 
        El procesamiento del lenguaje natural permite comprender y generar lenguaje humano.
        """
        '''

        # Prompt para pedir array de strings
        prompt = (
            f"[INST] Analiza el siguiente texto y sepáralo por temas teóricos. "
            f"Devuélvelo como una lista de diccionarios en formato Python. Cada diccionario debe tener las claves "
            f"'titulo' y 'contenido'. "
            f"No modifiques ni resumas el contenido, simplemente sepáralo y asigna un título claro a cada tema.\n\n"
            f"Texto:\n{texto}\n\n[/INST]"
        )

        # Tokenizar y generar
        inputs = tokenizer(prompt, return_tensors="pt").to(model.device)
        outputs = model.generate(**inputs, max_new_tokens=300)
        response = tokenizer.decode(outputs[0], skip_special_tokens=True)


        print("\n Array de temas teóricos detectados:\n")
        print(response)
