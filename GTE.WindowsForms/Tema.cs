using System;
using System.Drawing;
using System.Windows.Forms;

namespace GTE.WindowsForms
{
    /// <summary>
    /// Colores y estilos compartidos por todas las pantallas, para que el
    /// escritorio se vea igual que la interfaz web.
    /// </summary>
    public static class Tema
    {
        public static readonly Color Fondo = Color.FromArgb(248, 249, 250);
        public static readonly Color Superficie = Color.White;
        public static readonly Color Texto = Color.FromArgb(33, 37, 41);
        public static readonly Color TextoSuave = Color.FromArgb(108, 117, 125);
        public static readonly Color Primario = Color.FromArgb(13, 110, 253);
        public static readonly Color Exito = Color.FromArgb(25, 135, 84);
        public static readonly Color Peligro = Color.FromArgb(220, 53, 69);
        public static readonly Color Gris = Color.FromArgb(108, 117, 125);
        public static readonly Color Barra = Color.FromArgb(33, 37, 41);

        /// <summary>Fondo y tipografía base de una ventana.</summary>
        public static void Ventana(Form formulario)
        {
            formulario.BackColor = Fondo;
            formulario.Font = new Font("Segoe UI", 9.5F);
        }

        public static void Titulo(Label etiqueta)
        {
            etiqueta.ForeColor = Texto;
            etiqueta.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
        }

        public static void Etiqueta(Label etiqueta)
        {
            etiqueta.ForeColor = TextoSuave;
            etiqueta.Font = new Font("Segoe UI", 9.5F);
        }

        public static void Entrada(TextBox campo)
        {
            campo.BackColor = Superficie;
            campo.ForeColor = Texto;
            campo.BorderStyle = BorderStyle.FixedSingle;
            campo.Font = new Font("Segoe UI", 10F);
        }

        public static void Entrada(ComboBox combo)
        {
            combo.BackColor = Superficie;
            combo.ForeColor = Texto;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = new Font("Segoe UI", 10F);

            if (combo.DropDownStyle != ComboBoxStyle.Simple)
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Deja la grilla con el mismo aspecto que las tablas de la web, con el
        /// texto cómodo para leer en pantallas grandes: las columnas se reparten
        /// todo el ancho disponible, así la tabla se agranda con la ventana en
        /// lugar de dejar un hueco a la derecha.
        /// </summary>
        public static void Grilla(DataGridView grilla)
        {
            const float TamanioDeLetra = 11F;

            grilla.BackgroundColor = Superficie;
            grilla.BorderStyle = BorderStyle.None;
            grilla.GridColor = Color.FromArgb(233, 236, 239);
            grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grilla.MultiSelect = false;
            grilla.RowHeadersVisible = false;
            grilla.EnableHeadersVisualStyles = false;
            grilla.AllowUserToResizeRows = false;

            grilla.ColumnHeadersDefaultCellStyle.BackColor = Barra;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Barra;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", TamanioDeLetra, FontStyle.Bold);
            grilla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            grilla.RowTemplate.Height = 34;
            grilla.DefaultCellStyle.Font = new Font("Segoe UI", TamanioDeLetra);
            grilla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(207, 226, 255);
            grilla.DefaultCellStyle.SelectionForeColor = Texto;
            grilla.AlternatingRowsDefaultCellStyle.BackColor = Fondo;

            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public static void Boton(Button boton, Color color)
        {
            boton.BackColor = color;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;

            if (boton.Height < 36)
                boton.Height = 36;
        }

        public static void BotonPrimario(Button boton) => Boton(boton, Primario);

        public static void BotonExito(Button boton) => Boton(boton, Exito);

        public static void BotonPeligro(Button boton) => Boton(boton, Peligro);

        public static void BotonSecundario(Button boton) => Boton(boton, Gris);

        /// <summary>Botón de la barra lateral, como el menú de la web.</summary>
        public static void BotonMenu(Button boton)
        {
            boton.BackColor = Barra;
            boton.ForeColor = Color.FromArgb(222, 226, 230);
            boton.FlatStyle = FlatStyle.Flat;
            // Un borde apenas más claro que la barra separa una opción de la otra.
            boton.FlatAppearance.BorderSize = 1;
            boton.FlatAppearance.BorderColor = Color.FromArgb(52, 58, 64);
            boton.TextAlign = ContentAlignment.MiddleLeft;
            boton.Padding = new Padding(15, 0, 0, 0);
            boton.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Título de un grupo del menú lateral. Va sobre la barra oscura, así que
        /// tiene que ser claro para que se lea.
        /// </summary>
        public static void EtiquetaSeccion(Label etiqueta)
        {
            // Fondo un poco más claro y letra celeste: así el título del grupo se
            // distingue de las opciones, que van en blanco grisáceo.
            etiqueta.BackColor = Color.FromArgb(52, 58, 64);
            etiqueta.ForeColor = Color.FromArgb(142, 202, 230);
            etiqueta.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            etiqueta.TextAlign = ContentAlignment.MiddleLeft;
            etiqueta.Padding = new Padding(15, 0, 0, 0);
        }
    }
}
