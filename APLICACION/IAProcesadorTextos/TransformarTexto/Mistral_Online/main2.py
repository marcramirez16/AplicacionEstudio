import os
from odf.opendocument import load
from odf.text import P
from odf.element import Node

from TransformarTexto.Mistral_Online.TokenitzarArchivoCHATGPT import TokenitzarArchivoCHATGPT

if __name__ == '__main__':

    ruta = r"C:\Users\marcr\Desktop\Escritorio\RESUMENES\Sin título 1.odt"

    if not os.path.exists(ruta):
        print("❌ El archivo no existe")
    else:
        print("✅ El archivo existe")

        # Cargar el documento .odt
        odt_doc = load(ruta)

        # Extraer todos los párrafos
        all_paragraphs = odt_doc.getElementsByType(P)


        def extraer_texto(elemento):
            """Concatena el texto de todos los hijos de un elemento ODF."""
            texto = ""
            for nodo in elemento.childNodes:
                if isinstance(nodo, Node) and nodo.nodeType == 3:  # 3 = TEXT_NODE
                    texto += nodo.data
            return texto.strip()


        # Construir el texto
        texto = "\n".join([extraer_texto(p) for p in all_paragraphs if extraer_texto(p)])

        print("\n📄 Contenido del archivo ODT:\n")
        print(texto)

        tokenit = TokenitzarArchivoCHATGPT()
        print("----------------------------------------")
        print("----------------------------------------")
        print("----------------------------------------")
        print("lista con temas separados:")
        print("----------------------------------------")
        print("----------------------------------------")
        print("----------------------------------------")
        print("")
        print("")
        print("")
        print("")
        tokenit.separarTemas(texto)