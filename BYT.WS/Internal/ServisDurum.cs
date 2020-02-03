using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Internal
{
    public class ServisDurum
    {
        public ServisDurumKodlari ServisDurumKodlari { get; set; }
        public int ServisDurumKodu { get; set; }
        public List<Hata> Hatalar { get; set; }
        public List<Bilgi> Bilgiler { get; set; }
        public ServisDurum()
        {
            Hatalar = new List<Hata>();
        }

    }
    public class Bilgi
    {
        public string IslemTipi { get; set; }
        public string ReferansNo { get; set; }
        public string GUID { get; set; }
        public string Sonuc { get; set; }
        public object SonucVeriler { get; set; }
        public string ShortKey { get; set; }
    }
    public class Hata
    {
        public int HataKodu { get; set; }
        public string HataAciklamasi { get; set; }

    }
    public enum ServisDurumKodlari
    {

        [Description("İşlem sırasında beklenmeyen bir hata oluştu: ")]
        IslemBasarisiz = 0,

        [Description("İşlem Başarılı")]
        IslemBasarili = 1,

        [Description("Beklenmeyen Hata")]
        BeklenmeyenHata = 2,

        [Description("Beyanname kayıt aşamasında hata oluştu.")]
        BeyannameKayitHatasi = 3,

        [Description("Kalem kayıt aşamasında hata oluştu.")]
        KalemKayitHatasi = 4,





    }
}
