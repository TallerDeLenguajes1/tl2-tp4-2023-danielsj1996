using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi;

public abstract class AccesoDatos
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
public class AccesoCSV : AccesoDatos
{
    
    public override Cadeteria ObtenerInfoCadeteria(string rutaDatosCadeteria){
        string[] datosCadeteria;

        using (StreamReader archivo = new StreamReader(rutaDatosCadeteria))
        {
            datosCadeteria = archivo.ReadLine().Split(',');
        }

        Cadeteria cadeteria = new Cadeteria(datosCadeteria[0], datosCadeteria[1]);
        return cadeteria;
    } 

    public override List<Cadete> ObtenerListaCadetes(string rutaDatosCadetes){
        List<Cadete> cadetes = new List<Cadete>();

        string linea = "";
        string[] datosCadete;

        using(StreamReader archivo = new StreamReader(rutaDatosCadetes))
        {
            while((linea = archivo.ReadLine()) != null){
                datosCadete = linea.Split(',');
                Cadete cadete = new Cadete(Convert.ToInt32(datosCadete[0]), datosCadete[1], datosCadete[2], datosCadete[3]);
                cadetes.Add(cadete);
            }
        }

        return cadetes;
    }
}
public class AccesoAJson : AccesoDatos
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