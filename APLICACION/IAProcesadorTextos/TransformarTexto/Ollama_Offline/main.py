from TransformarTexto.Ollama_Offline.SepararTemasOllama import SepararTemasOllama

from TransformarTexto.Ollama_Offline.Prompt_Ollama import chatbot_llama


if __name__ == '__main__':
    #tokenit = SepararTemasOllama()
    #tokenit.separarTemas("texto")


    resumen = """
        LEY DE OHM
        Establece la relación entre la tensión, la corriente y la resistencia
        
        V = R * I
        R = V/I
        
        -Voltaje(V) Voltaje= Fuerza o presion electrica que empuja los electrones
        -Corriente(I) Amperios= Flujo de electrones, cantidad de electricidad que pasa por un punto
        -Resistencia(R) Ωhmios= dificultad de la corriente para pasar por un material “siempre positiva”
        
        “Al calcular la ley, si V y I son los valores maximos, la resistencia tambien sera la maxima, si por el contrario son los valores actuales, la resistencia tambien sera la actual…”
        
        * los resistores dan resistencia, dificultando el paso de la corriente .
        Si estos, son no lineales se les llama resistores no lineales. A diferencia de estos, los resistores “lineales”, son los que su resistencia no cambia sin importar el voltaje o la corriente...
        
        “Los resistores no lineales no son errores, muchas veces son funcionales e intencionales”
        
        * La Conductancia (G) Siemens =  “opuesto a la resistencia”, que tan facil, la corriente pasa a traves de un material
        G = 1/R    O tambien:   G = I/V
        
        Factor de conversion: 1 SIEMENS = 1 A/V  ej. resistencia de 2 oms tiene una contundacia de ½ S
        
        * corto circuito/circuito abierto:
        
        El corto circuito se define como una resistencia de cero ohm, entonces el voltaje sera 0, ya que V=RI
        
        - La ley de Ohm presenta algunas limitaciones:
        
        1.Se puede aplicar a los materiales metales, pero no al carbo o materiales utilizados en transistores
        2.La resistencia cambia con la temperatura, todos los materiales se calientan por el paso de corriente
        3.Algunas aleaciones conducen mejor las cargas en una direccion que en otra
        
        -----------------------------------------------------------------------------
        ELECTRONICA ANAOLGICA
        Es Continua, “sube suave” 
        “señales electricas que cambian de manera suave y continua”
        
        V(t) = k * M(t)
        -M(t) = magnitud fisica (ejemplo. temperatura)
        -K = constante “que tan fuerte es la relacion”
        -V(t) = Voltaje que representa la magnitud
        
        Un transductor, realiza la formula anterior. Hay de diferentes tipos (luz “fotodiodo”, temperatura “termopadar”, distancia “sensor ultrasonico”)
        
        
        Un transductor, genera una señal electrica a partir de una fisica, y un amplificador aumenta esa señal electrica para que sea leido mejor por un sistema.
        
        Formula que realiza un amplificador a un transductor:
        Vsalida​=a⋅Ventrada​ 
        Si a = 10 y a señal era 0.1v ahora sera 1v
        
        Tambien se puede crear amplificadores operacionales: permiten sumar y restar señales, comparar, integrar, derivar
        
        ELECTRONICA DIGITAL
        
        Electronica digital: 1 o 0 “Computadoras, calculadoras…”
        Depende de la logica “TTL, CMOS 3.3...”, x voltaje = 0 y b voltaje = 1
        
        Como convertir una magnitud fisica a digital?
        1. Medir la magnitud fisica “con transductor y amplificadores como haciamos en la electronica analogica”
        2. Se usa un Conversor Analógico-Digital (A/D) Para convertir el voltaje analogico anterior en una secuencia de bits
        
        El interruptor o conmutador es el dispositivo basico de la electronica digital. 
        
        “Abierto, no pasa la corriente: 0 ”“cerrado, si pasa la corriente: 1”. Estos se usan para crear puertas AND          OR            NOT “inversor”  NAND     NOR, XOR, XNOR
        0 0 = 0        0 0 = 0      0 = 1                    0 0 0 = 1   …          …    ….
        0 1 = 0        0 1 = 1      1 = 0                    0 1 0 = 1
        1 0 = 0        1 0 = 1                                  1 0 0 = 1
        1 1 = 1        1 1 = 1                                 
        
        Tambien se pueden utilizar biestables para crear bloques secuenciales “recordar el ultimo bit”, mientras que los combinacionales no tienen memoria
        
        Porque utilizamos electronica digital si es mas complicado?
        
        1.Precision: Cada valor se representa claramente con una palabra binaria única, sin confusion entre valores cercanos
        2.Menos errores por desviaciones
        3.Capacidad de calculo. Los simbolos permiten operar facilmente con matrices binarias
        4.La representacion simbolica permite realizar operaciones logicas y tomar decisiones
            """

    bot = chatbot_llama()
    partes = bot.dividir_por_temas(resumen)

    print("\nTemas encontrados:")
    for i, parte in enumerate(partes, 1):
        print(f"\nTema {i}:\n{parte.strip()}")