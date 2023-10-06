using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

public enum EstadoPedido
{
    [EnumMember(Value = "Entregado")]
    Entregado,
    [EnumMember(Value = "Pendiente")]
    Pendiente,
    [EnumMember(Value = "Cancelado")]
    Cancelado

}

public class Pedido
{
    private int nro;
    private string? observaciones;
    private EstadoPedido estado;
    private Cliente cliente;
    private Cadete cadete;

    public int Nro { get => nro; set => nro = value; }
    public string? Observaciones { get => observaciones; set => observaciones = value; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EstadoPedido Estado { get => estado; set => estado = value; }
    
    public Cliente Cliente { get => cliente; set => cliente = value; }
    
    public Cadete Cadete { get => cadete; set => cadete = value; }


    public Pedido()
    {
        this.cliente = new Cliente();
    }

    public Pedido(int numero, string observaciones, string nombre, string direccion, string telefonoCl, string datosRefCl)
    {
        Nro = numero;
        Observaciones = observaciones;
        Estado = EstadoPedido.Pendiente;
        Cliente = new Cliente(nombre, direccion, telefonoCl, datosRefCl);

    }
    public void VincularCadate(Cadete cad)
    {
        cadete = cad;
    }
    
    public void CambiarEstado(int estado)
    {
        switch (estado)
        {
            case 0: this.Estado = EstadoPedido.Pendiente;
            break;
            case 1: this.Estado = EstadoPedido.Entregado;
            break;
            case 2: this.Estado = EstadoPedido.Cancelado;
            break;
        }
    }
    public bool ExisteCadete()
    {
        return cadete != null;
    }

      public int IdCadete()
    {
        return cadete.Id;
    }
}