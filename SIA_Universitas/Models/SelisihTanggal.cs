using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIA_Universitas.Models
{
    public class SelisihTanggal
    {
        public DateTime OriTanggal_1 { get; set; }
        public DateTime OriTanggal_2 { get; set; }
        public int Tahun_1 { get { return OriTanggal_1.Year; } set { } }
        public int Tahun_2 { get { return OriTanggal_2.Year; } set { } }
        public int Bulan_1 { get { return OriTanggal_1.Month; } set { } }
        public int Bulan_2 { get { return OriTanggal_2.Month; } set { } }
        public int Tanggal_1 { get { return OriTanggal_1.Day; } set { } }
        public int Tanggal_2 { get { return OriTanggal_2.Day; } set { } }
        public short Tahun
        {
            get
            {
                if (new DateTime(Tahun_2,Bulan_1,Tanggal_1) <= OriTanggal_2)
                {
                    return Convert.ToByte(Tahun_2 - Tahun_1);
                }
                else
                {
                    return Convert.ToByte(Tahun_2 - Tahun_1 - 1);
                }
            }
            set { }
        }
        public short Bulan
        {
            get
            {
                int result = 0;
                if (Tanggal_2 >= Tanggal_1)
                {
                    result = Bulan_2 - Bulan_1;
                }
                else
                {
                    result = Bulan_2 - Bulan_1 - 1;
                }
                if (result < 0)
                {
                    result = result + 12;
                }
                return Convert.ToByte(result);
            }
            set { }
        }
        public short Hari
        {
            get
            {
                if (Tanggal_2 >= Tanggal_1)
                {
                    return Convert.ToByte(Tanggal_2 - Tanggal_1);
                }
                else
                {
                    if (Bulan_2 != 1)
                    {
                        if (!IsDate(Tahun_2, Bulan_2 - 1, Tanggal_1))
                        {
                            Tanggal_1 = TglAkhirBulan(Tahun_2, Bulan_2 - 1);
                        }
                        return Convert.ToByte(OriTanggal_2.Subtract(new DateTime(Tahun_2, Bulan_2 - 1, Tanggal_1)).Days);
                    }
                    else
                    {
                        return Convert.ToByte(OriTanggal_2.Subtract(new DateTime(Tahun_2 - 1, 12, Tanggal_1)).Days);
                    }
                }
            }
            set { }
        }
        public decimal JmlTahun { get { return Convert.ToDecimal(OriTanggal_2.Subtract(OriTanggal_1).Days) /  Convert.ToDecimal(365.25); } set { } }
        public int JmlHari { get { return Convert.ToInt32(OriTanggal_2.Subtract(OriTanggal_1).Days); } set { } }

        public bool IsDate(int year, int month, int day)
        {
            return day <= DateTime.DaysInMonth(year, month);
        }
        public int TglAkhirBulan(int year, int mont)
        {
            if (year == null || mont == null) { return 0; }
            if (mont < 1 || mont > 12) { return 0; }
            if (mont < 12) { mont++; } else { mont = 1; year++; }

            DateTime result = new DateTime(year, mont, 1);
            return result.AddDays(-1).Day;
        }
    }
}