using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GTE.WindowsForms
{
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

    public sealed class SeccionDelInicio
    {
        public SeccionDelInicio(string titulo, IEnumerable<AccesoDirecto> accesos)
        {
            Titulo = titulo;
            Accesos = accesos.ToList();
        }

        public string Titulo { get; }
        public IReadOnlyList<AccesoDirecto> Accesos { get; }
    }

    public class HomeForm : Form
    {
        private const int AnchoTarjeta = 300;
        private const int AltoTarjeta = 104;

        private readonly FlowLayoutPanel pnlTarjetas = new FlowLayoutPanel();
        private readonly List<Label> titulosDeSeccion = new();
        private readonly List<Panel> tarjetas = new();

        public HomeForm(string? nombre, string? rol, IEnumerable<SeccionDelInicio> secciones)
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

            foreach (var seccion in secciones.Where(seccion => seccion.Accesos.Count > 0))
            {
                var titulo = CrearTituloDeSeccion(seccion.Titulo);
                titulosDeSeccion.Add(titulo);
                pnlTarjetas.Controls.Add(titulo);

                foreach (var acceso in seccion.Accesos)
                {
                    var tarjeta = CrearTarjeta(acceso);
                    tarjetas.Add(tarjeta);
                    pnlTarjetas.Controls.Add(tarjeta);
                }
            }

            pnlTarjetas.Resize += (_, _) => AcomodarTarjetas();
            AcomodarTarjetas();

            Controls.Add(lblSaludo);
            Controls.Add(lblDetalle);
            Controls.Add(pnlTarjetas);
        }

        private void AcomodarTarjetas()
        {
            int disponible = pnlTarjetas.ClientSize.Width - 24;
            if (disponible <= 0)
                return;

            int columnas = disponible >= 960 ? 3 : disponible >= 620 ? 2 : 1;
            int ancho = Math.Max(240, (disponible - ((columnas - 1) * 16)) / columnas);

            foreach (var titulo in titulosDeSeccion)
                titulo.Width = disponible;

            foreach (var tarjeta in tarjetas)
            {
                tarjeta.Width = ancho;

                var titulo = tarjeta.Controls["lblTituloTarjeta"] as Label;
                var descripcion = tarjeta.Controls["lblDescripcion"] as Label;

                if (titulo == null || descripcion == null)
                    continue;

                descripcion.Width = ancho - 32;
                descripcion.Top = titulo.Bottom + 8;
                descripcion.Height = Math.Max(40, descripcion.PreferredHeight);
                tarjeta.Height = descripcion.Bottom + 14;
            }
        }

        private Label CrearTituloDeSeccion(string titulo)
        {
            return new Label
            {
                Text = titulo.ToUpperInvariant(),
                Name = "lblSeccion",
                AutoSize = false,
                Size = new Size(pnlTarjetas.ClientSize.Width - 6, 26),
                Margin = new Padding(0, 12, 0, 8),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Tema.TextoSuave,
                TextAlign = ContentAlignment.BottomLeft
            };
        }

        private Panel CrearTarjeta(AccesoDirecto seccion)
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
                Name = "lblTituloTarjeta",
                Location = new Point(16, 16),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Tema.Texto,
                Cursor = Cursors.Hand
            };

            var lblDescripcion = new Label
            {
                Text = seccion.Descripcion,
                Name = "lblDescripcion",
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

        private static bool EstaAdentro(Control tarjeta) =>
            tarjeta.ClientRectangle.Contains(tarjeta.PointToClient(Cursor.Position));
    }
}
