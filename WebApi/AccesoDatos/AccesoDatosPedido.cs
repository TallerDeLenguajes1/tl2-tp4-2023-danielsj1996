namespace WebApi;
using System.Text.Json;

public class AccesoDatosPedido
{
    public List<Pedido> ObtenerListaPedidos()
    {
        string rutaDatosPedidos = "RegistroPedidos.json";
        FileInfo archivo = new FileInfo(rutaDatosPedidos);
        List<Pedido> listadePedidos = new List<Pedido>();
        if (archivo.Exists && archivo.Length > 0)
        {
            string DatoPedidos = File.ReadAllText(rutaDatosPedidos);
            listadePedidos = JsonSerializer.Deserialize<List<Pedido>>(DatoPedidos);
        }

        return listadePedidos;
    }

        public void GuardarLista(List<Pedido> Pedidos){
        string rutaDatosPedidos = "RegistroPedidos.json";
        string infoLista = JsonSerializer.Serialize(Pedidos);
        File.WriteAllText(rutaDatosPedidos, infoLista);
    }
}