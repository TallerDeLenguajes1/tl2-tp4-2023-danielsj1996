using Microsoft.AspNetCore.Mvc;
using WebApi;
namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private Cadeteria cadeteria;
    private Pedido pedidos;
    private Cadete cadetes;
    private Informe informe;

    private AccesoDatosCadeteria acceso;


    private readonly ILogger<CadeteriaController> _logger;

    public CadeteriaController(ILogger<CadeteriaController> logger)
    {
        _logger = logger;
        cadeteria = Cadeteria.Instance;

    }


    [HttpGet(Name = "GetPedidos")]
    public ActionResult<string> GetCadeteria()
    {

        return Ok(cadeteria);
    }

    [HttpGet(Name = "GetPedidos")]
    [Route("[pedidos]")]

    public ActionResult<string> GetPedidos()
    {

        return Ok(pedidos);
    }


    [HttpGet(Name = "GetCadetes")]
    [Route("Cadete")]
    public ActionResult<string> GetCadetes()
    {
        return Ok(acceso.ObtenerListaCadetes());
    }

    [HttpGet(Name = "GetInforme")]
    [Route("Informe")]
    public ActionResult<string> GetInforme()
    {
        return Ok(informe);
    }
    [HttpPost("Add_Pedidos")]
    public ActionResult<string> AddPedidos(string obsPedido, string nombreCliente, string DireccionCliente, string telefonoCl, string datosRefCl)
    {
        cadeteria.NuevoPedido(obsPedido, nombreCliente, DireccionCliente, telefonoCl, datosRefCl);
        var pedido = cadeteria.ListadePedidos.FirstOrDefault(p => p.Nro == cadeteria.ListadePedidos.Count() - 1);
        if (pedido != null)
        {
            return Ok(pedido);
        }
        return StatusCode(500, "No se pudo tomar el pedido");
    }

    [HttpPut("Asignar_Pedido")]
    public ActionResult<string> AsignarPedido(int idCadete, int numPedido)
    {
        var pedido = cadeteria.ListadePedidos.FirstOrDefault(p => p.Nro == numPedido);
        var cadete = cadeteria.ListadeCadetes.FirstOrDefault(p => p.Id == idCadete);
        if (pedido != null)
        {
            if (cadete != null)
            {
                pedido.IdCadete = idCadete;
                return Ok(pedido);
            }
            return StatusCode(500, "No se pudo encontrar el cadete solicitado");
        }
        return StatusCode(500, "No se pudo encontrar el pedido");
    }
    [HttpPut("Cambiar_Estado_Pedido")]
    public ActionResult<string> CambiarEstadoPedido(int numPedido, int estado)
    {
        var pedido = cadeteria.ListadePedidos.FirstOrDefault(p => p.Nro == numPedido);
        if (pedido != null)
        {
            if (pedido.Estado == EstadoPedido.Pendiente)
            {
                if (estado > 0 && estado < 4)
                {
                    pedido.Estado = (EstadoPedido)Enum.Parse(typeof(EstadoPedido), estado.ToString());
                    return Ok(pedido);
                }
                return StatusCode(500,"El estado que quiere asignar no es valido")
            }else
            {
                if (pedido.Estado==EstadoPedido.Entregado)
                {return NotFound("El pedido ya fue entregado");
                    
                }else
                {
                    return NotFound("El pedido fue Cancelado anteriormente");
                }
            }
            
        }
        return StatusCode(500, "No se pudo encontrar el pedido");
    }
[HttpPut("Cambiar_Cadete_Pedido")]
public ActionResult<string>CambiarCadeteDePedido(int idCadete,int numPedido){
    return AsignarPedido(idCadete,numPedido);
}

}
