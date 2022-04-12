using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnQLNT.Models
{
    public class Giohang
    {
        MydataDataContext data = new MydataDataContext();
        public int iMant { set; get; }
        public string sTennt { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public int iSoluongton { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int MaNOITHAT)
        {
            iMant = MaNOITHAT;
            NOITHAT noithat = data.NOITHATs.Single(n => n.MaNOITHAT == iMant);
            sTennt = noithat.TenNOITHAT;
            sAnhbia = noithat.Anhbia;
            dDongia = double.Parse(noithat.Giaban.ToString());
            iSoluong = 1;
            iSoluongton = int.Parse(noithat.Soluongton.ToString());
        }
    }
}