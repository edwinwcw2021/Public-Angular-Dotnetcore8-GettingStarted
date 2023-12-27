using System.Security.Claims;

namespace datCal
{
  public class Datum
  {
    public enum out_format { dm, dms };

    public void HKGEO(int IG, double X, double Y, ref double PHI, ref double FLAM) 
    { 
      /*    
      'C**** CONVERT HK METRIC GRID COORDINATES TO GEODETIC COORDINTES
      'C ENTRY:
      'C         IG   ;  1 FOR HAYFORD SPHEROID, 2 FOR WGS84 SPHEROID
      'C         X    :  NORTHING  (HK 1980 DATAM)
      'C         Y    :  EASTING
      'C RETURN :
      'C         PHI  : LATITUDE
      'C         FLAM : LONGITUDE
      */
      double RAD, PHI0, FLAM0;
      double a, b, C, D;
      double WX, WY, AA, BB, DPHI;
      double PHIF = 0, DPH, SM, CR;
      double TPHI, TPHI2, TPHI4, TPHI6;
      double TT, TT2, TT3, TT4;
      double DUE, DX, CPHI1, CPHI2;
      double CPHI3, CPHI4, CLAM1, CLAM2;
      double CLAM3, CLAM4, DY, DX2;
      double RHO = 0, RMU = 0;

      RAD = Math.PI / 180;

      if (IG == 1)
      {
        PHI0 = (22d + 18d / 60d + 43.68d / 3600d) * RAD;
        FLAM0 = (114d + 10d / 60d + 42.8d / 3600d) * RAD;
      }
      else
      {

        PHI0 = (22d + 18d / 60d + 38.17d / 3600d) * RAD;
        FLAM0 = (114d + 10d / 60d + 51.65d / 3600d) * RAD;
        a = 1.0000001619d;
        b = 0.000027858d;
        C = 23.098979d;
        D = -23.149125d;
        WX = a * X - b * Y + C;
        WY = b * X + a * Y + D;
        X = WX;
        Y = WY;
      }

      DX = X - 819069.8d;
      DY = Y - 836694.05d;

      //C---- COMPUTE PROVISIONAL PHIF (APPROXIMATE)
      AA = 6.853561524d;
      BB = 110736.3925d;
      DPHI = ((Math.Sqrt(DX * AA * 4 + Math.Pow(BB, 2)) - BB) * 0.5 / AA) * RAD;
      PHIF = PHI0 + DPHI;
      DPH = 0;
      //'C---- EVALUATE PHIF, ITERATE UNTIL CR IS NEAR ZERO
      do
      {
        PHIF = PHIF + DPH;
        SM = SMER(IG, PHI0, PHIF);
        CR = DX - SM;
        RADIUS(IG, PHIF, ref RHO, ref RMU);
        DPH = CR / RHO;
      } while (Math.Abs(CR) < 0.00001);

      //' C---- COMPUTE RADII
      RADIUS(IG, PHIF, ref RHO, ref RMU);
      TPHI = Math.Tan(PHIF);
      TPHI2 = TPHI * TPHI;
      TPHI4 = TPHI2 * TPHI2;
      TPHI6 = TPHI2 * TPHI4;
      TT = RMU / RHO;
      TT2 = Math.Pow(TT, 2);
      TT3 = Math.Pow(TT, 3);
      TT4 = Math.Pow(TT, 4);
      //'C---- COMPUTE LATITUDE
      DUE = DY;
      DX = DUE / RMU;
      DX2 = DX * DX;
      CPHI1 = DUE / RHO * DX * TPHI / 2d;
      CPHI2 = CPHI1 / 12d * DX2 * (9d * TT * (1 - TPHI2) - 4d * TT2 + 12d * TPHI2);
      CPHI3 = CPHI1 / 360d * Math.Pow(DX2, 2) * (8d * TT4 * (11d - 24d * TPHI2) - 12d * TT3 * (21d - 71d * TPHI2) + 15d * TT2 * (15d - 98d * TPHI2 + 15d * TPHI4) + 180d * TT * (5d * TPHI2 - 3d * TPHI4) + 360d * TPHI4);
      CPHI4 = CPHI1 / 20160d * Math.Pow(DX2, 3) * (1385d + 3633d * TPHI2 + 4095d * TPHI4 + 1575d * TPHI2 * TPHI4);
      PHI = PHIF - CPHI1 + CPHI2 - CPHI3 + CPHI4;
      //'C---- COMPUTE LONGITUDE
      CLAM1 = DX / Math.Cos(PHIF);
      CLAM2 = CLAM1 * DX2 / 6d * (TT + 2d * TPHI2);
      CLAM3 = CLAM1 * Math.Pow(DX2, 2) / 120d * (TT2 * (9d - 68d * TPHI2) - 4d * TT3 * (1d - 6d * TPHI2) + 72d * TT * TPHI2 + 24d * TPHI4);
      CLAM4 = CLAM1 * Math.Pow(DX2, 3) / 5040d * (61d + 662d * TPHI2 + 1320d * TPHI4 + 720d * TPHI2 * TPHI4);
      FLAM = FLAM0 + CLAM1 - CLAM2 + CLAM3 - CLAM4;
      //'C---- CONVERT TO DECIMAL DEGREES
      PHI = PHI / RAD;
      FLAM = FLAM / RAD;
    }

