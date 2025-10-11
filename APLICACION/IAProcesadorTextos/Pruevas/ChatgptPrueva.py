from transformers import AutoTokenizer, AutoModelForCausalLM
import torch
'''
# Ruta local donde clonaste el modelo
model_path = "./Mistral-7B-Instruct-v0.1"

# Cargar tokenizer y modelo desde la carpeta local
tokenizer = AutoTokenizer.from_pretrained(model_path)
model = AutoModelForCausalLM.from_pretrained(
    model_path,
    device_map="auto",          # Usa GPU si hay
    torch_dtype=torch.float16,  # Precisión mixta para mejorar rendimiento
)
'''
'''outputs = model.generate(
    **inputs,
    max_new_tokens=200,     # Máximo número de tokens generados
    do_sample=True,         # Activar muestreo (para aleatoriedad)
    temperature=0.7,        # Controla la creatividad (0 = determinista)
    top_p=0.9,              # Nucleus sampling (proba acumulada)
    top_k=50,               # Solo elige entre los 50 tokens más probables
    repetition_penalty=1.1, # Penaliza repeticiones
    num_return_sequences=1, # Cuántas respuestas generar
)'''

'''prompt = (
    "[INST] Usuario: Explica qué es una red neuronal.\n"
    "Asistente: Una red neuronal es un modelo computacional...\n"
    "Usuario: ¿Y para qué sirve?\n"
    "[/INST]"
)'''
'''
# Prompt con formato instruct
prompt = "[INST] Explica qué es un generador en Python con ejemplo. [/INST]"

# Tokenizar entrada
inputs = tokenizer(prompt, return_tensors="pt").to(model.device)

# Generar texto
outputs = model.generate(**inputs, max_new_tokens=200)
response = tokenizer.decode(outputs[0], skip_special_tokens=True)

print(response)
'''