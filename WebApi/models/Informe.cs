namespace WebApi;


public class Informe
{
    private int cantCadetes;
    private List<int> idCadetes;
    private List<string> nombreDeCadetes;
    private List<int> cantPedidosEntregadosporCadetes;
    private List<double> montosCadetes;
    private int totalPedidosEntregados;
    private int cantPromedioDePedidosEntregados;


    public int CantCadetes { get => cantCadetes; }
    public List<int> IdCadetes { get => idCadetes; }
    public List<string> NombresCadetes { get => nombreDeCadetes; }
    public List<int> CantPedidosEntregadosporCadetes { get => cantPedidosEntregadosporCadetes; }
    public List<double> MontoCadetes { get => montosCadetes; }
    public int TotalPedidosEntregados { get => totalPedidosEntregados; }
    public int CantPromedioDePedidosEntregados { get => cantPromedioDePedidosEntregados; }


    public Informe(int cantCadetes, List<int> idCadetes, List<string> nombreDeCadetes, List<int> cantPedidosEntregadosporCadetes, List<double> montosCadetes, int totalPedidosEntregados, int cantPromedioDePedidosEntregados)
    {
        this.cantCadetes = cantCadetes;
        this.idCadetes = idCadetes;
        this.nombreDeCadetes = nombreDeCadetes;
        this.cantPedidosEntregadosporCadetes = cantPedidosEntregadosporCadetes;
        this.montosCadetes = montosCadetes;
        this.totalPedidosEntregados = totalPedidosEntregados;
        this.cantPromedioDePedidosEntregados = totalPedidosEntregados/cantCadetes;
    }


    private static void MostrarInforme(Informe informe)
    {

        Console.WriteLine("\n************** Informe **************");
        Console.WriteLine($"Cantidad de Cadetes: {informe.CantCadetes}");
        Console.WriteLine($"\nID                Nombre                      cant. Pedidos Entregados            Monto Ganado\n");
        for (int i = 0; i < informe.CantCadetes; i++)
        {
            Console.WriteLine($"{informe.IdCadetes[i]}              {informe.NombresCadetes[i]}                             {informe.CantPedidosEntregadosporCadetes[i]}                                  {informe.MontoCadetes[i]}");
        }
        Console.WriteLine($"\n Total de Pedidos Entregados: {informe.TotalPedidosEntregados}");
        Console.WriteLine($"\n Cantidad promedio de Pedidos Entregados por cadete: {informe.CantPromedioDePedidosEntregados}");


    }
    }