    public void GEOHK(int IG, double PHI, double FLAM, ref double X, ref double Y)
    {
      /*
      'C**** CONVERT GEODETIC COORDINTES TO HK METRIC GRID COORDINATES
      'C ENTRY:
      'C         IG   ; 1 FOR HAYFORD SPHEROID, 2 FOR WG282 SPHEROID
      'C         PHI  : LATITUDE   IN DECIMAL DEGREES
      'C         FLAM : LONGITUDE  IN DECIMAL DEGREES
      'C RETURN :
      'C         X    :  NORTHING  (HK 1980 METRIC DATAM)
      'C         Y    :  EASTING
      */
      double RAD, PHI0, FLAM0, RPHI;
      double RLAM, SM0, SM1, CJ;
      double TPHI, TPHI2, TPHI4, TPHI6;
      double TT, TT2, TT3, TT4;
      double XF, X1, X2, X3, X4;
      double YF, Y1, Y2, Y3;
      double WX, WY, a, b, C, D;
      double RHO = 0, RMU = 0;


      RAD = Math.PI / 180d;
      //'C---- CONVERT PROJECTION ORIGIN TO RADIANS
      if (IG == 1)
      {
        PHI0 = (22d + 18d / 60d + 43.68d / 3600d) * RAD;
        FLAM0 = (114d + 10d / 60d + 42.8d / 3600d) * RAD;
      }
      else
      {
        PHI0 = (22d + 18d / 60d + 38.17d / 3600d) * RAD;
        FLAM0 = (114d + 10d / 60d + 51.65d / 3600d) * RAD;
      }
      //'C---- CONVERT LATITUDE AND LONGITUDE TO RADIANS
      RPHI = PHI * RAD;
      RLAM = FLAM * RAD;
      //'C---- COMPUTE MERIDIAN ARCS
      SM0 = SMER(IG, 0d, PHI0);
      SM1 = SMER(IG, 0d, RPHI);
      //'C---- COMPUTE RADII
      RADIUS(IG, RPHI, ref RHO, ref RMU);
      //'C---- COMPUTE CJ (IN RADIANS)
      CJ = (RLAM - FLAM0) * Math.Cos(RPHI);
      TPHI = Math.Tan(RPHI);
      TPHI2 = TPHI * TPHI;
      TPHI4 = TPHI2 * TPHI2;
      TPHI6 = TPHI2 * TPHI4;
      TT = RMU / RHO;
      TT2 = Math.Pow(TT, 2);
      TT3 = Math.Pow(TT, 3);
      TT4 = Math.Pow(TT, 4);
      //'C---- COMPUTE  NORTHING


      XF = SM1 - SM0;
      X1 = RMU / 2d * Math.Pow(CJ, 2) * TPHI;
      X2 = X1 / 12d * Math.Pow(CJ, 2) * (4d * TT2 + TT - TPHI2);
      X3 = X2 / 30d * Math.Pow(CJ, 2) * (8d * TT4 * (11d - 24d * TPHI2) - 28d * TT3 * (1d - 6d * TPHI2) + TT2 * (1d - 32d * TPHI2) - 2d * TT * TPHI2 + TPHI4);
      X4 = X3 / 56d * Math.Pow(CJ, 2) * (1385d - 3111d * TPHI2 + 543d * TPHI4 - TPHI6);
      X = XF + X1 + X2 + X3 + X4 + 819069.8d;
      //'C---- COMPUTE  EASTING
      YF = RMU * CJ;
      Y1 = YF / 6d * Math.Pow(CJ, 2);
      Y2 = Y1 / 20d * Math.Pow(CJ, 2);
      Y3 = Y2 / 42d * Math.Pow(CJ, 2);
      Y1 = Y1 * (TT - TPHI2);
      Y2 = Y2 * (4d * TT3 * (1d - 6d * TPHI2) + TT2 * (1d + 8d * TPHI2) - TT * 2d * TPHI2 + TPHI4);
      Y3 = Y3 * (61d - 479d * TPHI2 + 179d * TPHI4 - TPHI6);
      Y = YF + Y1 + Y2 + Y3 + 836694.05d;
      if (IG == 2)
      {
        WX = X;
        WY = Y;
        //'C----   TRANSFROM WGS84 GRID TO HK 1980 GRID
        a = 0.9999998373d;
        b = -0.000027858d;
        C = -23.098331d;
        D = 23.149765d;
        X = a * WX - b * WY + C;
        Y = b * WX + a * WY + D;
      }
    }



