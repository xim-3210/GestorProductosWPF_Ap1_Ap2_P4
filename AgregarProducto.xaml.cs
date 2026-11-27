using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestorProductosWPF_Ap1_Ap2_P4
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    public partial class AgregarProducto : Window
    {
        public Producto Producto { get; set; }

        public AgregarProducto()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender,RoutedEventArgs e)
        {
            Producto = new Producto()
            {
                Id = int.Parse(textId.Text),
                CodigoBarras = textCodigoBarras.Text,
                Nombre = textNombre.Text,
                Categorias = textCategoria.Text,
                Precio = decimal.Parse(textPrecio.Text),
                Stock = int.Parse(textStock.Text),
            };

            this.DialogResult = true; 
            this.Close ();
        }
    }
}
