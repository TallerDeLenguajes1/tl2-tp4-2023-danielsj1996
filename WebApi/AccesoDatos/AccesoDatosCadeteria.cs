namespace WebApi;
using System.Text.Json;

public class AccesoDatosCadeteria
{
    public Cadeteria ObtenerInfoCadeteria()
    {
        string rutaDatosCadeteria = "DatosCadeteria.json";
        FileInfo archivo = new FileInfo(rutaDatosCadeteria);
        Cadeteria cadeteriaConInfo=null;
        if (archivo.Exists && archivo.Length > 0)
        {
            string DatosCadeteria = File.ReadAllText(rutaDatosCadeteria);
            cadeteriaConInfo = JsonSerializer.Deserialize<Cadeteria>(DatosCadeteria);
        }

        return cadeteriaConInfo;
    }
}