namespace WebApi;


public class Cadeteria
{
    private AccesoDatosPedido accesoDatosPedidos;
    private AccesoDatosCadeteria accesoDatosCadeteria;
    private AccesoDatosCadete accesoDatosCadetes;
    private AccesoDatos acceso;

    private static Cadeteria instance;
    private List<Cadete> listadeCadetes;
    private string nombre;
    private string telefono;




    public string Nombre { get => nombre; set => nombre = value; }
    public string Telefono { get => telefono; set => telefono = value; }
    public List<Cadete> ListadeCadetes { get => listadeCadetes; }
    public AccesoDatosPedido AccesoDatosPedidos { get => accesoDatosPedidos; }

    private Cadeteria() { }

    public Cadeteria(string nombre, string telefono)
    {
        this.Nombre = nombre;
        this.Telefono = telefono;
        
    }
    public static Cadeteria GetCadeteria()
    {
        if (instance == null)
        {
            var accesoDatosCadeteria = new AccesoDatosCadeteria();
            if (accesoDatosCadeteria.ObtenerInfoCadeteria != null)
            {
                instance = accesoDatosCadeteria.ObtenerInfoCadeteria();
                instance.accesoDatosCadetes = new AccesoDatosCadete();
                if (instance.accesoDatosCadetes.ObtenerListaCadetes().Count != null)
                {
                    instance.AgregarListaCadetes(instance.accesoDatosCadetes.ObtenerListaCadetes());
                    instance.accesoDatosPedidos = new AccesoDatosPedido();
                }
            }
        }
        return instance;
    }



    public void AgregarListaCadetes(List<Cadete> listadeCadetes)
    {
        this.listadeCadetes = listadeCadetes;
    }


    public bool NuevoPedido(string obsPedido, string nombreCliente, string DireccionCliente, string telefonoCl, string datosRefCl)
    {
        List<Pedido> listadePedidos = accesoDatosPedidos.ObtenerListaPedidos();
        int nroPedido = listadePedidos.Count + 1;
        Pedido pedido = new Pedido(nroPedido, obsPedido, nombreCliente, DireccionCliente, telefonoCl, datosRefCl);
        bool pedidoAgregado = AgregarPedido(pedido, listadePedidos);
        return pedidoAgregado;
    }
    public bool AgregarPedido(Pedido ped, List<Pedido> listadePedidos)
    {
        bool agregado = false;
        if (ped != null)
        {
            listadePedidos.Add(ped);
            instance.accesoDatosPedidos.GuardarLista(listadePedidos);
            agregado = true;
        }
        return agregado;

    }
    public bool AsignarCadeteAPedido(int idCadete, int nroPedido)
    {
        List<Pedido> listadePedidos = accesoDatosPedidos.ObtenerListaPedidos();
        bool asignacionOK = false;
        Cadete cad = listadeCadetes.Find(x => x.Id == idCadete);

        if (cad != null)
        {
            foreach (var p in listadePedidos)
            {
                if (p.Nro == nroPedido)
                {
                    p.VincularCadate(cad);
                    asignacionOK = true;
                    instance.AccesoDatosPedidos.GuardarLista(listadePedidos);
                    break;
                }
            }
        }

        return asignacionOK;
    }


    public bool CambiarEstadoPedido(int nroPedido, int nuevoEstado)
    {
        List<Pedido> listadePedidos = accesoDatosPedidos.ObtenerListaPedidos();
        foreach (var p in listadePedidos)
        {
            if (p.Nro == nroPedido)
            {
                p.CambiarEstado(nuevoEstado);
                instance.AccesoDatosPedidos.GuardarLista(listadePedidos);
                return true;
            }
        }

        return false;
    }

    public bool ReasignarPedidoACadete(int nroPedido, int idCadete)
    {
        List<Pedido> listadePedidos = accesoDatosPedidos.ObtenerListaPedidos();
        bool reasignacionOk = false;
        Cadete cad = listadeCadetes.Find(cadete => cadete.Id == idCadete);

        if (cad != null)
        {
            foreach (var p in listadePedidos)
            {
                if (p.Nro == nroPedido && p.Estado != EstadoPedido.Entregado)
                {
                    p.VincularCadate(cad);
                    reasignacionOk = true;
                    instance.AccesoDatosPedidos.GuardarLista(listadePedidos);
                }
            }
        }

        return reasignacionOk;
    }
    public int CantPedidosCadete(int idCadete, EstadoPedido estado)
    {List<Pedido> listadePedidos = accesoDatosPedidos.ObtenerListaPedidos();
        int cant = 0;
        foreach (var p in listadePedidos)
        {
            if (p.ExisteCadete() && (p.IdCadete() == idCadete) && (p.Estado == estado))
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
            cantPedidosEntregadosporCadetes.Add(CantPedidosCadete(cad.Id, EstadoPedido.Entregado));
            montosCadetes.Add(JornalACobrar(cad.Id));
        }
        int totalPedidosEntregados = cantPedidosEntregadosporCadetes.Sum();
        int cantPromedioDePedidosEntregados = (int)cantPedidosEntregadosporCadetes.Average();

        Informe informe = new Informe(listadeCadetes.Count, idCadetes, NombreDeCadetes, cantPedidosEntregadosporCadetes, montosCadetes, totalPedidosEntregados, cantPromedioDePedidosEntregados);
        return informe;

    }
}