import base64
from PIL import Image
import io

def imagen_a_base64(ruta_archivo):
    # Abrir la imagen
    with Image.open(ruta_archivo) as img:
        # Guardar la imagen en memoria en formato PNG
        buffered = io.BytesIO()
        img.save(buffered, format="PNG")

        # Convertir a Base64
        img_base64 = base64.b64encode(buffered.getvalue()).decode("utf-8")
        return img_base64

# Uso
ruta = "ecu.png"
bitmap_string = imagen_a_base64(ruta)
print(bitmap_string)