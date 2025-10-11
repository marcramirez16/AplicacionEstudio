import ollama

class SepararTemasOllama():
    def separarTemas(self, pregunta):
        print("provando")
        prompt = "escribe un articulo sobre chat gpt"
        modelo = 'tinyllama'

        response = ollama.chat(model=modelo, messages=[{'role': 'user', 'content': prompt}])
        print(response['message']['content'])

        bot = chatbot_llama()
        bot.activar_ollama()
        pregunta = input("Escribe tu pregunta: ")
        respuesta = bot.preguntar(pregunta)
        print(respuesta)