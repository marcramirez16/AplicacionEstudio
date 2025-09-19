using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafica
{
    public partial class Calc : UserControl
    {
        String[] arrayseleccionado;
        String[] array;
        int movimientoletras = 0;
        public Calc()
        {
            InitializeComponent();
            movimientoletras = 0;
            completararray_letras();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Calc_Load(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (movimientoletras != 2)
            {
                movimientoletras += 1;
                arrayletrasseleccionadas();
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (movimientoletras != 0)
            {
                movimientoletras -= 1;
                arrayletrasseleccionadas();
            }
        }

        private void arrayletrasseleccionadas()
        {
            Button[] botones = new Button[] { button15, button16, button17, button18, button19, button20, button21, button22, button23, button24, button25, button26, button27, button28, button29, button30, button31, button32, button33, button34};

            if (movimientoletras == 0)
            {
                for (int i = 0; i < botones.Length && i < 20; i++) 
                {
                    botones[i].Text = array[i];
                }
            }
            if (movimientoletras == 1)
            {
                int startIndex = 20;
                int endIndex = Math.Min(startIndex + 20, array.Length - 1); // Asegura no pasarse

                for (int i = 0; i < (endIndex - startIndex); i++)
                {
                    botones[i].Text = array[startIndex + i + 1];
                }
            }
            if (movimientoletras == 2)
            {
                int startIndex = 40;
                int endIndex = Math.Min(startIndex + 20, array.Length - 1); // Asegura no pasarse

                for (int i = 0; i < (endIndex - startIndex); i++)
                {
                    botones[i].Text = array[startIndex + i + 1];
                }
            }
        }

        private void completararray_letras()
        {
            array = new String[61]; 
            array[0] = "A";
            array[1] = "B";
            array[2] = "Γ";
            array[3] = "Δ";
            array[4] = "Ε";
            array[5] = "Ζ";
            array[6] = "Η";
            array[7] = "Θ";
            array[8] = "I";
            array[9] = "K";
            array[10] = "Λ";
            array[11] = "M";
            array[12] = "N";
            array[13] = "Ξ";
            array[14] = "Ο";
            array[15] = "Π";
            array[16] = "Ρ";
            array[17] = "Σ";
            array[18] = "Τ";
            array[19] = "Υ";    //hasta aqui grupo1
            array[20] = "Φ";
            array[21] = "Χ";
            array[22] = "Ψ";
            array[23] = "Ω";
            array[24] = "α";
            array[25] = "β";
            array[26] = "γ";
            array[27] = "δ";
            array[28] = "ε";
            array[29] = "ζ";
            array[30] = "η";
            array[31] = "θ";
            array[32] = "ι";
            array[33] = "κ";
            array[34] = "λ";
            array[35] = "μ";
            array[36] = "ν";
            array[37] = "ξ";
            array[38] = "ο";
            array[39] = "π";   //hasta aqui grupo2
            array[40] = "ρ";
            array[41] = "ς";
            array[42] = "σ";
            array[43] = "τ";
            array[44] = "υ";
            array[45] = "φ";
            array[46] = "χ";
            array[47] = "ψ";
            array[48] = "ω";
            array[49] = "No";
            array[50] = "No";
            array[51] = "No";
            array[52] = "No";
            array[53] = "No";
            array[54] = "No";
            array[55] = "No";
            array[56] = "No";
            array[57] = "No";
            array[58] = "No";
            array[59] = "No";
            array[60] = "No";



        }
    } 
}
