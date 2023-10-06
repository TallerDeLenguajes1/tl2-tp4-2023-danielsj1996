using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi;
public class Cadeteria
{
    private AccesoDatosCadeteria acceso;
    private static Cadeteria instance;
    private string nombre;
    private string telefono;
    private List<Cadete> listadeCadetes;
    private List<Pedido> listadePedidos;



    public string Nombre { get => nombre; set => nombre = value; }
    public string Telefono { get => telefono; set => telefono = value; }
    public List<Cadete> ListadeCadetes { get => listadeCadetes; }
    public List<Pedido> ListadePedidos { get => listadePedidos; set => listadePedidos = value; }



    private Cadeteria() { }

    public Cadeteria(string nombre, string telefono)
    {
        this.Nombre = nombre;
        this.Telefono = telefono;
        this.ListadePedidos = new List<Pedido>();
    }
    public static Cadeteria GetCadeteria()
    {
        if (instance == null)
        {
            instance = new Cadeteria();
            return instance;
        }
        else
        {
            return instance;
        }
    }    public bool CargaDatosIniciales(string tipoAcceso){
        bool cargaRealizada = false;
        List<Cadete> listacadetes = new List<Cadete>();
        if(tipoAcceso == "csv"){
            acceso = new AccesoCSV();
            if(acceso.ExisteArchivoDatos("datosCadeteria.csv")){
                instance = acceso.ObtenerInfoCadeteria("datosCadeteria.csv");
                if(acceso.ExisteArchivoDatos("datosCadetes.csv")){
                    listacadetes = acceso.ObtenerListaCadetes("datosCadetes.csv");
                    cargaRealizada = true;
                }
                instance.AgregarListaCadetes(listacadetes);
            } 
        } else{
            acceso = new AccesoAJson();
            if(acceso.ExisteArchivoDatos("../DatosCadeteria.json")){
                instance = acceso.ObtenerInfoCadeteria("../DatosCadeteria.json");
                if(acceso.ExisteArchivoDatos("../DatosCadetes.json")){
                    listadeCadetes = acceso.ObtenerListaCadetes("../DatosCadetes.json");
                    cargaRealizada = true;
                }
                instance.AgregarListaCadetes(listacadetes);
            } 
        }

        return cargaRealizada;
    }


    public void AgregarListaCadetes(List<Cadete> listadeCadetes)
    {
        this.listadeCadetes = listadeCadetes;
    }


    public bool NuevoPedido(string obsPedido, string nombreCliente, string DireccionCliente, string telefonoCl, string datosRefCl)
    {
        int nroPedido = listadePedidos.Count+1;
        Pedido pedido = new Pedido(nroPedido, obsPedido, nombreCliente, DireccionCliente, telefonoCl, datosRefCl);
        bool pedidoAgregado = AgregarPedido(pedido);
        return pedidoAgregado;
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
    public bool AsignarCadeteAPedido(int idCadete, int nroPedido)
    {
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
                    break;
                }
            }
        }

        return asignacionOK;
    }


    public bool CambiarEstadoPedido(int nroPedido, int nuevoEstado)
    {
        foreach (var p in listadePedidos)
        {
            if (p.Nro == nroPedido)
            {
                p.CambiarEstado(nuevoEstado);
                return true;
            }
        }

        return false;
    }

    public bool ReasignarPedidoACadete(int nroPedido, int idCadete)
    {
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
                }
            }
        }

        return reasignacionOk;
    }
        public int CantPedidosCadete(int idCadete, EstadoPedido estado)
    {
        int cant = 0;
        foreach (var p in ListadePedidos)
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