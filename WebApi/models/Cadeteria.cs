namespace WebApi;


public class Cadeteria
{
    private AccesoDatosPedido accesoDatosPedidos;
    private AccesoDatosCadeteria accesoDatosCadeteria;
    private AccesoDatosCadete accesoDatosCadetes;
    private static AccesoDatos acceso;
    private static Cadeteria instance;
    private string nombre;
    private string telefono;
    private List<Cadete> listadeCadetes;
    private List<Pedido> listadePedidos;




    public string Nombre { get => nombre; set => nombre = value; }
    public string Telefono { get => telefono; set => telefono = value; }
    public List<Cadete> ListadeCadetes { get => accesoDatosCadetes.ObtenerListaCadetes(); }
    public List<Pedido> ListadePedidos { get => accesoDatosPedidos.ObtenerListaPedidos(); }

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
            if (accesoDatosCadeteria.ObtenerInfoCadeteria() != null)
            {
                instance = accesoDatosCadeteria.ObtenerInfoCadeteria();
                instance.accesoDatosCadetes = new AccesoDatosCadete();
                instance.accesoDatosPedidos = new AccesoDatosPedido();

            }
        }
        return instance;
    }



    public bool NuevoPedido(string obsPedido, string nombreCliente, string DireccionCliente, string telefonoCl, string datosRefCl)
    {
        List<Pedido> listadePedidos = accesoDatosPedidos.ObtenerListaPedidos();
        int nroPedido = listadePedidos.Count + 1;
        Pedido pedido = new Pedido(nroPedido, obsPedido, nombreCliente, DireccionCliente, telefonoCl, datosRefCl);
        bool pedidoAgregado = AgregarPedido(pedido, listadePedidos);
        return pedidoAgregado;
    }

    public bool NuevoCadete(string nombreCad, string direccionCad, string telefonoCad)
    {
        List<Cadete> listadeCadetes = accesoDatosCadetes.ObtenerListaCadetes();
        int idCadete = listadeCadetes.Count + 1;
        Cadete cadete = new Cadete(idCadete, nombreCad, direccionCad, telefonoCad);
        bool CadeteAgregado = AgregarCadete(cadete);
        return CadeteAgregado;
    }

    public bool AgregarCadete(Cadete cadete)
    {
        bool agregado = false;
        if (cadete != null)
        {
            var listadeCadetes = accesoDatosCadetes.ObtenerListaCadetes();
            listadeCadetes.Add(cadete);
            agregado = true;
            instance.accesoDatosCadetes.GuardarLista(listadeCadetes);
        }
        return agregado;

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
        List<Cadete> listadeCadetes = accesoDatosCadetes.ObtenerListaCadetes();
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
                    instance.accesoDatosPedidos.GuardarLista(listadePedidos);
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
                instance.accesoDatosPedidos.GuardarLista(listadePedidos);
                return true;
            }
        }

        return false;
    }

    public bool ReasignarPedidoACadete(int nroPedido, int idCadete)
    {
        List<Cadete> listadeCadetes = accesoDatosCadetes.ObtenerListaCadetes();
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
                    instance.accesoDatosPedidos.GuardarLista(listadePedidos);
                }
            }
        }

        return reasignacionOk;
    }
    public int CantPedidosCadete(int idCadete, EstadoPedido estado)
    {
        List<Pedido> listadePedidos = accesoDatosPedidos.ObtenerListaPedidos();
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
        List<Cadete> listadeCadetes = accesoDatosCadetes.ObtenerListaCadetes();
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
    public Pedido ObtenerPedido(int nroPedido){
        var listaPedidos = accesoDatosPedidos.ObtenerListaPedidos();
        Pedido pedido = listaPedidos.FirstOrDefault(p => p.Nro == nroPedido);
        return pedido;
    }

    public Cadete ObtenerCadete(int idCadete){
        var listaCadetes = accesoDatosCadetes.ObtenerListaCadetes();
        Cadete cadete = listaCadetes.FirstOrDefault(cadete => cadete.Id == idCadete);
        return cadete;
    }

}