namespace WebApi;
using System.Text.Json;

public class AccesoDatosCadete
{
    public List<Cadete> ObtenerListaCadetes()
    {
        string rutaDatosCadetes = "DatosCadetes.json";
        FileInfo archivo = new FileInfo(rutaDatosCadetes);
        List<Cadete> listadeCadetes = new List<Cadete>();
        if (archivo.Exists && archivo.Length > 0)
        {
            string DatoCadetes = File.ReadAllText(rutaDatosCadetes);
            listadeCadetes = JsonSerializer.Deserialize<List<Cadete>>(DatoCadetes);
        }

        return listadeCadetes;
    }
}