    private double SMER(int IG, double PHI0, double PHIF)
    {
      double AXISM = 0, FLAT = 0, ECC = 0, a = 0, b = 0, C = 0, D = 0, DP0 = 0;
      double DPO = 0, DP2 = 0, DP4 = 0, DP6 = 0, SMER = 0;

      if (IG == 1)
      {
        AXISM = 6378388d;
        FLAT = 1d / 297d;
      }
      else
      {
        AXISM = 6378137d;
        FLAT = 1d / 298.2572235634d;
      }

      ECC = 2d * FLAT - Math.Pow(FLAT, 2);
      ECC = Math.Sqrt(ECC);
      a = 1d + 3d / 4d * Math.Pow(ECC, 2) + 45d / 64d * Math.Pow(ECC, 4) + 175d / 256d * Math.Pow(ECC, 6);
      b = 3d / 4d * Math.Pow(ECC, 2) + 15d / 16d * Math.Pow(ECC, 4) + 525d / 512d * Math.Pow(ECC, 6);
      C = 15d / 64d * Math.Pow(ECC, 4) + 105d / 256d * Math.Pow(ECC, 6);
      D = 35d / 512d * Math.Pow(ECC, 6);
      DP0 = PHIF - PHI0;
      DP2 = Math.Sin(2d * PHIF) - Math.Sin(2d * PHI0);
      DP4 = Math.Sin(4d * PHIF) - Math.Sin(4d * PHI0);
      DP6 = Math.Sin(6d * PHIF) - Math.Sin(6d * PHI0);
      SMER = AXISM * (1d - Math.Pow(ECC, 2));
      SMER = SMER * (a * DP0 - b * DP2 / 2d + C * DP4 / 4d - D * DP6 / 6d);
      return SMER;
    }

    private void RADIUS(int IG, double PHI, ref double RHO, ref double RMU)
    {
      double AXISM = 0, FLAT = 0, ECC = 0, FAC = 0;
      if (IG == 1)
      {
        AXISM = 6378388d;
        FLAT = 1d / 297d;
      }
      else
      {
        AXISM = 6378137d;
        FLAT = 1d / 298.2572235634d;
      }
      ECC = 2d * FLAT - Math.Pow(FLAT, 2);
      FAC = 1d - ECC * Math.Pow(Math.Sin(PHI), 2);
      RHO = AXISM * (1d - ECC) / Math.Pow(FAC, 1.5);
      RMU = AXISM / Math.Sqrt(FAC);
    }

    public void To_dms(double n, ref int D, ref int m, ref double s)
    {
      D = (int)Math.Floor(n);
      m = (int)Math.Floor((n - D) * 60);
      s = ((n - D) * 60 - m) * 60;
    }

    public void To_dms(double n, ref int D, ref double m)
    {
      D = (int)Math.Floor(n);
      m = Math.Round((n - D) * 60,2);
    }

    public string dms(double n, out_format fmt)
    {
      string ret = "";
      int D = 0, m = 0;
      double s = 0, dm = 0;
      switch(fmt)
      {        
        case out_format.dms:
          To_dms(n, ref D, ref m, ref s);
          ret = string.Format("{0}° {1}' {2:0.##}''", D, m, s);
          break;
        default:
          To_dms(n, ref D, ref dm);
          ret = string.Format("{0}° {1:0.##}'", D, dm);
          break;
      }
      return ret;
    }
  }
}