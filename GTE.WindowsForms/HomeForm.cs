using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GTE.WindowsForms
{
    /// <summary>Sección que se puede abrir desde el inicio, con su descripción.</summary>
    public sealed class AccesoDirecto
    {
        public AccesoDirecto(string titulo, string descripcion, Action abrir)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Abrir = abrir;
        }

        public string Titulo { get; }
        public string Descripcion { get; }
        public Action Abrir { get; }
    }

    /// <summary>
    /// Pantalla de inicio del escritorio: saluda al usuario y le muestra una
    /// tarjeta por cada sección que tiene habilitada, igual que la web.
    /// </summary>
    public class HomeForm : Form
    {
        private const int AnchoTarjeta = 300;
        private const int AltoTarjeta = 104;

        private readonly FlowLayoutPanel pnlTarjetas = new FlowLayoutPanel();

        public HomeForm(string? nombre, string? rol, IEnumerable<AccesoDirecto> secciones)
        {
            Tema.Ventana(this);
            ClientSize = new Size(780, 530);

            var lblSaludo = new Label
            {
                Text = $"Hola, {nombre}",
                Location = new Point(24, 18),
                AutoSize = true
            };
            Tema.Titulo(lblSaludo);

            var lblDetalle = new Label
            {
                Text = $"Entraste como {rol}. Estas son las secciones que tenés disponibles.",
                Location = new Point(26, 62),
                AutoSize = true
            };
            Tema.Etiqueta(lblDetalle);

            pnlTarjetas.Location = new Point(24, 104);
            pnlTarjetas.Size = new Size(ClientSize.Width - 48, ClientSize.Height - 128);
            pnlTarjetas.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            pnlTarjetas.BackColor = Tema.Fondo;
            pnlTarjetas.WrapContents = true;
            pnlTarjetas.AutoScroll = true;

            foreach (AccesoDirecto seccion in secciones)
                pnlTarjetas.Controls.Add(CrearTarjeta(seccion));

            Controls.Add(lblSaludo);
            Controls.Add(lblDetalle);
            Controls.Add(pnlTarjetas);
        }

        /// <summary>
        /// Tarjeta con el nombre de la sección y una línea que explica para qué
        /// sirve. Se puede clickear en cualquier parte, como las de la web.
        /// </summary>
        private Control CrearTarjeta(AccesoDirecto seccion)
        {
            var tarjeta = new Panel
            {
                Size = new Size(AnchoTarjeta, AltoTarjeta),
                BackColor = Tema.Superficie,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 16, 16),
                Cursor = Cursors.Hand
            };

            var lblTitulo = new Label
            {
                Text = seccion.Titulo,
                Location = new Point(16, 16),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Tema.Texto,
                Cursor = Cursors.Hand
            };

            var lblDescripcion = new Label
            {
                Text = seccion.Descripcion,
                Location = new Point(16, 46),
                Size = new Size(AnchoTarjeta - 32, 46),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Tema.TextoSuave,
                Cursor = Cursors.Hand
            };

            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(lblDescripcion);

            foreach (Control parte in new Control[] { tarjeta, lblTitulo, lblDescripcion })
            {
                parte.Click += (_, _) => seccion.Abrir();
                parte.MouseEnter += (_, _) => Resaltar(tarjeta, lblTitulo, encendido: true);
                parte.MouseLeave += (_, _) => Resaltar(tarjeta, lblTitulo, encendido: EstaAdentro(tarjeta));
            }

            return tarjeta;
        }

        private static void Resaltar(Panel tarjeta, Label lblTitulo, bool encendido)
        {
            tarjeta.BackColor = encendido ? Color.FromArgb(240, 245, 255) : Tema.Superficie;
            lblTitulo.ForeColor = encendido ? Tema.Primario : Tema.Texto;
        }

        /// <summary>
        /// Al salir de la tarjeta o de una de sus etiquetas hay que fijarse si el
        /// mouse sigue adentro, si no el resaltado parpadea al pasar por encima
        /// del título o de la descripción.
        /// </summary>
        private static bool EstaAdentro(Control tarjeta) =>
            tarjeta.ClientRectangle.Contains(tarjeta.PointToClient(Cursor.Position));
    }
}
