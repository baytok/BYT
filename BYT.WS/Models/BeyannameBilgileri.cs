using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
    //[FluentValidation.Attributes.Validator(typeof(BeyannameBilgileriModelValidator))]
    public class BeyannameBilgileri
    {

        public DbBeyan beyanname { get; set; }
        public List<DbKalem> kalemler { get; set; }
        public List<DbTamamlayiciBilgi> tamamlayiciBilgi { get; set; }
        public List<DbBeyannameAcma> tcgbAcma { get; set; }
        public List<DbMarka> marka { get; set; }
        public List<DbKonteyner> konteyner { get; set; }
        public List<DbOdemeSekli> odemeSekli { get; set; }
        public List<DbOzetBeyanAcma> ozetBeyanAcma { get; set; }
        public List<DbOzetBeyanAcmaTasimaSenet> tasimaSenetleri { get; set; }
        public List<DbOzetBeyanAcmaTasimaSatir> tasimaSatirlari { get; set; }
        public List<DbFirma> firma { get; set; }
        public List<DbTeminat> teminat { get; set; }
        public List<DbKiymetBildirim> kiymet { get; set; }
        public List<DbKiymetBildirimKalem> kiymetKalem { get; set; }


    }
    public class BeyannameBilgileriModelValidator : AbstractValidator<BeyannameBilgileri>
    {
        public BeyannameBilgileriModelValidator()
        {
            RuleFor(x => x.beyanname).NotNull().WithMessage("Beyanname Bilgileri Boş olamaz");
            RuleFor(x => x.kalemler).NotNull().WithMessage("Kalem Bilgileri Boş olamaz");
            // RuleFor(x => x.Alici_adsoyad)
            //.MaximumLength(150).WithMessage("Alıcı AdSoyad 150 karakteri  geçemez");
            // RuleFor(x => x.Alici_ulke)
            //.MaximumLength(30).WithMessage("Alıcı Ülke 30 karakteri  geçemez");
            // RuleFor(x => x.Belge_no)
            //.NotNull().WithMessage("Belge No Boş olamaz").Equal("0321").WithMessage("Belge No 0321 olması gerekli")
            //.MaximumLength(30).WithMessage("Belge No 30 karakteri  geçemez");
            // RuleFor(x => x.Belge_olusturma_tarihi)
            //.NotNull().WithMessage("Belge Oluşturma Tarihi Boş olamaz");
            // RuleFor(x => x.Belge_seri_no)
            //.NotNull().WithMessage("Belge Seri No Boş olamaz").MaximumLength(10).WithMessage("Belge Seri No 10 karakteri  geçemez");
            // RuleFor(x => x.Beyanname_no)
            //.MaximumLength(20).WithMessage("Beyanname No 20 karakteri  geçemez");
            // RuleFor(x => x.Beyan_tipi)
            //.NotNull().WithMessage("Beyan Tipi Boş olamaz")
            //.MaximumLength(9).WithMessage("Beyanname Tipi 9 karakteri  geçemez")
            //.NotEqual("TIR").Unless(x => x.Tasima_belgesi_no != "").WithMessage("Taşıma Belgesi No için Tır Karne Numarası girilmesi gerekli");
            // RuleFor(x => x.Gozlemler)
            //.MaximumLength(250).WithMessage("Gozlemler 250 karakteri  geçemez");
            // RuleFor(x => x.Gumruk_kodu)
            //.NotNull().WithMessage("Gümrük Boş olamaz").MaximumLength(6).WithMessage("Gümrük 6 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_adres)
            //.NotNull().WithMessage("İhracatçı Adres Boş olamaz").MaximumLength(250).WithMessage("İhracatçı Adres 250 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_ulke)
            //.NotNull().WithMessage("İhracatçı Ülke Boş olamaz").MaximumLength(30).WithMessage("İhracatçı Ülke 30 karakteri  geçemez");
            // RuleFor(x => x.Ihrac_ulke_grup)
            //.NotNull().WithMessage("İhrac Ülke Grup Boş olamaz").MaximumLength(30).WithMessage("İhrac Ülke Grup 30 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_unvan)
            //.NotNull().WithMessage("İhracatçı Ünvanı Boş olamaz").MaximumLength(250).WithMessage("İhracatçı Ünvanı 250 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_vergi_no)
            //.NotNull().WithMessage("İhracatçı Vergi No Boş olamaz").MaximumLength(11).WithMessage("İhracatçı Vergi No 11 karakteri  geçemez");
            // RuleFor(x => x.Ikinci_nusha)
            //.MaximumLength(9).WithMessage("İkinci Nüsha 9 karakteri  geçemez")
            //.NotEqual("1").Unless(x => x.Ilk_nusha_id != "").WithMessage("İkinci Nüsha girilmesi gerekli");
            // RuleFor(x => x.Ikinci_nusha)
            //.NotEqual("3").Unless(x => x.Beyanname_no != "").WithMessage("Beyanname Numarası ve Tescil Tarihi girilmesi gerekli");
            // RuleFor(x => x.Ilk_nusha_id)
            //.MaximumLength(30).WithMessage("Ilk Nüsha ID 30 karakteri  geçemez");
            // RuleFor(x => x.Kullanici)
            //.NotNull().WithMessage("Kullanıcı Boş olamaz").MaximumLength(11).WithMessage("Kullanıcı 11 karakteri  geçemez");
            // RuleFor(x => x.Kurum)
            //.NotNull().WithMessage("Kurum Boş olamaz").MaximumLength(9).WithMessage("Kurum 9 karakteri  geçemez");
            // RuleFor(x => x.Mersis_no)
            //.MaximumLength(30).WithMessage("Mersis No 30 karakteri  geçemez");
            // RuleFor(x => x.Mesaj)
            //.MaximumLength(50).WithMessage("Mesaj 50 karakteri  geçemez");
            // RuleFor(x => x.Tasima_bilgileri)
            //.MaximumLength(250).WithMessage("Taşıma Bilgileri 250 karakteri  geçemez");
            // RuleFor(x => x.Tasima_belgesi_no)
            //.MaximumLength(30).WithMessage("Taşıma Belgesi Numarası 30 karakteri  geçemez");
            // RuleFor(x => x.Varis_ulke_grup)
            //.NotNull().WithMessage("Varış Ülke Boş olamaz").MaximumLength(30).WithMessage("Varış Ülke 30 karakteri  geçemez");
            // RuleFor(x => x._Ticari_tanimlar)
            //.NotNull().WithMessage("Ticari Tanım Boş olamaz");



        }
    }
    public class DbBeyan
    {
      
        [StringLength(30)]
        public string RefNo { get; set; }

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BeyanInternalNo { get; set; }

        
        [StringLength(20)]
        public string? BeyannameNo { get; set; }



        [StringLength(350)]
        public string Aciklamalar { get; set; }

        [StringLength(9)]
        public string AliciSaticiIliskisi { get; set; }
       
        [StringLength(20)]
        public string AliciVergiNo { get; set; }

        [StringLength(9)]
        public string AntrepoKodu { get; set; }

        [StringLength(20)]
        public string AsilSorumluVergiNo { get; set; }

        [StringLength(9)]
        public string BasitlestirilmisUsul { get; set; }

        [Required]
        [StringLength(16)]
        public string BankaKodu { get; set; }
        [Required]
        [StringLength(20)]
        public string BeyanSahibiVergiNo { get; set; }

        [StringLength(30)]
        public string BirlikKriptoNumarasi { get; set; }

        [StringLength(30)]
        public string BirlikKayitNumarasi { get; set; }

        [StringLength(9)]
        public string CikisUlkesi { get; set; }

        [StringLength(35)]
        public string CikistakiAracinKimligi { get; set; }

        [StringLength(9)]
        public string CikistakiAracinTipi { get; set; }

        [StringLength(9)]
        public string CikistakiAracinUlkesi { get; set; }

        [StringLength(40)]
        public string EsyaninBulunduguYer { get; set; }

        [StringLength(9)]
        public string GidecegiSevkUlkesi { get; set; }

        [StringLength(9)]
        public string GidecegiUlke { get; set; }

        [StringLength(9)]
        public string GirisGumrukIdaresi { get; set; }

      
        [StringLength(20)]
        public string GondericiVergiNo { get; set; }

        [Required]
        [StringLength(9)]
        public string Gumruk { get; set; }

        [StringLength(9)]
        public string IsleminNiteligi { get; set; }

        public int KapAdedi { get; set; }

        [StringLength(9)]
        public string Konteyner { get; set; }

        [Required]
        [StringLength(15)]
        public string Kullanici { get; set; }

        [StringLength(9)]
        public string LimanKodu { get; set; }

        [StringLength(50)]
        public string Mail1 { get; set; }

        [StringLength(50)]
        public string Mail2 { get; set; }

        [StringLength(50)]
        public string Mail3 { get; set; }

        [StringLength(30)]
        public string Mobil1 { get; set; }

        [StringLength(30)]
        public string Mobil2 { get; set; }

        [StringLength(20)]
        public string MusavirVergiNo { get; set; }

        [StringLength(9)]
        public string OdemeAraci { get; set; }


        [StringLength(12)]
        public string MusavirReferansNo { get; set; }

        [StringLength(12)]
        public string ReferansTarihi { get; set; }

        [Required]
        [StringLength(9)]
        public string Rejim { get; set; }

        [StringLength(35)]
        public string SinirdakiAracinKimligi { get; set; }

        [StringLength(9)]
        public string SinirdakiAracinTipi { get; set; }

        [StringLength(9)]
        public string SinirdakiAracinUlkesi { get; set; }

        [StringLength(9)]
        public string SinirdakiTasimaSekli { get; set; }

        [StringLength(250)]
        public string TasarlananGuzergah { get; set; }

        public decimal TelafiEdiciVergi { get; set; }

        [StringLength(50)]
        public string TescilStatu { get; set; }

        public DateTime? TescilTarihi { get; set; }      


        [StringLength(9)]
        public string TeslimSekli { get; set; }

        [StringLength(40)]
        public string TeslimSekliYeri { get; set; }

        [StringLength(9)]
        public string TicaretUlkesi { get; set; }

        public decimal ToplamFatura { get; set; }

        [StringLength(9)]
        public string ToplamFaturaDovizi { get; set; }

        public decimal ToplamNavlun { get; set; }

        [StringLength(9)]
        public string ToplamNavlunDovizi { get; set; }

        public decimal ToplamSigorta { get; set; }

        [StringLength(9)]
        public string ToplamSigortaDovizi { get; set; }

        public decimal ToplamYurtDisiHarcamalar { get; set; }

        [StringLength(9)]
        public string ToplamYurtDisiHarcamalarDovizi { get; set; }
        public decimal ToplamYurtIciHarcamalar { get; set; }

        [StringLength(9)]
        public string VarisGumrukIdaresi { get; set; }

        public int YukBelgeleriSayisi { get; set; }

        [StringLength(40)]
        public string YuklemeBosaltmaYeri { get; set; }

        public DateTime? OlsuturulmaTarihi { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }
    public class DbBeyanModelValidator : AbstractValidator<DbBeyan>
    {
        public DbBeyanModelValidator()
        {

            RuleFor(x => x.Gumruk)
            .NotNull().WithMessage("Gümrük Kodu Boş olamaz").MaximumLength(10).WithMessage("Gümrük Kodu 10 karakteri  geçemez");

            // RuleFor(x => x.Alici_ulke)
            //.MaximumLength(30).WithMessage("Alıcı Ülke 30 karakteri  geçemez");
            // RuleFor(x => x.Belge_no)
            //.NotNull().WithMessage("Belge No Boş olamaz").Equal("0321").WithMessage("Belge No 0321 olması gerekli")
            //.MaximumLength(30).WithMessage("Belge No 30 karakteri  geçemez");
            // RuleFor(x => x.Belge_olusturma_tarihi)
            //.NotNull().WithMessage("Belge Oluşturma Tarihi Boş olamaz");
            // RuleFor(x => x.Belge_seri_no)
            //.NotNull().WithMessage("Belge Seri No Boş olamaz").MaximumLength(10).WithMessage("Belge Seri No 10 karakteri  geçemez");
            // RuleFor(x => x.Beyanname_no)
            //.MaximumLength(20).WithMessage("Beyanname No 20 karakteri  geçemez");
            // RuleFor(x => x.Beyan_tipi)
            //.NotNull().WithMessage("Beyan Tipi Boş olamaz")
            //.MaximumLength(9).WithMessage("Beyanname Tipi 9 karakteri  geçemez")
            //.NotEqual("TIR").Unless(x => x.Tasima_belgesi_no != "").WithMessage("Taşıma Belgesi No için Tır Karne Numarası girilmesi gerekli");
            // RuleFor(x => x.Gozlemler)
            //.MaximumLength(250).WithMessage("Gozlemler 250 karakteri  geçemez");
            // RuleFor(x => x.Gumruk_kodu)
            //.NotNull().WithMessage("Gümrük Boş olamaz").MaximumLength(6).WithMessage("Gümrük 6 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_adres)
            //.NotNull().WithMessage("İhracatçı Adres Boş olamaz").MaximumLength(250).WithMessage("İhracatçı Adres 250 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_ulke)
            //.NotNull().WithMessage("İhracatçı Ülke Boş olamaz").MaximumLength(30).WithMessage("İhracatçı Ülke 30 karakteri  geçemez");
            // RuleFor(x => x.Ihrac_ulke_grup)
            //.NotNull().WithMessage("İhrac Ülke Grup Boş olamaz").MaximumLength(30).WithMessage("İhrac Ülke Grup 30 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_unvan)
            //.NotNull().WithMessage("İhracatçı Ünvanı Boş olamaz").MaximumLength(250).WithMessage("İhracatçı Ünvanı 250 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_vergi_no)
            //.NotNull().WithMessage("İhracatçı Vergi No Boş olamaz").MaximumLength(11).WithMessage("İhracatçı Vergi No 11 karakteri  geçemez");
            // RuleFor(x => x.Ikinci_nusha)
            //.MaximumLength(9).WithMessage("İkinci Nüsha 9 karakteri  geçemez")
            //.NotEqual("1").Unless(x => x.Ilk_nusha_id != "").WithMessage("İkinci Nüsha girilmesi gerekli");
            // RuleFor(x => x.Ikinci_nusha)
            //.NotEqual("3").Unless(x => x.Beyanname_no != "").WithMessage("Beyanname Numarası ve Tescil Tarihi girilmesi gerekli");
            // RuleFor(x => x.Ilk_nusha_id)
            //.MaximumLength(30).WithMessage("Ilk Nüsha ID 30 karakteri  geçemez");
            // RuleFor(x => x.Kullanici)
            //.NotNull().WithMessage("Kullanıcı Boş olamaz").MaximumLength(11).WithMessage("Kullanıcı 11 karakteri  geçemez");
            // RuleFor(x => x.Kurum)
            //.NotNull().WithMessage("Kurum Boş olamaz").MaximumLength(9).WithMessage("Kurum 9 karakteri  geçemez");
            // RuleFor(x => x.Mersis_no)
            //.MaximumLength(30).WithMessage("Mersis No 30 karakteri  geçemez");
            // RuleFor(x => x.Mesaj)
            //.MaximumLength(50).WithMessage("Mesaj 50 karakteri  geçemez");
            // RuleFor(x => x.Tasima_bilgileri)
            //.MaximumLength(250).WithMessage("Taşıma Bilgileri 250 karakteri  geçemez");
            // RuleFor(x => x.Tasima_belgesi_no)
            //.MaximumLength(30).WithMessage("Taşıma Belgesi Numarası 30 karakteri  geçemez");
            // RuleFor(x => x.Varis_ulke_grup)
            //.NotNull().WithMessage("Varış Ülke Boş olamaz").MaximumLength(30).WithMessage("Varış Ülke 30 karakteri  geçemez");
            // RuleFor(x => x._Ticari_tanimlar)
            //.NotNull().WithMessage("Ticari Tanım Boş olamaz");



        }
    }
    public class DbTeminat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

    
        [Required]
        [StringLength(9)]
        public string TeminatSekli { get; set; }

        [Required]
      
        public decimal TeminatOrani { get; set; }

       
        [StringLength(20)]
        public string GlobalTeminatNo { get; set; }

      
        public decimal BankaMektubuTutari { get; set; }

     
        public decimal NakdiTeminatTutari { get; set; }

     
        public decimal DigerTutar { get; set; }

      
        [StringLength(20)]
        public string DigerTutarReferansi { get; set; }


        
        [StringLength(100)]
        public string Aciklama { get; set; }
        public DateTime? SonIslemZamani { get; set; }

    }
    public class DbKiymetBildirim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KiymetInternalNo { get; set; }

        [Required]
        [StringLength(9)]
        public string AliciSatici { get; set; }

        [StringLength(300)]
        public string AliciSaticiAyrintilar { get; set; }

        [StringLength(9)]
        public string Edim { get; set; }

        [StringLength(9)]
        public string Emsal { get; set; }

        [StringLength(300)]
        public string FaturaTarihiSayisi { get; set; }

        [StringLength(300)]
        public string GumrukIdaresiKarari { get; set; }

        [StringLength(9)]
        public string Kisitlamalar { get; set; }

        [StringLength(300)]
        public string KisitlamalarAyrintilar { get; set; }

        [StringLength(9)]
        public string Munasebet { get; set; }

        [StringLength(9)]
        public string Royalti { get; set; }

        [StringLength(300)]
        public string RoyaltiKosullar { get; set; }

        [StringLength(9)]
        public string SaticiyaIntikal { get; set; }

        [StringLength(300)]
        public string SaticiyaIntikalKosullar { get; set; }

        [Required]
        [StringLength(300)]
        public string SehirYer { get; set; }

        [Required]
        [StringLength(300)]
        public string SozlesmeTarihiSayisi { get; set; }

        [Required]
        [StringLength(9)]
        public string Taahhutname { get; set; }

        [Required]
        [StringLength(9)]
        public string TeslimSekli { get; set; }
        public DateTime? SonIslemZamani { get; set; }


    }
    public class DbKiymetBildirimKalem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KiymetInternalNo { get; set; }

        [Required]
        public int BeyannameKalemNo { get; set; }

        public decimal DigerOdemeler { get; set; }

        [StringLength(100)]
        public string DigerOdemelerNiteligi { get; set; }

        public decimal DolayliIntikal { get; set; }

       
        public decimal DolayliOdeme { get; set; }

        public decimal GirisSonrasiNakliye { get; set; }

        public decimal IthalaKatilanMalzeme { get; set; }

        public decimal IthalaUretimAraclar { get; set; }

        public decimal IthalaUretimTuketimMalzemesi { get; set; }
        public decimal KapAmbalajBedeli { get; set; }

        [Required]
        public int KiymetKalemNo { get; set; }

        public decimal Komisyon { get; set; }
        public decimal Nakliye { get; set; }
        public decimal PlanTaslak { get; set; }
        public decimal RoyaltiLisans { get; set; }
        public decimal Sigorta { get; set; }
        public decimal TeknikYardim { get; set; }
        public decimal Tellaliye { get; set; }
        public decimal VergiHarcFon { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }

    public class DbOzetBeyanAcma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanNo { get; set; }

        [Required]
        [StringLength(9)]
        public string IslemKapsami { get; set; }

        [Required]
        [StringLength(9)]
        public string Ambar { get; set; }

        [Required]
        [StringLength(20)]
        public string BaskaRejim { get; set; }

        
        [StringLength(1500)]
        public string Aciklama { get; set; }
        public DateTime? SonIslemZamani { get; set; }
    }

    public class DbOzetBeyanAcmaTasimaSenet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

        [Required]
        [StringLength(20)]
        public string TasimaSenediNo { get; set; }

        public DateTime? SonIslemZamani { get; set; }



    }

    public class DbOzetBeyanAcmaTasimaSatir
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string OzetBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string TasimaSenetInternalNo { get; set; }

     
        [StringLength(9)]
        public string AmbarKodu { get; set; }

        [Required]
        public decimal Miktar { get; set; }

        [Required]
        [StringLength(9)]
        public string TasimaSatirNo { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }

    public class DbKalem
    {
        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string KalemInternalNo { get; set; }


        [StringLength(500)]
        public string Aciklama44 { get; set; }

        [Required]
        public decimal Adet { get; set; }



        [StringLength(9)]
        public string AlgilamaBirimi1 { get; set; }


        [StringLength(9)]
        public string AlgilamaBirimi2 { get; set; }


        [StringLength(9)]
        public string AlgilamaBirimi3 { get; set; }



        public decimal AlgilamaMiktari1 { get; set; }


        public decimal AlgilamaMiktari2 { get; set; }


        public decimal AlgilamaMiktari3 { get; set; }

        [Required]
        public decimal BrutAgirlik { get; set; }

        [Required]
        [StringLength(9)]
        public string Cins { get; set; }


        [StringLength(9)]
        public string EkKod { get; set; }


        [StringLength(9)]
        public string GirisCikisAmaci { get; set; }


        [StringLength(300)]
        public string GirisCikisAmaciAciklama { get; set; }

        [Required]
        [StringLength(12)]
        public string Gtip { get; set; }

        [Required]
        public decimal FaturaMiktari { get; set; }

        [Required]
        [StringLength(9)]
        public string FaturaMiktariDovizi { get; set; }



        [StringLength(9)]
        public string IkincilIslem { get; set; }


        [StringLength(9)]
        public string ImalatciFirmaBilgisi { get; set; }


        [StringLength(15)]
        public string ImalatciVergiNo { get; set; }

        [Required]
        public decimal IstatistikiKiymet { get; set; }

        [Required]
        public decimal IstatistikiMiktar { get; set; }


        [StringLength(9)]
        public string KalemIslemNiteligi { get; set; }

        [Required]
        public int KalemSiraNo { get; set; }


        [StringLength(9)]
        public string KullanilmisEsya { get; set; }

        [Required]
        [StringLength(70)]
        public string Marka { get; set; }


        [StringLength(9)]
        public string MahraceIade { get; set; }

        [Required]
        [StringLength(9)]
        public string MenseiUlke { get; set; }

        [Required]
        public decimal Miktar { get; set; }

        [Required]
        [StringLength(9)]
        public string MiktarBirimi { get; set; }


        [StringLength(9)]
        public string Muafiyetler1 { get; set; }


        [StringLength(9)]
        public string Muafiyetler2 { get; set; }


        [StringLength(9)]
        public string Muafiyetler3 { get; set; }


        [StringLength(9)]
        public string Muafiyetler4 { get; set; }


        [StringLength(9)]
        public string Muafiyetler5 { get; set; }


        [StringLength(500)]
        public string MuafiyetAciklamasi { get; set; }


        [Required]
        public decimal NavlunMiktari { get; set; }


        [StringLength(9)]
        public string NavlunMiktariDovizi { get; set; }


        [Required]
        public decimal NetAgirlik { get; set; }

        [Required]
        [StringLength(70)]
        public string Numara { get; set; }


        [StringLength(9)]
        public string Ozellik { get; set; }


        [StringLength(12)]
        public string ReferansTarihi { get; set; }


        [StringLength(20)]
        public string SatirNo { get; set; }

        [Required]
        public decimal SigortaMiktari { get; set; }


        [StringLength(9)]
        public string SigortaMiktariDovizi { get; set; }

        [Required]
        public decimal SinirGecisUcreti { get; set; }


        [StringLength(9)]
        public string StmIlKodu { get; set; }

        [Required]
        [StringLength(9)]
        public string TamamlayiciOlcuBirimi { get; set; }


        //[StringLength(350)]
        //public string TarifeTanimi { get; set; }

        [Required]
        [StringLength(350)]
        public string TicariTanimi { get; set; }

        [Required]
        [StringLength(9)]
        public string TeslimSekli { get; set; }


        [StringLength(9)]
        public string UluslararasiAnlasma { get; set; }

        [Required]
        public decimal YurtDisiDiger { get; set; }



        [StringLength(9)]
        public string YurtDisiDigerDovizi { get; set; }


        [StringLength(100)]
        public string YurtDisiDigerAciklama { get; set; }

        [Required]
        public decimal YurtDisiDemuraj { get; set; }



        [StringLength(9)]
        public string YurtDisiDemurajDovizi { get; set; }

        [Required]
        public decimal YurtDisiFaiz { get; set; }



        [StringLength(9)]
        public string YurtDisiFaizDovizi { get; set; }

        [Required]
        public decimal YurtDisiKomisyon { get; set; }



        [StringLength(9)]
        public string YurtDisiKomisyonDovizi { get; set; }

        [Required]
        public decimal YurtDisiRoyalti { get; set; }



        [StringLength(9)]
        public string YurtDisiRoyaltiDovizi { get; set; }


        [Required]
        public decimal YurtIciBanka { get; set; }

        [Required]
        public decimal YurtIciCevre { get; set; }

        [Required]
        public decimal YurtIciDiger { get; set; }


        [StringLength(100)]
        public string YurtIciDigerAciklama { get; set; }

        [Required]
        public decimal YurtIciDepolama { get; set; }

        [Required]
        public decimal YurtIciKkdf { get; set; }

        [Required]
        public decimal YurtIciKultur { get; set; }

        [Required]
        public decimal YurtIciLiman { get; set; }

        [Required]
        public decimal YurtIciTahliye { get; set; }

        public DateTime? SonIslemZamani { get; set; }

       


    }
    public class DbKalemModelValidator : AbstractValidator<DbKalem>
    {
        public DbKalemModelValidator()
        {

            RuleFor(x => x.Gtip)
             .NotNull().WithMessage("Gtip Boş olamaz").MaximumLength(15).WithMessage("Gtip 12 karakteri  geçemez");

            // RuleFor(x => x.Alici_ulke)
            //.MaximumLength(30).WithMessage("Alıcı Ülke 30 karakteri  geçemez");
            // RuleFor(x => x.Belge_no)
            //.NotNull().WithMessage("Belge No Boş olamaz").Equal("0321").WithMessage("Belge No 0321 olması gerekli")
            //.MaximumLength(30).WithMessage("Belge No 30 karakteri  geçemez");
            // RuleFor(x => x.Belge_olusturma_tarihi)
            //.NotNull().WithMessage("Belge Oluşturma Tarihi Boş olamaz");
            // RuleFor(x => x.Belge_seri_no)
            //.NotNull().WithMessage("Belge Seri No Boş olamaz").MaximumLength(10).WithMessage("Belge Seri No 10 karakteri  geçemez");
            // RuleFor(x => x.Beyanname_no)
            //.MaximumLength(20).WithMessage("Beyanname No 20 karakteri  geçemez");
            // RuleFor(x => x.Beyan_tipi)
            //.NotNull().WithMessage("Beyan Tipi Boş olamaz")
            //.MaximumLength(9).WithMessage("Beyanname Tipi 9 karakteri  geçemez")
            //.NotEqual("TIR").Unless(x => x.Tasima_belgesi_no != "").WithMessage("Taşıma Belgesi No için Tır Karne Numarası girilmesi gerekli");
            // RuleFor(x => x.Gozlemler)
            //.MaximumLength(250).WithMessage("Gozlemler 250 karakteri  geçemez");
            // RuleFor(x => x.Gumruk_kodu)
            //.NotNull().WithMessage("Gümrük Boş olamaz").MaximumLength(6).WithMessage("Gümrük 6 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_adres)
            //.NotNull().WithMessage("İhracatçı Adres Boş olamaz").MaximumLength(250).WithMessage("İhracatçı Adres 250 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_ulke)
            //.NotNull().WithMessage("İhracatçı Ülke Boş olamaz").MaximumLength(30).WithMessage("İhracatçı Ülke 30 karakteri  geçemez");
            // RuleFor(x => x.Ihrac_ulke_grup)
            //.NotNull().WithMessage("İhrac Ülke Grup Boş olamaz").MaximumLength(30).WithMessage("İhrac Ülke Grup 30 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_unvan)
            //.NotNull().WithMessage("İhracatçı Ünvanı Boş olamaz").MaximumLength(250).WithMessage("İhracatçı Ünvanı 250 karakteri  geçemez");
            // RuleFor(x => x.Ihracatci_vergi_no)
            //.NotNull().WithMessage("İhracatçı Vergi No Boş olamaz").MaximumLength(11).WithMessage("İhracatçı Vergi No 11 karakteri  geçemez");
            // RuleFor(x => x.Ikinci_nusha)
            //.MaximumLength(9).WithMessage("İkinci Nüsha 9 karakteri  geçemez")
            //.NotEqual("1").Unless(x => x.Ilk_nusha_id != "").WithMessage("İkinci Nüsha girilmesi gerekli");
            // RuleFor(x => x.Ikinci_nusha)
            //.NotEqual("3").Unless(x => x.Beyanname_no != "").WithMessage("Beyanname Numarası ve Tescil Tarihi girilmesi gerekli");
            // RuleFor(x => x.Ilk_nusha_id)
            //.MaximumLength(30).WithMessage("Ilk Nüsha ID 30 karakteri  geçemez");
            // RuleFor(x => x.Kullanici)
            //.NotNull().WithMessage("Kullanıcı Boş olamaz").MaximumLength(11).WithMessage("Kullanıcı 11 karakteri  geçemez");
            // RuleFor(x => x.Kurum)
            //.NotNull().WithMessage("Kurum Boş olamaz").MaximumLength(9).WithMessage("Kurum 9 karakteri  geçemez");
            // RuleFor(x => x.Mersis_no)
            //.MaximumLength(30).WithMessage("Mersis No 30 karakteri  geçemez");
            // RuleFor(x => x.Mesaj)
            //.MaximumLength(50).WithMessage("Mesaj 50 karakteri  geçemez");
            // RuleFor(x => x.Tasima_bilgileri)
            //.MaximumLength(250).WithMessage("Taşıma Bilgileri 250 karakteri  geçemez");
            // RuleFor(x => x.Tasima_belgesi_no)
            //.MaximumLength(30).WithMessage("Taşıma Belgesi Numarası 30 karakteri  geçemez");
            // RuleFor(x => x.Varis_ulke_grup)
            //.NotNull().WithMessage("Varış Ülke Boş olamaz").MaximumLength(30).WithMessage("Varış Ülke 30 karakteri  geçemez");
            // RuleFor(x => x._Ticari_tanimlar)
            //.NotNull().WithMessage("Ticari Tanım Boş olamaz");



        }
    }
    public class DbOdemeSekli
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        [StringLength(2)]
        public string OdemeSekliKodu { get; set; }

        [Required]
    
        public decimal OdemeTutari { get; set; }

        [Required]
        [StringLength(30)]
        public string TBFID { get; set; }

        public DateTime? SonIslemZamani { get; set; }


    }
    public class DbFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }


        [Required]
        [StringLength(150)]
        public string AdUnvan { get; set; }



        [Required]
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

      
        [StringLength(15)]
        public string Faks { get; set; }

        [Required]
        [StringLength(35)]
        public string IlIlce { get; set; }

        [Required]
        [StringLength(9)]
        public string KimlikTuru { get; set; }

        [Required]
        [StringLength(20)]
        public string No { get; set; }


        [StringLength(10)]
        public string PostaKodu { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefon { get; set; }

        [Required]
        [StringLength(15)]
        public string Tip { get; set; }

        [Required]
        [StringLength(9)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }

    }
    public class DbMarka
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        [StringLength(500)]
        public string MarkaAdi { get; set; }

        [Required]
        public decimal MarkaKiymeti { get; set; }

        
        [StringLength(20)]
        public string MarkaTescilNo { get; set; }

        [Required]
        [StringLength(9)]
        public string MarkaTuru { get; set; }
      
      
        [StringLength(30)]
        public string Model { get; set; }

       
        public int MotorGucu { get; set; }

        [StringLength(30)]
        public string MotorHacmi { get; set; }


        [StringLength(30)]
        public string MotorNo { get; set; }

        [StringLength(20)]
        public string MotorTipi { get; set; }

        [StringLength(30)]
        public string ModelYili { get; set; }
       
     
        [StringLength(30)]
        public string Renk { get; set; }

        
        [StringLength(100)]
        public string ReferansNo { get; set; }
                   

        public int SilindirAdet { get; set; }

        [StringLength(20)]
        public string Vites { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class DbKonteyner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        [StringLength(9)]
        public string UlkeKodu { get; set; }
       

        [Required]
        [StringLength(35)]
        public string KonteynerNo { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class DbTamamlayiciBilgi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        [StringLength(12)]
        public string Gtip { get; set; }


        [Required]
        [StringLength(20)]
        public string Bilgi { get; set; }

        [Required]
        [StringLength(9)]
        public string Oran { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class DbBeyannameAcma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }


        [Required]
        [StringLength(20)]
        public string BeyannameNo { get; set; }

        [Required]     
        public int KalemNo { get; set; }

        [Required]     
        public decimal Miktar { get; set; }

      
        [StringLength(100)]
        public string Aciklama { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class DbBelge
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        public int KalemNo { get; set; }

        [Required]
        [StringLength(10)]
        public string BelgeKodu { get; set; }


        [StringLength(1000)]
        public string BelgeAciklamasi { get; set; }

        [Required]
        [StringLength(10)]
        public string Dogrulama { get; set; }


        [StringLength(30)]
        public string Referans { get; set; }

        [Required]
        [StringLength(12)]
        public string BelgeTarihi { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class DbVergi
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        public int KalemNo { get; set; }

        [Required]
        public int VergiKodu { get; set; }


        [StringLength(1000)]
        public string VergiAciklamasi { get; set; }

        [Required]
        public decimal Miktar { get; set; }

        [Required]
        [StringLength(5)]
        public string Oran { get; set; }

        [Required]
        [StringLength(3)]
        public string OdemeSekli { get; set; }

        [Required]
        public decimal Matrah { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class DbSoruCevap
    {
        [Key]
        public int ID { get; set; }


        [Required]
        [StringLength(30)]
        public string BeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [Required]
        public int KalemNo { get; set; }

        [Required]
        [StringLength(10)]
        public string SoruKodu { get; set; }


        [StringLength(10)]
        public string SoruCevap { get; set; }


        [StringLength(1000)]
        public string SoruAciklamasi { get; set; }

        [Required]
        [StringLength(10)]
        public string Tip { get; set; }

        [StringLength(int.MaxValue)]
        public string Cevaplar { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }


}
