using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace FransitoExplorer
{
    public partial class Form1 : Form
    {
        private String[] historial; //intenté guardar ambos en favoritos mediante ficheros, pero no se por qué no me guardaba el fichero, inlcuso creándolo, yo manualmente en el raíz del proyecto. 
        private int contador = 0;
        private String busqueda = "http://www.google.es";
        private String inicio = "http://www.google.es";
        public Form1()
        {
            InitializeComponent();
            historial = new String[10];
        }

        //Método que permite que google sea cargado nada más abrir el navegador. 
        private void Form1_Load(object sender, EventArgs e)
        {
            WebBrowser navegador = new WebBrowser();
            tabControl1.TabPages[0].Controls.Add(navegador);
            navegador.Dock = DockStyle.Fill;
            url.Text = busqueda;
            navegador.Navigate(busqueda);
            navegador.LocationChanged += navegador_LocationChanged;
            navegador.DocumentTitleChanged += navegador_DocumentTitleChanged;
        }

        //Método que carga la página que pongas en la barra y pulsas intro.
        private void url_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                if (String.IsNullOrEmpty(url.Text)) return;
                if (url.Text.Equals("about:blank")) return;
                if (!url.Text.StartsWith("http://") &&
                    !url.Text.StartsWith("https://"))
                {
                    url.Text = "http://" + url.Text;
                }
                try
                {
                    (tabControl1.SelectedTab.Controls[0] as WebBrowser).Navigate(new Uri(url.Text));
                    if (contador < 10)
                    {
                        historial[contador] = url.Text;
                        contador++;

                    }
                    else
                    {
                        contador = 0;
                    }
                }
                catch (System.UriFormatException)
                {
                    return;
                }
            }
        }

        //Método que permite volver a la página anterior
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).GoBack();
        }

        //Método que permite volver a la página posterior
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).GoForward();
        }

        //Método que permite actualizar la página
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).Refresh();
        }

        //Método para volver a la página home
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).Navigate(new Uri(inicio));
        }

        //Método que añade la página actual a favoritos.
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            /*StreamWriter file;
            file = File.AppendText("favoritos.txt");
            file.WriteLine(url.Text);
            file.Close(); No me funciona*/
            combo_fav.Items.Add(url.Text);
        }

        //Método que abre una nueva pestaña
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            WebBrowser navegador = new WebBrowser(); 
            TabPage NuevaP = new TabPage("Nueva Pestaña"); // Creamos una nueva pestaña
            tabControl1.TabPages.Add(NuevaP); //cargamos la pestaña en el control
            tabControl1.SelectedTab = NuevaP; 
            tabControl1.TabPages[tabControl1.TabCount-1].Controls.Add(navegador);
            navegador.Dock = DockStyle.Fill;
            navegador.LocationChanged += navegador_LocationChanged;
            navegador.DocumentTitleChanged += navegador_DocumentTitleChanged;
            url.Text = "";
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).Navigate(busqueda);
            url.Text = busqueda;
        }

        private void navegador_LocationChanged(object sender, EventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            TabPage tab = browser.Parent as TabPage;
            tab.ToolTipText = browser.Url.ToString();
            if (tabControl1.SelectedTab.Equals(tab))
                url.Text = browser.Url.ToString();
        }

        private void navegador_DocumentTitleChanged(object sender, EventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            TabPage tab = browser.Parent as TabPage;
            tab.Text = browser.DocumentTitle;
            tab.ToolTipText = browser.Url.ToString();
        }

        //Método que elimina pestañas
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count != 0)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                url.Text = "";
            }
 
        }

        //Método que permite guardar como
        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).ShowSaveAsDialog();
        }

        //Método que permite realizar la vista previa
        private void vistaPreviaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).ShowPrintPreviewDialog();
        }

        //Método que permite realizar la impresión
        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).ShowPrintDialog();
        }

        //Método de configuración de la página
        private void configurarPáginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).ShowPageSetupDialog();
        }

        //Salir
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gracias por usar Fransito Explorer, Adiós!!!");
            this.Close();
        }

        //Método que muestra y oculta los favoritos, los cuales solo se añaden al combo box, no se guardan en arrays ni nada, ya que al no poder darle persistencia me pareció innecesario.
        private void favoritosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.combo_fav.Visible==false && this.label_fav.Visible==false)
            {
                this.combo_fav.Visible = true;
                this.label_fav.Visible = true;
            }
            else
            {
                this.combo_fav.Visible = false;
                this.label_fav.Visible = false;
            }
        }

        //Navegar por los favoritos.
        private void combo_fav_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                (tabControl1.SelectedTab.Controls[0] as WebBrowser).Navigate(new Uri(combo_fav.SelectedItem.ToString()));
            }
        }

        private void combo_his_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                (tabControl1.SelectedTab.Controls[0] as WebBrowser).Navigate(new Uri(combo_his.SelectedItem.ToString()));
            }

        }


        //Método que muestra y oculta el historial.
        private void historialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.combo_his.Visible == false && this.label_his.Visible == false)
            {
                this.combo_his.Visible = true;
                this.label_his.Visible = true;

                for (int i = 0;i < contador;i++)
                {
                    combo_his.Items.Add(historial[i]);
                }
            }
            else
            {
                this.combo_his.Visible = false;
                this.label_his.Visible = false;
            }
        }

        private void páginaDeInicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.inicio = Microsoft.VisualBasic.Interaction.InputBox("Defina su página de inicio: ", "Búsqueda");
        }

        private void búsquedaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           this.busqueda = Microsoft.VisualBasic.Interaction.InputBox("Defina su página de búsqueda: ", "Búsqueda");
        }

        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (tabControl1.SelectedTab.Controls[0] as WebBrowser).ShowPropertiesDialog();
        }

    }
}
