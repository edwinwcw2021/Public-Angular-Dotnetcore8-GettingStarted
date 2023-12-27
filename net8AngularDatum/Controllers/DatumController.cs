using datCal;
using datCal.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;


namespace net8AngularDatum.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DatumController : Controller
  {
#pragma warning disable 1998
    [HttpPost("getGEOByHKG")]
    [ProducesResponseType(200, Type = typeof(Geo))]
    public async Task<IActionResult> GetResultGeo([FromForm] HKG req)
    {      
      Geo res = new Geo();
      double lat = 0, lng = 0, mlat = 0, mlng = 0;
      int dlat = 0, dlng = 0;
      Common.AppLog(JsonConvert.SerializeObject(req, Formatting.Indented));
      try
      {
        Datum dat = new Datum();
        dat.HKGEO(2, req.x, req.y, ref lat, ref lng);
        dat.To_dms(lat, ref dlat, ref mlat);
        dat.To_dms(lng, ref dlng, ref mlng);
        res.latitudeDeg = dlat;
        res.longitudeDeg = dlng;
        res.latitudeMin = mlat;
        res.longitudeMin = mlng;
        Common.AppLog(JsonConvert.SerializeObject(res, Formatting.Indented));
        return Json(res);
      } catch (Exception ex)
      {
        Common.ErrLog(ex.ToString()); 
        return Json(NoContent());
      }
    }

    [HttpPost("getHKGByGEO")]
    [ProducesResponseType(200, Type = typeof(HKG))]
    public async Task<IActionResult> GetResultHKG([FromForm] Geo req)
    {
      HKG res = new HKG();
      Datum dat = new Datum();
      double x = 0, y = 0;
      double lat = req.latitudeDeg + req.latitudeMin / 60d;
      double lng = req.longitudeDeg + req.longitudeMin / 60d;
      Common.AppLog(JsonConvert.SerializeObject(req, Formatting.Indented));
      try
      {
        dat.GEOHK(2, lat, lng, ref x, ref y);
        res.x = Math.Round(x, 2);
        res.y = Math.Round(y, 2);
        Common.AppLog(JsonConvert.SerializeObject(res, Formatting.Indented));
        return Json(res);
      } catch(Exception ex)
      {
        Common.ErrLog(ex.ToString());
        return Json(NoContent());
      }
    }
#pragma warning restore 1998

  }
}
