using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi;

public abstract class AccesoDatosCadeteria
{
    public abstract Cadeteria ObtenerInfoCadeteria(string rutaDatosCadeteria);

    public abstract List<Cadete> ObtenerListaCadetes(string rutaDatosCadetes);
    public bool ExisteArchivoDatos(string ruta)
    {
        FileInfo archivo = new FileInfo(ruta);

        if (archivo.Exists && archivo.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class AccesoAJson : AccesoDatosCadeteria
{
    public override Cadeteria ObtenerInfoCadeteria(string rutaDatosCadeteria)
    {
        string infoCadeteria = File.ReadAllText(rutaDatosCadeteria);
        Cadeteria cadeteriaConInfo=JsonSerializer.Deserialize<Cadeteria>(infoCadeteria);
        return cadeteriaConInfo;
    }

    public override List<Cadete> ObtenerListaCadetes(string rutaDatosCadete){
        
        string infoDeCadetes = File.ReadAllText(rutaDatosCadete);
        List<Cadete> cadetes=JsonSerializer.Deserialize<List<Cadete>>(infoDeCadetes);
        return cadetes;
    }
}