from transformers import AutoTokenizer, AutoModelForCausalLM
import torch
'''
model_path = "./Mistral-7B-Instruct-v0.1"

tokenizer = AutoTokenizer.from_pretrained(model_path)
model = AutoModelForCausalLM.from_pretrained(
    model_path,
    device_map="auto",
    torch_dtype=torch.float16,
)

# Texto largo a analizar
texto_largo = """
La inteligencia artificial se divide en varios campos como el aprendizaje automático, 
la visión por computador y el procesamiento del lenguaje natural. 
El aprendizaje automático permite a las máquinas aprender de datos. 
La visión por computador analiza imágenes y vídeos. 
El procesamiento del lenguaje natural permite comprender y generar lenguaje humano.
"""

# Prompt instructivo
prompt = (
    f"[INST] Analiza el siguiente texto y divídelo en diferentes temas teóricos. "
    f"Asigna un título corto y claro a cada tema. Devuélvelo en formato claro:\n\n{texto_largo}\n\n[/INST]"
)

# Tokenizar y generar
inputs = tokenizer(prompt, return_tensors="pt").to(model.device)
outputs = model.generate(**inputs, max_new_tokens=300)
response = tokenizer.decode(outputs[0], skip_special_tokens=True)

print("\n🧠 Temas teóricos detectados:\n")
print(response)
'''