
public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string CodigoBarras { get; set; }
    public string Categorias { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }

    public override string ToString()
    {
        return $"{Id} | {CodigoBarras} - {Nombre} | {Precio} | {Stock}";
    }
}

public class GestorProductos
{
    public List<Producto> listaProductos = new List<Producto>();

    public Dictionary<string,Producto> diccionarioCodigo = new Dictionary<string,Producto>();

    //Operaciones lista
    public List<Producto> ObtenerListaProductos()
    {
        return new List<Producto>(listaProductos);
    }

    public void AgragarProducto(Producto p)
    {
        //Validar codigo de barras unico 
        if (diccionarioCodigo.ContainsKey(p.CodigoBarras))
        {
            throw new Exception("El codigo de barras ya existe");
        }
        listaProductos.Add(p);
        diccionarioCodigo[p.CodigoBarras] = p;
    }

    public bool EliminarProducto(string codigoBarras)
    {
        if (diccionarioCodigo.TryGetValue(codigoBarras,out var producto))
        {
            listaProductos.Remove(producto);
            diccionarioCodigo.Remove(codigoBarras);
            return true;
        }
        return false;
    }

    public void MostrarInventario()
    {
        Console.WriteLine("Inventario completo (orden de ingreso): ");
        foreach (var p in listaProductos)
        {
            Console.Write(p.ToString());
        }
    }

    //Operacion con diccionario 
    public Producto BuscarPorCodigo(string codigoBarras)
    {
        return diccionarioCodigo.TryGetValue(codigoBarras,out var producto) ? producto : null;
    }

    public bool ExisteProducto(string codigoBarras)
    {
        return diccionarioCodigo.ContainsKey(codigoBarras); 
    }

    public void MostrarProductosPorCategoria(string categoria)
    {
        Console.WriteLine($"Productos en categoria {categoria}:");
        foreach (var p in diccionarioCodigo)
        {
            if(p.Value.Categorias.Equals(categoria, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(p.Value.ToString());
            }
        }
    }
}

//Actividad 2 

    static public class Ordenador
    {
    static public void QuickSortId(List<Producto> productos) //La lista si lleva un orden, el diccionario no 
    {
        //Quicksort

        //Condicion de termino 
        if(productos.Count <= 1) { return; }

        //1-Seleccionar un elemento como pivote 
        Producto pivote = productos[productos.Count - 1];
        var menores = new List<Producto>();
        var mayores = new List<Producto>();

        //2- Organizar lista para que los menores al pivote esten a la izquierda y los mayores a la derecha 

        for (int i = 0; i < productos.Count-1; i++)
        {
            if (productos[i].Id < pivote.Id)
            {
                menores.Add(productos[i]);
            }
            else
            {
                mayores.Add(productos[i]);
            }
        }
        //3- Recursivamente aplica el algoritmo en los subarreglos formados 
        QuickSortId(menores);
        QuickSortId(mayores);

        //4- Combina los subarreglos para obtener la coleccion ordenada 
        productos.Clear();
        productos.AddRange(menores);
        productos.Add(pivote);
        productos.AddRange(mayores); 
    }

    //------------------- POR PRECIO --------------------------------------------------------------------------
    static public void QuickSortPorPrecio(List<Producto> productos) //La lista si lleva un orden, el diccionario no 
    {
        //Quicksort

        //Condicion de termino 
        if (productos.Count <= 1) { return; }

        //1-Seleccionar un elemento como pivote 
        Producto pivote = productos[productos.Count - 1];
        var menores = new List<Producto>();
        var mayores = new List<Producto>();

        //2- Organizar lista para que los menores al pivote esten a la izquierda y los mayores a la derecha 

        for (int i = 0; i < productos.Count - 1; i++)
        {
            if (productos[i].Precio < pivote.Precio)
            {
                menores.Add(productos[i]);
            }
            else
            {
                mayores.Add(productos[i]);
            }
        }
        //3- Recursivamente aplica el algoritmo en los subarreglos formados 
        QuickSortPorPrecio(menores);
        QuickSortPorPrecio(mayores);

        //4- Combina los subarreglos para obtener la coleccion ordenada 
        productos.Clear();
        productos.AddRange(menores);
        productos.Add(pivote);
        productos.AddRange(mayores);
    }

    //Merge Sort
    static public List<Producto> MergeSortPorNombre (List<Producto> productos)
    {
        //Condicon de termino 
        if (productos.Count <= 1) { return productos; }

        //1-Dividir la lista en dos mitades 
        int mitad = productos.Count / 2;
        var izquierda = productos.GetRange(0,mitad) ;
        var derecha = productos.GetRange(mitad, productos.Count - mitad);

        //2- Ordenar cada mitad de forma recursiva 
        izquierda = MergeSortPorNombre(izquierda) ;
        derecha = MergeSortPorNombre (derecha) ;

        //3-Mezclar las dos mitades ordenadas 
        return Mezclar(izquierda, derecha); 
    }

    //Funcion auxiliar para la mezcla 
    static public List<Producto> Mezclar (List<Producto> izquierda, List<Producto> derecha)
    {
        var resultado = new List<Producto>();
        int i = 0, j = 0;

        //4- Comparar y ordenar en orden 
        while (i < izquierda.Count && j < derecha.Count)
        {
            if (string.Compare(izquierda[i].Nombre, derecha[j].Nombre) < 0)
            {
                resultado.Add(izquierda[i++]); 
            }
            else
            {
                resultado.Add(derecha[j++]);
            }
        }

        while ( i < izquierda.Count)
        {
            resultado.Add(izquierda[i++]);
        }
        while (j < derecha.Count)
        {
            resultado.Add(derecha[j++]);
        }

        return resultado;
    }
}

//Actividad 3 
// * Para cualquier algoritmo de busqueda se parte de una coleccion ya ordenada 
public static class Buscador
{
    //Busqueda binaria 
    public static (Producto, int) BusquedaBinaria (List<Producto> productos, int idBuscado)
    {
        int inicio = 0;
        int fin = productos.Count - 1;
        int iteraciones = 0; 

        //1-Divide la coleccion por la mitad 
        while(inicio <=  fin)
        {
            iteraciones++;

            //2 - Calcular el punto medio 
            int medio = (inicio + fin) / 2;

            //3 - Comprobar si encontramos el valor que buscamos 
            if (productos[medio].Id == idBuscado)
            {
                return (productos[medio], iteraciones);
            }
            //4 - Ajustar los limites de buscqueda 
            if (productos[medio].Id < idBuscado)
            {
                inicio = medio + 1; //Buscar en la mitad derecha 
            }
            else
            {
                fin = medio - 1; //Buscar en la mitad izquierda 
            }
        }
        return (null, iteraciones); //No encontrado 
    }

    //Busqueda secuencial por nombre 
    public static (Producto, int) BusquedaSecuencial (List<Producto> productos, string nombreBuscado)
    {
        int iteraciones = 0; 

        //1 - Recorrer la coleccion elemento por elemento
        foreach(var producto in productos)
        {
            iteraciones++; 
            //2 - Comparar el criterio de busqueda 
            if(producto.Nombre.Equals(nombreBuscado, StringComparison.OrdinalIgnoreCase))
            {
                return(producto, iteraciones);
            }
        }
        return (null, iteraciones); //No encontrado 
    }
}