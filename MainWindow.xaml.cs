
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestorProductosWPF_Ap1_Ap2_P4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GestorProductos gestor = new GestorProductos();
        public MainWindow()
        {
            InitializeComponent();
            CargarDatosIniciales();
            dataGridProductos.ItemsSource = gestor.ObtenerListaProductos();
            comboTipoBusqueda.Items.Add("ID");
            comboTipoBusqueda.Items.Add("Nombre");
            comboTipoBusqueda.Items.Add("Codigo de Barras");
            comboTipoBusqueda.SelectedIndex = 0; 

            comboTipoOrden.Items.Add("Nombre");
            comboTipoOrden.Items.Add("Precio");
            comboTipoOrden.SelectedIndex = 0;
        }

        public void CargarDatosIniciales()
        {
            gestor.AgragarProducto(new Producto
            {
                Id = 3,
                CodigoBarras = "12345",
                Nombre = "Maruchan",
                Categorias = "Comida",
                Precio = 20,
                Stock = 20 
            }
                );
            gestor.AgragarProducto(new Producto
            {
                Id = 1,
                CodigoBarras = "45678",
                Nombre = "Mollaquiles",
                Categorias = "Comida",
                Precio = 43,
                Stock = 20
            }
                );
            gestor.AgragarProducto(new Producto
            {
                Id = 2,
                CodigoBarras = "54789",
                Nombre = "Shampoo",
                Categorias = "Higiene",
                Precio = 30,
                Stock = 20
            }
                );

            gestor.AgragarProducto(new Producto
            {
                Id = 4,
                CodigoBarras = "24681",
                Nombre = "Guantes",
                Categorias = "Ropa",
                Precio = 30,
                Stock = 50
            }
            );

        }

        private void btnAgregar_Click(object sender,RoutedEventArgs e)
        {
            var ventanaAgregar = new AgregarProducto();
            if (ventanaAgregar.ShowDialog() == true)
            {
                try
                {
                    Producto nuevoProducto = ventanaAgregar.Producto;
                    gestor.AgragarProducto(nuevoProducto);
                    dataGridProductos.ItemsSource = null;
                    dataGridProductos.ItemsSource = gestor.ObtenerListaProductos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void btnEliminar_Click(object sender,RoutedEventArgs e)
        {
            if (dataGridProductos.SelectedItem is Producto productoSeleccionado)
            {
                bool eliminado = gestor.EliminarProducto(productoSeleccionado.CodigoBarras);
                if (eliminado)
                {
                    dataGridProductos.ItemsSource = null;
                    dataGridProductos.ItemsSource = gestor.ObtenerListaProductos();
                    MessageBox.Show("Producto eliminado","Exito",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un producto","Advertencia",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void btnBuscar_Click(object sender,RoutedEventArgs e)
        {
            string criterio = comboTipoBusqueda.SelectedItem.ToString();
            string valor = txtBusqueda.Text.Trim();

            switch (criterio)   
            {
                case "ID":
                    if (int.TryParse(valor,out int id))
                    {
                        var (productoB, iteracionesB) = Buscador.BusquedaBinaria(gestor.ObtenerListaProductos(),id);
                        MostrarResultadoBusqueda(productoB,iteracionesB);
                    }
                    break;
                case "Nombre":
                    var (productoS, iteracionesS) = Buscador.BusquedaSecuencial(gestor.ObtenerListaProductos(),valor);
                    MostrarResultadoBusqueda(productoS,iteracionesS);
                    break;
                case "Codigo Barras":
                    Producto productoC = gestor.BuscarPorCodigo(valor);
                    MostrarResultadoBusqueda(productoC,1);
                    break;
            }

        }

        public void MostrarResultadoBusqueda(Producto producto, int iteraciones)
        {
            txtResultadoBusquda.Text = producto?.ToString() ?? "No encontrado";
            progresoIteraciones.Value = iteraciones;
        }

        private void btnOrdenar_Click(object sender,RoutedEventArgs e)
        {
            List<Producto> productos = new List<Producto>(gestor.ObtenerListaProductos());
            string criterio = comboTipoOrden.SelectedItem.ToString();

            switch (criterio)
            {
                case "ID":
                    Ordenador.QuickSortId(productos);
                    break;
                case "Nombre":
                    productos = Ordenador.MergeSortPorNombre(productos);
                    break;
                case "Precio":

                    Ordenador.QuickSortPorPrecio(productos);
                    break;
            }

            ListViewOrdenados.ItemsSource = productos;
            DibujarBarras(productos);

        }

        private void DibujarBarras (List<Producto> productos )
        {
           CanvasGrafico.Children.Clear();
            double maxPrecio = (double)productos.Max(p => p.Precio);
            double escala = CanvasGrafico.ActualHeight / maxPrecio; 
            
            for (int i = 0; i < productos.Count; i++)
            {
                Rectangle barra = new Rectangle
                {
                    Width = 30,
                    Height = (double)productos[i].Precio * escala,
                    Fill = Brushes.Aquamarine,
                    Margin = new Thickness(i * 40, CanvasGrafico.ActualHeight - ((double)productos[i].Precio* escala), 0,0)

                };

                CanvasGrafico.Children.Add(barra);
                TextBlock etiqueta = new TextBlock
                {
                    Text = productos[i].Nombre,
                    Margin = new Thickness(i * 40,CanvasGrafico.ActualHeight - 20,0,0)
                };

                CanvasGrafico.Children.Add (etiqueta);

            }
        }

 
    }
}