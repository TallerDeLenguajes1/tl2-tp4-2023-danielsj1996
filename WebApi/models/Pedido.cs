namespace WebApi;

public enum EstadoPedido
{
    Entregado,
    Pendiente,
    Candelado,

}

public class Pedido
{
    private int nro;
    private string? observaciones;
    private EstadoPedido estado;
    private Cliente cliente;
    private int idCadete;


    public int Nro { get => nro; set => nro = value; }
    public string? Observaciones { get => observaciones; set => observaciones = value; }
    public EstadoPedido Estado { get => estado; set => estado = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }
    public int IdCadete { get => idCadete; set => idCadete = value; }



    public Pedido(int numero, string observaciones, Cliente cliente)
    {
        Nro = numero;
        Observaciones = observaciones;
        Estado = EstadoPedido.Pendiente;
        Cliente = cliente;
        IdCadete = 0;


    }
    public void Entregado()
    {
        Estado = EstadoPedido.Entregado;
    }
    public void CancelarPedido()
    {

        Estado = EstadoPedido.Candelado;
    }
}