import tkinter as tk
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
from matplotlib.figure import Figure
import requests
import base64
import io

root = tk.Tk()
root.title("Teclado de Ecuaciones LaTeX Avanzado")
equation = tk.StringVar()

def render():
    eq_text = equation.get().strip()
    fig.clear()
    if eq_text:
        fig.text(0.05, 0.5, f"${eq_text}$", fontsize=20)
    canvas.draw()

def insert(symbol, cursor_shift=0):
    pos = entry.index(tk.INSERT)
    text = equation.get()
    equation.set(text[:pos] + symbol + text[pos:])
    entry.icursor(pos + (cursor_shift if cursor_shift>0 else len(symbol)))
    render()

def delete_last():
    pos = entry.index(tk.INSERT)
    text = equation.get()
    if pos==0: return
    equation.set(text[:pos-1] + text[pos:])
    entry.icursor(pos-1)
    render()

def guardar():
    text = equation.get().strip()
    if not text: return
    # Crear imagen
    temp_fig = Figure(figsize=(6,2))
    temp_fig.text(0.05,0.5,f"${text}$", fontsize=20)
    img_bytes = io.BytesIO()
    temp_fig.savefig(img_bytes, dpi=300, bbox_inches='tight', format='png')
    img_bytes.seek(0)
    img_base64 = base64.b64encode(img_bytes.read()).decode('utf-8')
    img_bytes.close()

    # POST a Spring Boot
    url = "http://localhost:8080/api/SubirImagenBase64"
    payload = {"nombre":"ecuacion.png","imagen":img_base64}
    try:
        resp = requests.post(url, json=payload)
        if resp.status_code == 200:
            print("Imagen enviada, base64 recibido:", resp.text[:50],"...")
        else:
            print("Error al enviar:", resp.status_code, resp.text)
    except Exception as e:
        print("Error conexión:", e)
    root.destroy()

# Entry
entry = tk.Entry(root, textvariable=equation, font=("Arial",16), width=40)
entry.pack(pady=10)

# Botones básicos
button_frame = tk.Frame(root)
button_frame.pack()
buttons = [
    ("7","7"),("8","8"),("9","9"),("+","+"),("-","-"),
    ("4","4"),("5","5"),("6","6"),("*","\\cdot "),("/","/"),
    ("1","1"),("2","2"),("3","3"),("0","0"),(".","."),
    ("(", "("),(")", ")"),("π","\\pi "),("∞","\\infty "),
    ("÷","\\div ")
]
row=0; col=0
for t,s in buttons:
    b = tk.Button(button_frame,text=t,width=5,height=2,command=lambda x=s: insert(x))
    b.grid(row=row,column=col,padx=2,pady=2)
    col+=1
    if col>4: col=0; row+=1

# Botones especiales
special_buttons = [
    ("Fracción","\\frac{}{}",6),
    ("Raíz","\\sqrt{}",6),
    ("Potencia","^{}",2),
    ("Subíndice","_{}",2),
    ("Σ","\\sum_{}^{}",6),
    ("∏","\\prod_{}^{}",6),
    ("∫","\\int_{}^{}",6),
    ("∂","\\partial ",0),
    ("← Borrar","del",0),
    ("Guardar","guardar",0)
]
special_frame = tk.Frame(root)
special_frame.pack(pady=5)
for t,s,c in special_buttons:
    if s=="del":
        b = tk.Button(special_frame,text=t,width=10,height=2,command=delete_last)
    elif s=="guardar":
        b = tk.Button(special_frame,text=t,width=10,height=2,command=guardar)
    else:
        b = tk.Button(special_frame,text=t,width=10,height=2,command=lambda x=s,y=c: insert(x,y))
    b.pack(side=tk.LEFT,padx=2,pady=2)

# Letras griegas
greek_letters = [
    ("α","\\alpha "), ("β","\\beta "), ("γ","\\gamma "), ("δ","\\delta "),
    ("ε","\\epsilon "), ("ζ","\\zeta "), ("η","\\eta "), ("θ","\\theta "),
    ("λ","\\lambda "), ("μ","\\mu "), ("ν","\\nu "), ("ξ","\\xi "),
    ("ρ","\\rho "), ("σ","\\sigma "), ("τ","\\tau "), ("φ","\\phi "),
    ("ψ","\\psi "), ("ω","\\omega ")
]
greek_frame = tk.Frame(root)
greek_frame.pack(pady=5)
for t,s in greek_letters:
    b = tk.Button(greek_frame, text=t, width=5, height=2, command=lambda x=s: insert(x))
    b.pack(side=tk.LEFT, padx=1, pady=1)

# Otros símbolos
other_symbols = [
    ("≈","\\approx "), ("≠","\\neq "), ("<","<"), (">",">")
]
symbol_frame = tk.Frame(root)
symbol_frame.pack(pady=5)
for t,s in other_symbols:
    b = tk.Button(symbol_frame, text=t, width=5, height=2, command=lambda x=s: insert(x))
    b.pack(side=tk.LEFT, padx=1, pady=1)

# Renderizador
fig = Figure(figsize=(6,2))
canvas = FigureCanvasTkAgg(fig, master=root)
canvas.get_tk_widget().pack()
render()
root.mainloop()
