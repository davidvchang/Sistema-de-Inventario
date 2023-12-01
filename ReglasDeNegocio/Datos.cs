using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReglasDeNegocio
{
    public class Datos
    {

        public ArrayList renglonesDetalle = new ArrayList();
        public Int32 sFolio;
        public DateTime sFecha;
        public Double sTotal;
        public String sTipoMovimiento;

        public void Row(String sProductoId, Double dPrecio, Double dCantidad)
        {
            ArrayList row = new ArrayList();
            row.Add(sProductoId);
            row.Add(dPrecio);
            row.Add(dCantidad);
            renglonesDetalle.Add(row);

        }

        public void GetRow(Int32 sNumRow, ref String sProductoId, ref Double dPrecio, ref Double dCantidad)
        {
            ArrayList row = (ArrayList)renglonesDetalle[sNumRow];
            sProductoId = row[0].ToString();
            dPrecio = Double.Parse(row[1].ToString());
            dCantidad = Double.Parse(row[2].ToString());

        }


        public Int32 NumRow()
        {
            return renglonesDetalle.Count;

        }

    }
}
