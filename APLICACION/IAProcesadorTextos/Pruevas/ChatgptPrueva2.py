from transformers import AutoTokenizer, AutoModelForCausalLM
from sentence_transformers import SentenceTransformer, util
import torch

def hacerconsulta():
    # Ruta local del modelo
    model_path = "../Mistral-7B-Instruct-v0.1"

    # Cargar tokenizer y modelo
    tokenizer = AutoTokenizer.from_pretrained(model_path)
    model = AutoModelForCausalLM.from_pretrained(
        model_path,
        device_map="auto",
        torch_dtype=torch.float16,
    )

    # Prompt con formato instruct
    prompt = "[INST] Explica qué es un generador en Python con ejemplo. [/INST]"

    # Tokenizar
    inputs = tokenizer(prompt, return_tensors="pt").to(model.device)

    # Generar 5 respuestas diferentes
    outputs = model.generate(
        **inputs,
        max_new_tokens=150,
        num_return_sequences=5,
        do_sample=True,
        temperature=0.7,
        top_p=0.9,
    )

    # Decodificar todas
    respuestas = [
        tokenizer.decode(output, skip_special_tokens=True).strip()
        for output in outputs
    ]

    # Mostrar todas
    for i, r in enumerate(respuestas):
        print(f"\n🔹 Respuesta {i+1}:\n{r}")

    # ---------- COMPARAR CON EMBEDDINGS ----------

    # Cargar modelo de embeddings (rápido y bueno)
    embedder = SentenceTransformer("all-MiniLM-L6-v2")

    # Embeddings de las respuestas
    embeddings = embedder.encode(respuestas, convert_to_tensor=True)

    # Matriz de similitud de coseno
    sim_matrix = util.pytorch_cos_sim(embeddings, embeddings)

    # Sumar similitudes de cada respuesta con las demás
    scores = sim_matrix.sum(dim=1)

    # Seleccionar la más representativa (más parecida al resto)
    indice_mejor = scores.argmax().item()
    respuesta_representativa = respuestas[indice_mejor]

    print("\n✅ Respuesta más representativa:\n", respuesta_representativa)


    #Mostrar respuestas no tan representativas
    print("\n📌 Otras respuestas diferentes:\n")
    for i, (respuesta, score) in enumerate(zip(respuestas, scores)):
        if i != indice_mejor:
            print(f"🔸 Respuesta {i+1} (similitud total: {score:.2f}):\n{respuesta}\n")


if __name__ == '__main__':
    hacerconsulta();