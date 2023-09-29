namespace WebApi;
public class Cadeteria
{
    private string nombre;
    private string telefono;
    private List<Cadete> listadeCadetes;
    private List<Pedido> listadePedidos;
    private static Cadeteria instance;
    private Cadeteria(){};
    private AccesoDatosCadeteria acceso;
    public static Cadeteria Instance
    {
        get
        {
            // Crear la instancia Cadeteria si aún no existe.
            if (instance == null)
            {
                instance = new Cadeteria();
                var instance = new AccesoDatosCadeteria().ObtenerInfoCadeteria;
                instance = acceso.ObtenerInfoCadeteria;
                instance.cadetes = acceso.ObtenerListaCadetes;
            }
            return instance;
        }
    }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Telefono { get => telefono; set => telefono = value; }
    public List<Cadete> ListadeCadetes { get => listadeCadetes; }
    public List<Pedido> ListadePedidos { get => listadePedidos; set => listadePedidos = value; }

    public Cadeteria(string nombre, string telefono)
    {
        this.Nombre = nombre;
        this.Telefono = telefono;
        this.ListadePedidos = new List<Pedido>();
    }

    private int CantCadetes()
    {
        return ListadeCadetes.Count;
    }

    public void AgregarListaCadetes(List<Cadete> listadeCadetes)
    {
        this.listadeCadetes = listadeCadetes;
    }

    public bool AgregarPedido(Pedido ped)
    {
        bool agregado = false;
        if (ped != null)
        {
            ListadePedidos.Add(ped);
            agregado = true;
        }
        return agregado;

    }
    public Pedido NuevoPedido(string obsPedido, string nombreCliente, string DireccionCliente, string telefonoCl, string datosRefCl)
    {
        var cliente = new Cliente(nombreCliente,DireccionCliente,telefonoCl,datosRefCl);
        var pedido = new Pedido(ListadePedidos.Count(),obsPedido, cliente);
        listadePedidos.Add(pedido);
        return pedido;
    }

    
    public int CantPedidosCadete(int idCadete, EstadoPedido estado)
    {
        int cant = 0;
        foreach (var p in ListadePedidos)
        {
            if ((p.ExisteCadete()) && (p.IdCadete() == idCadete) && (p.Estado == estado))
            {
                cant++;
            }
        }
        return cant;
    }


    public double JornalACobrar(int idCadete)
    {
        double Jornal = 500 * CantPedidosCadete(idCadete, EstadoPedido.Entregado);
        return Jornal;
    }




    public Informe CrearInforme()
    {
        List<int> idCadetes = listadeCadetes.Select(cad => cad.Id).ToList();
        List<string> NombreDeCadetes = listadeCadetes.Select(cad => cad.Nombre).ToList();
        List<int> cantPedidosEntregadosporCadetes = new List<int>();
        List<double> montosCadetes = new List<double>();
        foreach (var cad in ListadeCadetes)
        {
            cantPedidosEntregadosporCadetes.Add(CantPedidosCadete(cad.Id,EstadoPedido.Entregado));
            montosCadetes.Add(JornalACobrar(cad.Id));
        }
        int totalPedidosEntregados = cantPedidosEntregadosporCadetes.Sum();
        int cantPromedioDePedidosEntregados = (int)cantPedidosEntregadosporCadetes.Average();

        Informe informe = new Informe(CantCadetes(), idCadetes, NombreDeCadetes, cantPedidosEntregadosporCadetes, montosCadetes, totalPedidosEntregados, cantPromedioDePedidosEntregados);
        return informe;

    }
}