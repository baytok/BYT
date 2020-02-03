using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT.UI.Models.Dto
{
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

       
        [StringLength(350)]
        public string TarifeTanimi { get; set; }

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

     

        //   public List<DbVergi> Vergiler { get; set; }


    }
}
