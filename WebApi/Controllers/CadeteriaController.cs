using Microsoft.AspNetCore.Mvc;
using WebApi;
namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private static bool datosCargados = false;
    private readonly ILogger<CadeteriaController> _logger;
    private Cadeteria mostaza;

    private List<Cadete> listadeCadetes;

    public CadeteriaController(ILogger<CadeteriaController> logger)
    {
        _logger = logger;
        mostaza = Cadeteria.GetCadeteria();

    }


    [HttpGet]
    public ActionResult<string> GetInfo()
    {
            if (!datosCargados)
        {
            return NotFound("ERROR.Informacion no definida");
        }
        else
        {
            string info = mostaza.Nombre + "," + mostaza.Telefono;
            return Ok(info);
        }
    }


    [HttpGet]
    [Route("Pedido")]

    public ActionResult<IEnumerable<Pedido>> GetPedidos()
    {
        if (mostaza.ListadePedidos.Count != 0)
        {
            return Ok(mostaza.ListadePedidos);
        }
        else
        {
            return NotFound("ERROR. Ningun pedido Cargado Actualmente");
        }
    }


    [HttpGet]
    [Route("Cadete")]
    public ActionResult<IEnumerable<Cadete>> GetCadetes()
    {
        if (!datosCargados)
        {
            return NotFound("ERROR. Informacion no Encontrada");
        }
        else
        {
            return Ok(mostaza.ListadeCadetes);
        }
    }

    [HttpGet]
    [Route("Informe")]
    public ActionResult<Informe> GetInforme()
    {
        if (!datosCargados)
        {
            return BadRequest("ERROR. Acceso denegado");
        }
        else
        {
            return Ok(mostaza.CrearInforme());
        }
    }
    [HttpPost("CargaDatos")]
    public ActionResult<string> CargaInicialDatos(string tipoAcceso)
    {
        if (!mostaza.CargarDatosIniciales(tipoAcceso))
        {
            return StatusCode(500, "ERROR. No se cargaron los datos correctamente.");
        }
        else
        {
            datosCargados = true;
            return Ok("Datos cargados correctamente");
        }
    }
    [HttpPost("Add_Pedidos")]
    public ActionResult<string> AddPedidos(string obsPedido, string nombreCliente, string DireccionCliente, string telefonoCl, string datosRefCl)
    {
        if (!mostaza.NuevoPedido(obsPedido, nombreCliente, DireccionCliente, telefonoCl, datosRefCl))
        {
            return StatusCode(500, "No se pudo tomar el pedido");
        }else
        {
            return Ok("Pedido Agregado Exitosamente");
        }

    }

    [HttpPut("Asignar_Pedido")]
    public ActionResult<string> AsignarPedido(int idCadete, int numPedido)
    {
        if(!mostaza.AsignarCadeteAPedido(idCadete, numPedido)){
            return BadRequest("ERROR. ID de cadete o nro pedido no existentes");
        } else{
            return Ok("Asignación realizada con éxito");
        }
        
    }

    [HttpPut("Cambiar_Estado_Pedido")]
    public ActionResult<string> CambiarEstadoPedido(int numPedido, int estado)
    {
        if (!mostaza.CambiarEstadoPedido(numPedido,estado))
        {
            return StatusCode(500, "No se pudo encontrar el pedido solicitado");
        }else{
            return Ok("Asignacion Realizada Correctamente");
        }
    }
    [HttpPut("Cambiar_Cadete_Pedido")]
    public ActionResult<string> CambiarCadeteDePedido(int idCadete, int numPedido)
    {
              if(!mostaza.ReasignarPedidoACadete(numPedido, idCadete)){
            return BadRequest("ERROR. No se puede realizar la operacion");
        } else{
            return Ok("Cambio de cadete a pedido realizado exitosamente");
        }
    }
    }


