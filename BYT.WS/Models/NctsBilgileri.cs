﻿using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BYT.WS.Models
{
  
    public class CC015B
    {
        public CCxxxB Group { get; set; }      
        public TRAPRIPC1 AsilSorumlu { get; set; }
        public TRACONCO1 Gonderici { get; set; }
        public TRACONCE1 Alici { get; set; }
        public TRAAUTCONTRA VarisYetkiliGumruk { get; set; }
        public CUSOFFDEPEPT HareketGumruk { get; set; }
        public List<CUSOFFTRARNS> TransitGumruk { get; set; }
        public CUSOFFDESEST VarisGumruk { get; set; }
        public CONRESERS KontrolSonuc { get; set; }       
        public REPREP Temsilci { get; set; }
        public SEAINFSLI Muhur { get; set; }
        public List<ITI> Roto { get; set; }
        public CARTRA100 Tasiyici { get; set; }
        public TRACORSEC037 GuvenliGonderici { get; set; }
        public TRACONSEC029 GuvenliAlici { get; set; }
        public HEAHEA Beyan{ get; set; }
        public List<GUAGUA> Teminat { get; set; }
        public List<GOOITEGDS> Kalem { get; set; }
        public List<AB> BeyanAcma { get; set; }
        public List<OB> OzetBeyanAcma { get; set; }
        public BS BasitlestirilmisUsul { get; set; }
       
    }

    public class CCxxxB
    {
        public string SynIdeMES1 { get; set ; } // UNOC 
        public string MesSenMES3 { get; set; } // NTA.TR 
        public string MesRecMES6 { get; set; } // NTA.TR 
        public string SynVerNumMES2 { get; set; } // 3  ???
        public string DatOfPreMES9 { get; set; } // 120414,200509,20053 ???
        public string TimOfPreMES10 { get; set; } // 1829,1514,2040 ???
        public string IntConRefMES11 { get; set; } // 12041418291992,3043TRN2000078,3004TRN2000568 ???
        public string AckReqMES16 { get; set; } // 0 ???
        public string TesIndMES18 { get; set; } // 0 ???
        public string MesIdeMES19 { get; set; } // 12041418291942,CC015BTR ???
        public string MesTypMES20 { get; set; } // CC029B,CC015B ???
        public string ComAccRefMES21 { get; set; } // 101339640 ???

     
        //<xsd:element ref="SenIdeCodQuaMES4" minOccurs="0"/>       
        //<xsd:element ref="RecIdeCodQuaMES7" minOccurs="0"/>     
        //<xsd:element ref="RecRefMES12" minOccurs="0"/>
        //<xsd:element ref="RecRefQuaMES13" minOccurs="0"/>
        //<xsd:element ref="AppRefMES14" minOccurs="0"/>
        //<xsd:element ref="PriMES15" minOccurs="0"/>  
        //<xsd:element ref="ComAgrIdMES17" minOccurs="0"/>
        //<xsd:element ref="MesSeqNumMES22" minOccurs="0"/>
        //<xsd:element ref="FirAndLasTraMES23" minOccurs="0"/>

    }
    public class TRAPRIPC1
    {
        [StringLength(150)]
        public string NamPC17 { get; set; } //  Unvan

        [StringLength(150)]
        public string StrAndNumPC122 { get; set; } //  Adres

        [StringLength(9)]
        public string PosCodPC123 { get; set; } //  PostaKodu

        [StringLength(35)]
        public string CitPC124 { get; set; } //  İlIlçe

        [StringLength(4)]
        public string CouPC125 { get; set; } //  UlkeKodu

        [StringLength(4)]
        public string NADLNGPC { get; set; } //  LNG

        [StringLength(15)]
        public string TINPC159 { get; set; } //  Vergi No

        [StringLength(4)]
        public string HITPC126 { get; set; } //  ????


    }
    public class TRACONCO1
    {
        [StringLength(150)]
        public string NamCO17 { get; set; } //Unvan


        [StringLength(150)]
        public string StrAndNumCO122 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodCO123 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitCO124 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCO125 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string NADLNGCO { get; set; } //LNG



        [StringLength(15)]
        public string TINCO159 { get; set; } //2.Gönderici_VergiNo




    }
    public class TRACONCE1
    {
        [StringLength(150)]
        public string NamCE17 { get; set; } //Unvan


        [StringLength(150)]
        public string StrAndNumCE122 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodCE123 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitCE124 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCE125 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string NADLNGCE { get; set; } //LNG



        [StringLength(15)]
        public string TINCE159 { get; set; } //8.Alıcı_VergiNo



    }
    public class TRAAUTCONTRA
    {
        [StringLength(17)]
        public string TINTRA59 { get; set; } //VarisGum_Yetkili

    }
    public class CUSOFFDEPEPT
    {
        [StringLength(9)]
        public string RefNumEPT1 { get; set; } //Gumruk,TR341300

    }
    public class CUSOFFTRARNS
    {
        [StringLength(9)]
        public string RefNumRNS1 { get; set; } //Ref No, BG001015
        [StringLength(9)]
        public string ArrTimTRACUS085 { get; set; } //??? 202005232359

    }
    public class CUSOFFDESEST
    {
        [StringLength(9)]
        public string RefNumEST1 { get; set; } //Gumruk,NL000205

    }
    public class CONRESERS
    {
        [StringLength(9)]
        public string ConResCodERS16 { get; set; } //
        [StringLength(9)]
        public string DatLimERS69 { get; set; } //

    }    
    public class REPREP
    {
        [StringLength(50)]
        public string NamREP5 { get; set; } // VOLKAN AKIN
        [StringLength(9)]
        public string RepCapREP18 { get; set; } //

        [StringLength(4)]
        public string RepCapREP18LNG { get; set; } // TR

    }
    public class SEAINFSLI
    {
        [StringLength(9)]
        public string SeaNumSLI2 { get; set; } // Numara

     
        public List<SEAIDSID> Muhur { get; set; } //

    }
    public class SEAIDSID
    {
        [StringLength(50)]
        public string SeaIdeSID1 { get; set; } //Muhur No

        [StringLength(4)]
        public string SeaIdeSID1LNG { get; set; } // Dil

    }
    public class ITI
    {
        [StringLength(4)]
        public string CouOfRouCodITI1 { get; set; } //
      
     

    }   
    public class CARTRA100
    {

        [StringLength(150)]
        public string NamCARTRA121 { get; set; } //Unvan


        [StringLength(150)]
        public string StrAndNumCARTRA254 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodCARTRA121 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitCARTRA789 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCodCARTRA587 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string NADCARTRA121 { get; set; } //LNG



        [StringLength(15)]
        public string TINCARTRA254 { get; set; } //Tasiyici_VergiNo

    }
    public class TRACORSEC037
    {
        [StringLength(150)]
        public string NamTRACORSEC041 { get; set; } //Unvan


        [StringLength(150)]
        public string StrNumTRACORSEC043 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodTRACORSEC042 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitTRACORSEC038 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCodTRACORSEC039 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string TRACORSEC037LNG { get; set; } //LNG



        [StringLength(15)]
        public string TINTRACORSEC044 { get; set; } //S04.GöndericiGüvenlik_VergiNo





    }
    public class TRACONSEC029
    {
        [StringLength(150)]
        public string NameTRACONSEC033 { get; set; } //Unvan


        [StringLength(150)]
        public string StrNumTRACONSEC035 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodTRACONSEC034 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitTRACONSEC030 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCodTRACONSEC031 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string TRACONSEC029LNG { get; set; } //LNG



        [StringLength(15)]
        public string TINTRACONSEC036 { get; set; } //S06.AlıcıGüvenlik_VergiNo


    }
    public class HEAHEA
    {
        [StringLength(16)]
        public string RefNumHEA4 { get; set; } //Reference number, Tescil No

     
        [StringLength(4)]
        public string TypOfDecHEA24 { get; set; } //Type of declaration, Rejim Kodu

        [StringLength(4)]
        public string CouOfDesCodHEA30 { get; set; } //Country of destination code, VarisUlkesi


        [StringLength(17)]
        public string AgrLocOfGooCodHEA38 { get; set; } //Agreed location of goods, code, EsyaKabulYer

        [StringLength(35)]
        public string AgrLocOfGooHEA39 { get; set; } //Agreed location of goods, ???

        [StringLength(3)]
        public string AgrLocOfGooHEA39LNG { get; set; } //Agreed location of goods LNG, ???

        [StringLength(17)]
        public string AutLocOfGooCodHEA41 { get; set; } //Authorised location of goods, code, EsyaOnayYer

        [StringLength(20)]
        public string PlaOfLoaCodHEA46 { get; set; } //Place of loading, code, YuklemeYer

        [StringLength(4)]
        public string CouOfDisCodHEA55 { get; set; } //Country of dispatch/export code, Çıkış Ülkesi

        [StringLength(20)]
        public string CusSubPlaHEA66 { get; set; } //Customs sub place, EsyaYeri


        public int InlTraModHEA75 { get; set; } //Inland transport mode, DahildeTasSekli

        [StringLength(4)]
        public string TraModAtBorHEA76 { get; set; } //Transport mode at border,TasimaSekli25


        [StringLength(40)]
        public string IdeOfMeaOfTraAtDHEA78 { get; set; } //Identity of means of transport at departure (exp/trans), TasitKimligi18


        [StringLength(4)]
        public string IdeOfMeaOfTraAtDHEA78LNG { get; set; } //Identity of means of transport at departure LNG, TasitKimligi18_LNG


        [StringLength(4)]
        public string NatOfMeaOfTraAtDHEA80 { get; set; } //Nationality of means of transport at departure, UlkeKodu18


        [StringLength(40)]
        public string IdeOfMeaOfTraCroHEA85 { get; set; } //Identity of means of transport crossing border, TasitKimligi21


        [StringLength(4)]
        public string IdeOfMeaOfTraCroHEA85LNG { get; set; } //Identity of means of transport crossing border LNG, TasitKimligi21_LNG



        [StringLength(4)]
        public string NatOfMeaOfTraCroHEA87 { get; set; } //Nationality of means of transport crossing border, UlkeKodu21

        [StringLength(40)]
        public string TypOfMeaOfTraCroHEA88 { get; set; } //Type of means of transport crossing border,TasimaSekli21


        public char ConIndHEA96 { get; set; } //Containerised indicator, Konteyner


        [StringLength(9)]
        public string DiaLanIndAtDepHEA254 { get; set; } //Dialog language indicator at departure, ?????

        [StringLength(4)]
        public string NCTSAccDocHEA601LNG { get; set; } //NCTS accompanying document language code, BeyanTipi_LNG


        public int TotNumOfIteHEA305 { get; set; } //Total number of items, KalemSayisi


        public int TotNumOfPacHEA306 { get; set; } //Total number of packages, ToplamKapSayisi


        public decimal TotGroMasHEA307 { get; set; } //Total gross mass, KalemToplamBrutKG


        public DateTime DecDatHEA383 { get; set; } //Declaration date, Tarih


        [StringLength(15)]
        public string DecPlaHEA394 { get; set; } //Declaration place, Yer


        [StringLength(4)]
        public string DecPlaHEA394LNG { get; set; } //Declaration place LNG, YerTarih_LNG


        [StringLength(3)]
        public string SpeCirIndHEA1 { get; set; } //Specific Circumstance Indicator, BeyanTip


        [StringLength(3)]
        public string TraChaMetOfPayHEA1 { get; set; } //Transport charges/ Method of Payment, BEY_OdemeAraci


        [StringLength(10)]
        public string ComRefNumHEA { get; set; } //Commercial Reference Number, RefaransNo
      
        public int SecHEA358 { get; set; } //Security,  GuvBeyan


        [StringLength(35)]
        public string ConRefNumHEA { get; set; } //Conveyance reference number, KonveyansRef


        [StringLength(35)]
        public string CodPlUnHEA357 { get; set; } //Place of unloading, code, BosaltmaYer


        [StringLength(4)]
        public string CodPlUnHEA357LNG { get; set; } //Place of unloading LNG, YukBosYer_LNG


        [StringLength(50)]
        public string TruckId2 { get; set; } //Place of unloading LNG, Dorse1


        [StringLength(50)]
        public string TruckId3 { get; set; } //Place of unloading LNG, Dorse2


     
        public decimal DamgaVergi { get; set; } //Place of unloading LNG, EgitimFonu


        [StringLength(15)]
        public string MusavirKimlikNo { get; set; } //Place of unloading LNG

    }  
    public class GUAGUA
    {
        [StringLength(7)]
        public string GuaTypGUA1  {get; set; } // Tur

        public List<GUAREFREF> TeminatBilgileri { get; set; }
    }
    public class GUAREFREF
    {

        [StringLength(16)]
        public string GuaRefNumGRNREF1 { get; set; } //Guarantee reference number (GRN), Global_RefNo


        [StringLength(35)]
        public string OthGuaRefREF4 { get; set; } //Other guarantee reference, Diger_RefNo

        [StringLength(4)]
        public string AccCodREF6 { get; set; } //Access code,ErisimKod

        [StringLength(4)]
        public string CurREF8 { get; set; } //Access code,DovizCins


        public decimal AmoConREF7 { get; set; }  // DigerTutar


      
        public int VALLIMECVLE { get; set; } //???? ECGecerliDegil


        [StringLength(4)]
        public string VALLIMNONECLIM { get; set; } //???? UlkeGecerliDegil



    }
    public class AB
    {
        public List<Acma2> BeyanAcma { get; set; }
    }
    public class Acma2
    {
        [StringLength(20)]
        public string Iddtext { get; set; }

        public int Nartnumart { get; set; }

        public int Nartnumart1 { get; set; }

        public decimal Qamv { get; set; }

        [StringLength(200)]
        public string Lamvecom { get; set; }
    }
    public class OB
    {
        public List<Acma> OzetBeyanAcma { get; set; }
    }
    public class Acma
    {
        [StringLength(9)]
        public string DisInd { get; set; }

        [StringLength(20)]
        public string Idosext { get; set; }

        [StringLength(9)]
        public string WareInd { get; set; }

        [StringLength(20)]
        public string Ltitref { get; set; }

        [StringLength(9)]
        public string WareCod { get; set; }

        [StringLength(9)]
        public string TitlNum { get; set; }

        public decimal DisQty { get; set; }
    }
    public class BS
    {
        [StringLength(150)]
        public string NamBeyanSahibi { get; set; }

        [StringLength(150)]
        public string StBeyanSahibi { get; set; }

        [StringLength(35)]
        public string CitBeyanSahibi { get; set; }

        [StringLength(15)]
        public string TinBeyanSahibi { get; set; }

     
    }
    public class GOOITEGDS {
      
        public int IteNumGDS7 { get; set; } //Item number, SiraNo

        [StringLength(16)]
        public string ComCodTarCodGDS10 { get; set; } //Commodity code, Gtip

        [StringLength(4)]
        public string DecTypGDS15 { get; set; } //Type of declaration, RejimKodu


        [StringLength(210)]
        public string GooDesGDS23 { get; set; } //Goods description, TICARI_TANIM


        [StringLength(4)]
        public string GooDesGDS23LNG { get; set; } //Goods description LNG, TICARI_TANIM_LNG


        public decimal GroMasGDS46 { get; set; } //Gross mass, BurutAgirlik


        public decimal NetMasGDS48 { get; set; } //Net mass, NetAgirlik

        [StringLength(4)]
        public string CouOfDisGDS58 { get; set; } //Country of dispatch/export code, CikisUlkesi


        [StringLength(9)]
        public string CouOfDesGDS59 { get; set; } //ountry of destination code,VarisUlkesi


        public char MetOfPayGDI12 { get; set; } //Transport charges/ Method of Payment, TptChMOdemeKod


        [StringLength(70)]
        public string ComRefNumGIM1 { get; set; } //Commercial Reference Number,KonsimentoRef


        [StringLength(4)]
        public string UNDanGooCodGDI1 { get; set; } //UN dangerous goods code,UNGD


        [StringLength(20)]
        public string IhrBeyanNo { get; set; } //ihracat Beyan No

        [StringLength(9)]
        public string IhrBeyanTip { get; set; } //ihracat Beyan Tipi

        [StringLength(20)]
        public string IhrBeyanParcali { get; set; } //ihracat Beyan Parçalı

        public List<PREADMREFAR2> OncekiBelgeler { get; set; }

        public List<PRODOCDC2> Belgeler { get; set; }

        public List<SPEMENMT2> EkBilgi { get; set; }

        public TRACONCO2 Gonderici { get; set; }

        public TRACONCE2 Alici { get; set; }

        public List<CONNR2> Konteyner { get; set; }

        public List<PACGS2> Kap { get; set; }

        public List<SGICODSD2> HASSASESYA { get; set; }

        public TRACORSECGOO021 GuvenliGonderici { get; set; }
        public TRACONSECGOO013 GuvenliAlici { get; set; }

      
    }
    public class PREADMREFAR2
    {

        [StringLength(6)]
        public string PreDocTypAR21 { get; set; } //OnceBelgeTip

        [StringLength(35)]
        public string PreDocRefAR26 { get; set; } //OnceBelgeRef


        [StringLength(4)]
        public string PreDocRefLNG { get; set; } //OnceBelge_LNG


        [StringLength(26)]
        public string ComOfInfAR29 { get; set; } //TamamlayiciBilgi


        [StringLength(4)]
        public string ComOfInfAR29LNG { get; set; } //TamamlayiciBilgi_LNG



    }
    public class PRODOCDC2
    {

        [StringLength(4)]
        public string DocTypDC21 { get; set; } //BelgeTip


        [StringLength(70)]
        public string DocRefDC23 { get; set; } //Belge_Ref



        [StringLength(4)]
        public string DocRefDCLNG { get; set; } //Belge_LNG



        [StringLength(26)]
        public string ComOfInfDC25 { get; set; } //TamamlayiciOlcu



        [StringLength(4)]
        public string ComOfInfDC25LNG { get; set; } //TamamlayiciOlcu_LNG




    }
    public class SPEMENMT2
    {

        [StringLength(70)]
        public string AddInfMT21 { get; set; } //EkBilgi


        [StringLength(4)]
        public string AddInfMT21LNG { get; set; } //EkBilgi_LNG


        [StringLength(5)]
        public string AddInfCodMT23 { get; set; } //EkBilgiID


        public int ExpFroECMT24 { get; set; } //EC2Ihr


        [StringLength(4)]
        public string ExpFroCouMT25 { get; set; } //UlkeKodu2Ihr



    }
    public class TRACONCO2
    {
        [StringLength(150)]
        public string NamCO27 { get; set; } //Unvan


        [StringLength(150)]
        public string StrAndNumCO222 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodCO223 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitCO224 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCO225 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string NADLNGGTCO { get; set; } //LNG



        [StringLength(15)]
        public string TINCO259 { get; set; } //2.Gönderici_VergiNo




    }
    public class TRACONCE2
    {
        [StringLength(150)]
        public string NamCE27 { get; set; } //Unvan


        [StringLength(150)]
        public string StrAndNumCE222 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodCE223 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitCE224 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCE225 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string NADLNGGICE { get; set; } //LNG



        [StringLength(15)]
        public string TINCE259 { get; set; } //8.Alıcı_VergiNo



    }
    public class CONNR2
    {

        [StringLength(50)]
        public string ConNumNR21 { get; set; } //KonteynerNo


        [StringLength(4)]
        public string KonteynerUlke{ get; set; } //EkBilgi_LNG


    }
    public class PACGS2
    {

        [StringLength(42)]
        public string MarNumOfPacGS21 { get; set; } //KapMarkaNo


        [StringLength(4)]
        public string MarNumOfPacGS21LNG { get; set; } //KapMarkaNo_LNG


        [StringLength(3)]
        public string KinOfPacGS23 { get; set; } //KapTipi


        public int NumOfPacGS24 { get; set; } //KapAdet

      
        public int NumOfPieGS25 { get; set; } //KalemSayisi


    }
    public class SGICODSD2
    {

        public int SenGooCodSD22 { get; set; } //HassasKod

        public decimal SenQuaSD23 { get; set; } //HassasMiktar





    }
    public class TRACORSECGOO021
    {
        [StringLength(150)]
        public string NamTRACORSECGOO025 { get; set; } //Unvan


        [StringLength(150)]
        public string StrNumTRACORSECGOO027 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodTRACORSECGOO026 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CitTRACORSECGOO022 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCodTRACORSECGOO023 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string TRACORSECGOO021LNG { get; set; } //LNG



        [StringLength(15)]
        public string TINTRACORSECGOO028 { get; set; } //2.GöndericiGüvenlik_VergiNo




    }
    public class TRACONSECGOO013
    {
        [StringLength(150)]
        public string NamTRACONSECGOO017 { get; set; } //Unvan


        [StringLength(150)]
        public string StrNumTRACONSECGOO019 { get; set; } //Adres



        [StringLength(9)]
        public string PosCodTRACONSECGOO018 { get; set; } //PostaKodu


        [StringLength(35)]
        public string CityTRACONSECGOO014 { get; set; } //IlIlce



        [StringLength(4)]
        public string CouCodTRACONSECGOO015 { get; set; } //UlkeKodu



        [StringLength(4)]
        public string TRACONSECGOO013LNG { get; set; } //LNG



        [StringLength(15)]
        public string TINTRACONSECGOO020 { get; set; } //S06.AlıcıGüvenlik_VergiNo




    }

    public class NbBeyan
    {
        [StringLength(30)]
        public string RefNo { get; set; }

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string NctsBeyanInternalNo { get; set; }


        [StringLength(20)]
        public string? BeyannameNo { get; set; } //Reference number, RefNumHEA4

        [StringLength(50)]
        public string TescilStatu { get; set; }

        public DateTime? TescilTarihi { get; set; } //DecDatHEA383
        public DateTime? OlsuturulmaTarihi { get; set; }

        public DateTime? SonIslemZamani { get; set; }

        [Required]
        [StringLength(9)]
        public string HareketGumruk { get; set; } // CUSOFFDEPEPT 


        [Required]
        [StringLength(9)]
        public string VarisGumruk { get; set; } // CUSOFFDESEST 

        [StringLength(17)]
        public string VarisGumrukYetkilisi { get; set; } // TINTRA59

        [Required]
        [StringLength(9)]
        public string SinirGumruk { get; set; } // ?? 


        [StringLength(4)]
        [Required]
        public string VarisUlke { get; set; } //Country of destination code, CouOfDesCodHEA30

        [StringLength(4)]
        [Required]
        public string CikisUlke { get; set; } //Country of dispatch/export code,CouOfDisCodHEA55 

        //[StringLength(9)]
        //public string  EsyaKabulYerKod{ get; set; } //Agreed location of goods, AgrLocOfGooHEA39 ???

        // [StringLength(4)]
        //public string EsyaKabulYerKodDil { get; set; } //Agreed location of goods LNG,AgrLocOfGooHEA39LNG ???


        [StringLength(17)]
        public string EsyaKabulYer { get; set; } //Agreed location of goods, code,AgrLocOfGooCodHEA38 

        [StringLength(35)]
        public string BosaltmaYer { get; set; } //Place of unloading, code, CodPlUnHEA357


        [StringLength(4)]
        public string YukBosYerDil { get; set; } //Place of unloading LNG, CodPlUnHEA357LNG

     

        [StringLength(17)]
        public string EsyaOnayYer  { get; set; } //Authorised location of goods, code, AutLocOfGooCodHEA41

        [StringLength(20)]
        public string YuklemeYeri { get; set; } //Place of loading, code,PlaOfLoaCodHEA46 
     

        [StringLength(20)]
        public string EsyaYer  { get; set; } //Customs sub place, CusSubPlaHEA66

        [StringLength(4)]
        public string DahildeTasimaSekli  { get; set; } //Inland transport mode, InlTraModHEA75

        [StringLength(4)]
        public string CikisTasimaSekli { get; set; } //Transport mode at border,TraModAtBorHEA76 


        [StringLength(40)]
        public string CikisTasitKimligi { get; set; } //Identity of means of transport at departure (exp/trans), IdeOfMeaOfTraAtDHEA78


        [StringLength(4)]
        public string CikisTasitKimligiDil { get; set; } //Identity of means of transport at departure LNG, IdeOfMeaOfTraAtDHEA78LNG


        [StringLength(4)]
        public string CikisTasitUlke { get; set; } //Nationality of means of transport at departure, NatOfMeaOfTraAtDHEA80


        [StringLength(40)]
        public string SinirTasitKimligi  { get; set; } //Identity of means of transport crossing border, IdeOfMeaOfTraCroHEA85


        [StringLength(4)]
        public string SinirTasitKimligiDil  { get; set; } //Identity of means of transport crossing border LNG, IdeOfMeaOfTraCroHEA85LNG

        public bool Tanker { get; set; }

        [StringLength(4)]
        public string SinirTasitUlke  { get; set; } //Nationality of means of transport crossing border, NatOfMeaOfTraCroHEA87

        [StringLength(40)]
        public string SinirTasimaSekli  { get; set; } //Type of means of transport crossing border,TypOfMeaOfTraCroHEA88


        public bool Konteyner { get; set; } //Containerised indicator,ConIndHEA96 


        //[StringLength(9)]
        //public string DiaLanIndAtDepHEA254 { get; set; } //Dialog language indicator at departure, ?????



        public int KalemSayisi  { get; set; } //Total number of items, TotNumOfIteHEA305


        public int ToplamKapSayisi  { get; set; } //Total number of packages, TotNumOfPacHEA306


        public decimal KalemToplamBrutKG  { get; set; } //Total gross mass, TotGroMasHEA307       


        [StringLength(4)]
        [Required]
        public string Rejim { get; set; } //Type of declaration,TypOfDecHEA24 

        [StringLength(4)]
        [Required]
        public string BeyanTipi  { get; set; } //Specific Circumstance Indicator, SpeCirIndHEA1


        [StringLength(4)]
        [Required]
        public string BeyanTipiDil { get; set; } //NCTS accompanying document language code, NCTSAccDocHEA601LNG


        [StringLength(4)]
        public string OdemeAraci  { get; set; } //Transport charges/ Method of Payment, TraChaMetOfPayHEA1


        [StringLength(10)]
        public string RefaransNo  { get; set; } //Commercial Reference Number, ComRefNumHEA

        public int GuvenliBeyan  { get; set; } //Security,  SecHEA358 //1


        [StringLength(35)]
        public string KonveyansRefNo  { get; set; } //Conveyance reference number, ConRefNumHEA
     

        [StringLength(50)]
        public string Dorse1  { get; set; } //Place of unloading LNG, TruckId2


        [StringLength(50)]
        public string Dorse2  { get; set; } //Place of unloading LNG, TruckId3


        public decimal DamgaVergi { get; set; } //Place of unloading LNG, EgitimFonu


        [StringLength(15)]
        public string MusavirKimlikNo { get; set; }

        [StringLength(15)]
        public string Kullanici { get; set; }

        

        [StringLength(15)]
        [Required]
        public string Yer { get; set; } //Declaration place, DecPlaHEA394

        [StringLength(4)]
        [Required]
        public string YerTarihDil { get; set; } //Declaration place LNG, DecPlaHEA394LNG

        [StringLength(50)]
        [Required]
        public string Temsilci { get; set; } // NamREP5

        [StringLength(9)]
        [Required]
        public string TemsilKapasite { get; set; } //RepCapREP18


        [StringLength(4)]       
        public string TemsilKapasiteDil { get; set; } // TR,RepCapREP18LNG  



        [StringLength(9)]
        public string KontrolSonuc { get; set; } // ConResCodERS16

        [StringLength(12)]
        public string SureSinir { get; set; } // DatLimERS69

    }
    public class NbBeyanSahibi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


   
        [StringLength(150)]
        public string AdUnvan { get; set; }


        
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

       
     
        [StringLength(35)]
        public string IlIlce { get; set; }



        [StringLength(15)]
        public string No { get; set; }

      

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbGondericiFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


        [Required]
        [StringLength(150)]
        public string AdUnvan { get; set; }


        [Required]
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

        [Required]
        [StringLength(4)]
        public string Dil { get; set; }

        [Required]
        [StringLength(35)]
        public string IlIlce { get; set; }


        [Required]
        [StringLength(15)]
        public string No { get; set; }

        [Required]
        [StringLength(10)]
        public string PostaKodu { get; set; }

      
        [Required]
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbAliciFirma
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


        [Required]
        [StringLength(150)]
        public string AdUnvan { get; set; }


        [Required]
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

        [Required]
        [StringLength(4)]
        public string Dil { get; set; }

        [Required]
        [StringLength(35)]
        public string IlIlce { get; set; }

        [Required]
        [StringLength(15)]
        public string No { get; set; }

        [Required]
        [StringLength(10)]
        public string PostaKodu { get; set; }


        [Required]
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbAsilSorumluFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


        [Required]
        [StringLength(150)]
        public string AdUnvan { get; set; }


        [Required]
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

        [Required]
        [StringLength(4)]
        public string Dil { get; set; }

        [Required]
        [StringLength(35)]
        public string IlIlce { get; set; }


        [Required]
        [StringLength(15)]
        public string No { get; set; }

        [Required]
        [StringLength(10)]
        public string PostaKodu { get; set; }


        [Required]
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbTasiyiciFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


      
        [StringLength(150)]
        public string AdUnvan { get; set; }


     
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

       
        [StringLength(4)]
        public string Dil { get; set; }

     
        [StringLength(35)]
        public string IlIlce { get; set; }

        [StringLength(15)]
        public string No { get; set; }

      
        [StringLength(10)]
        public string PostaKodu { get; set; }


       
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbGuvenliGondericiFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


        [StringLength(150)]
        public string AdUnvan { get; set; }


      
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

       
        [StringLength(4)]
        public string Dil { get; set; }

     
        [StringLength(35)]
        public string IlIlce { get; set; }

        [StringLength(15)]
        public string No { get; set; }

      
        [StringLength(10)]
        public string PostaKodu { get; set; }


      
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbGuvenliAliciFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


        
        [StringLength(150)]
        public string AdUnvan { get; set; }


       
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

      
        [StringLength(4)]
        public string Dil { get; set; }

       
        [StringLength(35)]
        public string IlIlce { get; set; }

        [StringLength(15)]
        public string No { get; set; }

      
        [StringLength(10)]
        public string PostaKodu { get; set; }


       
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbRota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]

        public string NctsBeyanInternalNo { get; set; }

       
        [StringLength(4)]
        [Required]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbMuhur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]

        public string NctsBeyanInternalNo { get; set; }


        [StringLength(50)]
        [Required]
        public string MuhurNo { get; set; }

        [StringLength(4)]
        [Required]
        public string Dil { get; set; }


        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbTransitGumruk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }


        [StringLength(9)]
        [Required]
        public string Gumruk { get; set; }

              
        [StringLength(20)]
        [Required]
        public string VarisTarihi { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbTeminat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]

        public string NctsBeyanInternalNo { get; set; }

        [StringLength(4)]
        [Required]
        public string TeminatTipi { get; set; }

        [StringLength(16)]
        [Required]
        public string GRNNo { get; set; } 


        [StringLength(35)]     
        public string DigerRefNo { get; set; } 

        [StringLength(4)]
        public string ErisimKodu { get; set; } //

        [StringLength(4)]
        [Required]
        public string DovizCinsi { get; set; }

        [Required]
        public decimal Tutar { get; set; }  


        public int ECGecerliDegil { get; set; } 


        [StringLength(4)]
        public string UlkeGecerliDegil { get; set; } 

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbKalem
    {
        [Required]
        [StringLength(30)]

        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string KalemInternalNo { get; set; }

        public int KalemSiraNo { get; set; } //IteNumGDS7 

        [StringLength(16)]
        [Required]
        public string Gtip { get; set; } //Commodity code, ComCodTarCodGDS10 

        [StringLength(4)]
        public string RejimKodu { get; set; } //Type of declaration, DecTypGDS15


        [StringLength(210)]
        public string TicariTanim { get; set; } //Goods description,GooDesGDS23 


        [StringLength(4)]
        public string TicariTanimDil { get; set; } //Goods description LNG, GooDesGDS23LNG

        public decimal Kiymet { get; set; }

        [StringLength(4)]
        public string KiymetDoviz { get; set; }

        [Required]
        public decimal BurutAgirlik { get; set; } //Gross mass, GroMasGDS46


        public decimal NetAgirlik { get; set; } //Net mass, NetMasGDS48

        [StringLength(4)]
        public string CikisUlkesi { get; set; } //Country of dispatch/export code, CouOfDisGDS58


        [StringLength(4)]
        public string VarisUlkesi { get; set; } //ountry of destination code,CouOfDesGDS59


        [StringLength(4)]
        public string TptChMOdemeKod { get; set; } //Transport charges/ Method of Payment, MetOfPayGDI12


        [StringLength(70)]
        public string KonsimentoRef { get; set; } //Commercial Reference Number,ComRefNumGIM1


        [StringLength(4)]
        public string UNDG { get; set; } //UN dangerous goods code,UNDanGooCodGDI1


        [StringLength(20)]
        public string IhrBeyanNo { get; set; } //ihracat Beyan No

        [StringLength(9)]
        public string IhrBeyanTip { get; set; } //ihracat Beyan Tipi

        [StringLength(9)]
        public string IhrBeyanParcali { get; set; } //ihracat Beyan Parçalı
        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbKalemGondericiFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

       
        [StringLength(150)]
        public string AdUnvan { get; set; }


       
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

       
        [StringLength(4)]
        public string Dil { get; set; }

     
        [StringLength(35)]
        public string IlIlce { get; set; }


      
        [StringLength(15)]
        public string No { get; set; }

       
        [StringLength(10)]
        public string PostaKodu { get; set; }


      
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbKalemAliciFirma
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

    
        [StringLength(150)]
        public string AdUnvan { get; set; }


      
        [StringLength(150)]
        public string CaddeSokakNo { get; set; }

      
        [StringLength(4)]
        public string Dil { get; set; }

        
        [StringLength(35)]
        public string IlIlce { get; set; }

     
        [StringLength(15)]
        public string No { get; set; }

      
        [StringLength(10)]
        public string PostaKodu { get; set; }


      
        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbKalemGuvenliGondericiFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [StringLength(150)]
        public string AdUnvan { get; set; }



        [StringLength(150)]
        public string CaddeSokakNo { get; set; }


        [StringLength(4)]
        public string Dil { get; set; }


        [StringLength(35)]
        public string IlIlce { get; set; }

        [StringLength(15)]
        public string No { get; set; }


        [StringLength(10)]
        public string PostaKodu { get; set; }



        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbKalemGuvenliAliciFirma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [StringLength(150)]
        public string AdUnvan { get; set; }



        [StringLength(150)]
        public string CaddeSokakNo { get; set; }


        [StringLength(4)]
        public string Dil { get; set; }


        [StringLength(35)]
        public string IlIlce { get; set; }

        [StringLength(15)]
        public string No { get; set; }


        [StringLength(10)]
        public string PostaKodu { get; set; }



        [StringLength(4)]
        public string UlkeKodu { get; set; }

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbKonteyner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [StringLength(50)]
        [Required]
        public string KonteynerNo { get; set; } //KonteynerNo


        [StringLength(4)]
        [Required]
        public string Dil { get; set; } //EkBilgi_LNG

        public DateTime? SonIslemZamani { get; set; }
    }

    public class NbOncekiBelgeler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }


        [StringLength(6)]
        public string BelgeTipi  { get; set; } //OnceBelgeTip,PreDocTypAR21

        [StringLength(35)]
        public string RefNo { get; set; } //OnceBelgeRef,PreDocRefAR26


        [StringLength(4)]
        public string BelgeDil { get; set; } //OnceBelge_LNG,PreDocRefLNG


        [StringLength(26)]
        public string TamamlayiciBilgi { get; set; } //TamamlayiciBilgi,ComOfInfAR29


        [StringLength(4)]
        public string  TamamlayiciBilgiDil{ get; set; } //TamamlayiciBilgi_LNG,ComOfInfAR29LNG

    public DateTime? SonIslemZamani { get; set; }
    }

    public class NbBelgeler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [StringLength(4)]
        public string BelgeTipi { get; set; } //BelgeTip,DocTypDC21


        [StringLength(70)]
        public string RefNo { get; set; } //Belge_Ref,DocRefDC23


        [StringLength(4)]
        public string BelgeDil { get; set; } //Belge_LNG,DocRefDCLNG


        [StringLength(26)]
        public string TamamlayiciOlcu { get; set; } //TamamlayiciOlcu,ComOfInfDC25


        [StringLength(4)]
        public string TamamlayiciOlcuDil { get; set; } //TamamlayiciOlcu_LNG,ComOfInfDC25LNG

        public DateTime? SonIslemZamani { get; set; }
    }

    public class NbEkBilgi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [StringLength(70)]
        public string EkBilgi { get; set; } //EkBilgi,AddInfMT21


        [StringLength(4)]
        public string Dil { get; set; } //EkBilgi_LNG,AddInfMT21LNG


        [StringLength(5)]
        public string EkBilgiId { get; set; } //EkBilgiID,AddInfCodMT23


        public int Ec2Ihr { get; set; } //EC2Ihr,ExpFroECMT24


        [StringLength(4)]
        public string UlkeKodu { get; set; } //UlkeKodu2Ihr,ExpFroCouMT25

        public DateTime? SonIslemZamani { get; set; }
    }

    public class NbKap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        [StringLength(42)]
        public string MarkaNo { get; set; } //KapMarkaNo,MarNumOfPacGS21


        [StringLength(4)]
        public string  MarkaDil { get; set; } //KapMarkaNo_LNG,MarNumOfPacGS21LNG


        [StringLength(3)]
        public string KapTipi { get; set; } //KapTipi,KinOfPacGS23


        public int  KapAdet{ get; set; } //KapAdet,NumOfPacGS24

        public int KalemSayisi  { get; set; } //KalemSayisi,NumOfPieGS25

        public DateTime? SonIslemZamani { get; set; }
    }

    public class NbHassasEsya
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [Required]
        [StringLength(30)]
        public string KalemInternalNo { get; set; }

        public int Kod { get; set; } //HassasKod,SenGooCodSD22

        public decimal Miktar { get; set; } //HassasMiktar,SenQuaSD23

    public DateTime? SonIslemZamani { get; set; }
    }

    public class NbObAcma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [StringLength(9)]
        public string IslemKapsami { get; set; } //DisInd

        [StringLength(20)]
        public string OzetBeyanNo { get; set; } //Idosext

        [StringLength(9)]
        public string AmbarIci { get; set; } //WareInd

        [StringLength(20)]
        public string TasimaSenetNo { get; set; } // Ltitref

        [StringLength(9)]
        public string AmbarKodu { get; set; } //WareCod

        [StringLength(9)]
        public string TitlNum { get; set; } // TitlNum

        public decimal Miktar { get; set; } // DisQty

        public DateTime? SonIslemZamani { get; set; }
    }
    public class NbAbAcma
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string NctsBeyanInternalNo { get; set; }

        [StringLength(20)]
        public string  BeyannameNo { get; set; } // Iddtext

        public int AcilanKalemNo { get; set; } // Nartnumart 

        public int KalemNo { get; set; } // Nartnumart1 

        public decimal Miktar { get; set; } // Qamv

        [StringLength(200)]
        public string Aciklama { get; set; } // Lamvecom

        public DateTime? SonIslemZamani { get; set; }
    }

}
