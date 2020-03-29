USE [master]
GO
/****** Object:  Database [BYTDb]    Script Date: 24.03.2020 22:05:24 ******/
CREATE DATABASE [BYTDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BYTDb', FILENAME = N'C:\Projeler\VeriTabanlari\BYTDb\BYTDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BYTDb_log', FILENAME = N'C:\Projeler\VeriTabanlari\BYTDb\BYTDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [BYTDb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BYTDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BYTDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BYTDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BYTDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BYTDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BYTDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [BYTDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BYTDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BYTDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BYTDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BYTDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BYTDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BYTDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BYTDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BYTDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BYTDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BYTDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BYTDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BYTDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BYTDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BYTDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BYTDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BYTDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BYTDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BYTDb] SET  MULTI_USER 
GO
ALTER DATABASE [BYTDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BYTDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BYTDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BYTDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BYTDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BYTDb', N'ON'
GO
ALTER DATABASE [BYTDb] SET QUERY_STORE = OFF
GO
USE [BYTDb]
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[1000RefId1000]    Script Date: 24.03.2020 22:05:24 ******/
CREATE SEQUENCE [dbo].[1000RefId1000] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefId1000]    Script Date: 24.03.2020 22:05:24 ******/
CREATE SEQUENCE [dbo].[RefId1000] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
/****** Object:  Table [dbo].[DbBeyan]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbBeyan](
	[RefNo] [varchar](30) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[BeyannameNo] [varchar](20) NULL,
	[Aciklamalar] [varchar](350) NULL,
	[AliciSaticiIliskisi] [varchar](9) NULL,
	[AliciVergiNo] [varchar](20) NULL,
	[AntrepoKodu] [varchar](9) NULL,
	[AsilSorumluVergiNo] [varchar](20) NULL,
	[BasitlestirilmisUsul] [varchar](9) NULL,
	[BankaKodu] [varchar](16) NULL,
	[BeyanSahibiVergiNo] [varchar](20) NULL,
	[BirlikKriptoNumarasi] [varchar](30) NULL,
	[BirlikKayitNumarasi] [varchar](30) NULL,
	[CikisUlkesi] [varchar](9) NULL,
	[CikistakiAracinKimligi] [varchar](35) NULL,
	[CikistakiAracinTipi] [varchar](9) NULL,
	[CikistakiAracinUlkesi] [varchar](9) NULL,
	[EsyaninBulunduguYer] [varchar](40) NULL,
	[GidecegiSevkUlkesi] [varchar](9) NULL,
	[GidecegiUlke] [varchar](9) NULL,
	[GirisGumrukIdaresi] [varchar](9) NULL,
	[GondericiVergiNo] [varchar](20) NULL,
	[Gumruk] [varchar](20) NULL,
	[IsleminNiteligi] [varchar](9) NULL,
	[KapAdedi] [int] NULL,
	[Konteyner] [varchar](9) NULL,
	[Kullanici] [varchar](15) NOT NULL,
	[LimanKodu] [varchar](9) NULL,
	[Mail1] [varchar](50) NULL,
	[Mail2] [varchar](50) NULL,
	[Mail3] [varchar](50) NULL,
	[Mobil1] [varchar](30) NULL,
	[Mobil2] [varchar](30) NULL,
	[MusavirVergiNo] [varchar](20) NULL,
	[OdemeAraci] [varchar](9) NULL,
	[MusavirReferansNo] [varchar](12) NULL,
	[ReferansTarihi] [varchar](12) NULL,
	[Rejim] [varchar](9) NOT NULL,
	[SinirdakiAracinKimligi] [varchar](35) NULL,
	[SinirdakiAracinTipi] [varchar](9) NULL,
	[SinirdakiAracinUlkesi] [varchar](9) NULL,
	[SinirdakiTasimaSekli] [varchar](9) NULL,
	[TasarlananGuzergah] [varchar](250) NULL,
	[TelafiEdiciVergi] [decimal](18, 2) NULL,
	[TescilStatu] [varchar](50) NULL,
	[TescilTarihi] [datetime2](7) NULL,
	[TeslimSekli] [varchar](9) NULL,
	[TeslimSekliYeri] [varchar](40) NULL,
	[TicaretUlkesi] [varchar](9) NULL,
	[ToplamFatura] [decimal](18, 2) NULL,
	[ToplamFaturaDovizi] [varchar](9) NULL,
	[ToplamNavlun] [decimal](18, 2) NULL,
	[ToplamNavlunDovizi] [varchar](9) NULL,
	[ToplamSigorta] [decimal](18, 2) NULL,
	[ToplamSigortaDovizi] [varchar](9) NULL,
	[ToplamYurtDisiHarcamalar] [decimal](18, 2) NULL,
	[ToplamYurtDisiHarcamalarDovizi] [varchar](9) NULL,
	[ToplamYurtIciHarcamalar] [decimal](18, 2) NULL,
	[VarisGumrukIdaresi] [varchar](9) NULL,
	[YukBelgeleriSayisi] [int] NULL,
	[YuklemeBosaltmaYeri] [varchar](40) NULL,
PRIMARY KEY CLUSTERED 
(
	[BeyanInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbBeyannameAcma]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbBeyannameAcma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KalemInternalNo] [varchar](30) NOT NULL,
	[BeyannameNo] [varchar](20) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[Miktar] [decimal](18, 2) NOT NULL,
	[Aciklama] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbFirma]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[AdUnvan] [varchar](150) NOT NULL,
	[CaddeSokakNo] [varchar](150) NOT NULL,
	[Faks] [varchar](15) NULL,
	[IlIlce] [varchar](35) NOT NULL,
	[KimlikTuru] [varchar](9) NOT NULL,
	[No] [varchar](20) NOT NULL,
	[PostaKodu] [varchar](10) NULL,
	[Telefon] [varchar](15) NULL,
	[Tip] [varchar](15) NULL,
	[UlkeKodu] [varchar](15) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKalem]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKalem](
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KalemInternalNo] [varchar](30) NOT NULL,
	[Aciklama44] [varchar](500) NULL,
	[Adet] [decimal](18, 0) NULL,
	[AlgilamaBirimi1] [varchar](9) NULL,
	[AlgilamaBirimi2] [varchar](9) NULL,
	[AlgilamaBirimi3] [varchar](9) NULL,
	[AlgilamaMiktari1] [decimal](18, 2) NULL,
	[AlgilamaMiktari2] [decimal](18, 2) NULL,
	[AlgilamaMiktari3] [decimal](18, 2) NULL,
	[BrutAgirlik] [decimal](18, 2) NULL,
	[Cins] [varchar](9) NULL,
	[EkKod] [varchar](9) NULL,
	[GirisCikisAmaci] [varchar](9) NULL,
	[GirisCikisAmaciAciklama] [varchar](300) NULL,
	[Gtip] [varchar](15) NOT NULL,
	[FaturaMiktari] [decimal](18, 2) NOT NULL,
	[FaturaMiktariDovizi] [varchar](9) NULL,
	[IkincilIslem] [varchar](9) NULL,
	[ImalatciFirmaBilgisi] [varchar](9) NULL,
	[ImalatciVergiNo] [varchar](15) NULL,
	[IstatistikiKiymet] [decimal](18, 2) NULL,
	[IstatistikiMiktar] [decimal](18, 2) NULL,
	[KalemIslemNiteligi] [varchar](9) NULL,
	[KalemSiraNo] [int] NOT NULL,
	[KullanilmisEsya] [varchar](9) NULL,
	[Marka] [varchar](70) NULL,
	[MahraceIade] [varchar](9) NULL,
	[MenseiUlke] [varchar](9) NULL,
	[Miktar] [decimal](18, 2) NULL,
	[MiktarBirimi] [varchar](9) NULL,
	[Muafiyetler1] [varchar](9) NULL,
	[Muafiyetler2] [varchar](9) NULL,
	[Muafiyetler3] [varchar](9) NULL,
	[Muafiyetler4] [varchar](9) NULL,
	[Muafiyetler5] [varchar](9) NULL,
	[MuafiyetAciklamasi] [varchar](500) NULL,
	[NavlunMiktari] [decimal](18, 2) NULL,
	[NavlunMiktariDovizi] [varchar](9) NULL,
	[NetAgirlik] [decimal](18, 2) NOT NULL,
	[Numara] [varchar](70) NULL,
	[Ozellik] [varchar](9) NULL,
	[ReferansTarihi] [varchar](12) NULL,
	[SatirNo] [varchar](20) NULL,
	[SigortaMiktari] [decimal](18, 2) NULL,
	[SigortaMiktariDovizi] [varchar](9) NULL,
	[SinirGecisUcreti] [decimal](18, 2) NULL,
	[StmIlKodu] [varchar](9) NULL,
	[TamamlayiciOlcuBirimi] [varchar](9) NULL,
	[TarifeTanimi] [varchar](350) NULL,
	[TicariTanimi] [varchar](350) NULL,
	[TeslimSekli] [varchar](9) NULL,
	[UluslararasiAnlasma] [varchar](9) NULL,
	[YurtDisiDiger] [decimal](18, 2) NULL,
	[YurtDisiDigerDovizi] [varchar](9) NULL,
	[YurtDisiDigerAciklama] [varchar](100) NULL,
	[YurtDisiDemuraj] [decimal](18, 2) NULL,
	[YurtDisiDemurajDovizi] [varchar](9) NULL,
	[YurtDisiFaiz] [decimal](18, 2) NULL,
	[YurtDisiFaizDovizi] [varchar](9) NULL,
	[YurtDisiKomisyon] [decimal](18, 2) NULL,
	[YurtDisiKomisyonDovizi] [varchar](9) NULL,
	[YurtDisiRoyalti] [decimal](18, 2) NULL,
	[YurtDisiRoyaltiDovizi] [varchar](9) NULL,
	[YurtIciBanka] [decimal](18, 2) NULL,
	[YurtIciCevre] [decimal](18, 2) NULL,
	[YurtIciDiger] [decimal](18, 2) NULL,
	[YurtIciDigerAciklama] [varchar](100) NULL,
	[YurtIciDepolama] [decimal](18, 2) NULL,
	[YurtIciKkdf] [decimal](18, 2) NULL,
	[YurtIciKultur] [decimal](18, 2) NULL,
	[YurtIciLiman] [decimal](18, 2) NULL,
	[YurtIciTahliye] [decimal](18, 2) NULL,
	[TimeStamp] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[KalemInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKiymetBildirim]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKiymetBildirim](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KiymetInternalNo] [varchar](30) NOT NULL,
	[AliciSatici] [varchar](9) NOT NULL,
	[AliciSaticiAyrintilar] [varchar](300) NULL,
	[Edim] [varchar](9) NULL,
	[Emsal] [varchar](9) NULL,
	[FaturaTarihiSayisi] [varchar](300) NULL,
	[GumrukIdaresiKarari] [varchar](300) NULL,
	[Kisitlamalar] [varchar](9) NULL,
	[KisitlamalarAyrintilar] [varchar](300) NULL,
	[Munasebet] [varchar](9) NULL,
	[Royalti] [varchar](9) NULL,
	[RoyaltiKosullar] [varchar](300) NULL,
	[SaticiyaIntikal] [varchar](9) NULL,
	[SaticiyaIntikalKosullar] [varchar](300) NULL,
	[SehirYer] [varchar](300) NULL,
	[SozlesmeTarihiSayisi] [varchar](300) NULL,
	[Taahutname] [varchar](9) NOT NULL,
	[TeslimSekli] [varchar](9) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKiymetBildirimKalem]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKiymetBildirimKalem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KiymetInternalNo] [varchar](30) NOT NULL,
	[KiymetKalemNo] [int] NOT NULL,
	[BeyannameKalemNo] [int] NOT NULL,
	[DigerOdemeler] [decimal](18, 2) NULL,
	[DigerOdemelerNiteligi] [varchar](100) NULL,
	[DolayliIntikal] [decimal](18, 2) NULL,
	[DolayliOdeme] [decimal](18, 2) NULL,
	[GirisSonrasiNakliye] [decimal](18, 2) NULL,
	[IthalaKatilanMalzeme] [decimal](18, 2) NULL,
	[IthalaUretimAraclar] [decimal](18, 2) NULL,
	[IthalaUretimTuketimMalzemesi] [decimal](18, 2) NULL,
	[KapAmbalajBedeli] [decimal](18, 2) NULL,
	[Komisyon] [decimal](18, 2) NULL,
	[Nakliye] [decimal](18, 2) NULL,
	[PlanTaslak] [decimal](18, 2) NULL,
	[RoyaltiLisans] [decimal](18, 2) NULL,
	[Sigorta] [decimal](18, 2) NULL,
	[TeknikYardim] [decimal](18, 2) NULL,
	[Tellaliye] [decimal](18, 2) NULL,
	[VergiHarcFon] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKonteyner]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKonteyner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KalemInternalNo] [varchar](30) NOT NULL,
	[UlkeKodu] [varchar](9) NOT NULL,
	[KonteynerNo] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbMarka]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbMarka](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KalemInternalNo] [varchar](30) NOT NULL,
	[MarkaAdi] [varchar](500) NOT NULL,
	[MarkaKiymeti] [decimal](18, 2) NOT NULL,
	[MarkaTescilNo] [varchar](20) NULL,
	[MarkaTuru] [varchar](9) NOT NULL,
	[Model] [varchar](30) NULL,
	[MotorGucu] [int] NULL,
	[MotorHacmi] [varchar](30) NULL,
	[MotorNo] [varchar](30) NULL,
	[MotorTipi] [varchar](20) NULL,
	[ModelYili] [varchar](30) NULL,
	[ReferansNo] [varchar](100) NULL,
	[Renk] [varchar](30) NULL,
	[SilindirAdet] [int] NOT NULL,
	[Vites] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbOdemeSekli]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOdemeSekli](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KalemInternalNo] [varchar](30) NOT NULL,
	[OdemeSekliKodu] [varchar](2) NOT NULL,
	[OdemeTutari] [decimal](18, 2) NOT NULL,
	[TBFID] [varchar](30) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbOzetbeyanAcma]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOzetbeyanAcma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[OzetBeyanInternalNo] [varchar](30) NOT NULL,
	[OzetbeyanNo] [varchar](30) NOT NULL,
	[IslemKapsami] [varchar](9) NULL,
	[Ambar] [varchar](9) NOT NULL,
	[BaskaRejim] [varchar](20) NOT NULL,
	[Aciklama] [varchar](1500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucBelgeler]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucBelgeler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[BelgeKodu] [varchar](10) NOT NULL,
	[BelgeAciklamasi] [varchar](1000) NOT NULL,
	[Dogrulama] [varchar](10) NULL,
	[Referans] [varchar](30) NULL,
	[BelgeTarihi] [varchar](12) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucDigerBilgiler]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucDigerBilgiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[CiktiSeriNo] [varchar](10) NULL,
	[DovizKuruAlis] [varchar](10) NULL,
	[DovizKuruSatis] [varchar](10) NULL,
	[MuayeneMemuru] [varchar](20) NULL,
	[KalanKontor] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucGumrukKiymeti]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucGumrukKiymeti](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[Miktar] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucHatalar]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucHatalar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[HataKodu] [int] NOT NULL,
	[HataAciklamasi] [varchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucHesapDetaylar]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucHesapDetaylar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[Miktar] [varchar](20) NULL,
	[Aciklama] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucIstatistikiKiymet]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucIstatistikiKiymet](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[Miktar] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucOzetBeyan]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucOzetBeyan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[OzetBeyanNo] [varchar](20) NULL,
	[TescilTarihi] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucSoruCevaplar]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucSoruCevaplar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[SoruKodu] [varchar](10) NOT NULL,
	[SoruCevap] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucSorular]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucSorular](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[SoruKodu] [varchar](9) NOT NULL,
	[SoruAciklamasi] [varchar](1000) NOT NULL,
	[Tip] [varchar](10) NOT NULL,
	[Cevaplar] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucToplamVergiler]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucToplamVergiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[VergiKodu] [varchar](10) NOT NULL,
	[VergiAciklamasi] [varchar](1000) NOT NULL,
	[Miktar] [varchar](20) NULL,
	[OdemeSekli] [varchar](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucToplananVergiler]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucToplananVergiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[Miktar] [varchar](20) NULL,
	[OdemeSekli] [varchar](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucVergiler]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucVergiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[VergiKodu] [varchar](10) NOT NULL,
	[VergiAciklamasi] [varchar](1000) NOT NULL,
	[Miktar] [varchar](20) NULL,
	[Oran] [varchar](5) NULL,
	[OdemeSekli] [varchar](3) NULL,
	[Matrah] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbTamamlayiciBilgi]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbTamamlayiciBilgi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[KalemInternalNo] [varchar](30) NOT NULL,
	[Gtip] [varchar](12) NOT NULL,
	[Bilgi] [varchar](20) NOT NULL,
	[Oran] [varchar](9) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbTasimaSatir]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbTasimaSatir](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[OzetBeyanInternalNo] [varchar](30) NOT NULL,
	[TasimaInternalNo] [varchar](30) NOT NULL,
	[TasimaSatirNo] [varchar](9) NOT NULL,
	[AmbarKodu] [varchar](9) NULL,
	[Miktar] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbTasimaSenedi]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbTasimaSenedi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[OzetBeyanInternalNo] [varchar](30) NOT NULL,
	[TasimaInternalNo] [varchar](30) NOT NULL,
	[TasimaSenediNo] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbTeminat]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbTeminat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [varchar](30) NOT NULL,
	[TeminatSekli] [varchar](9) NOT NULL,
	[TeminatOrani] [decimal](18, 2) NOT NULL,
	[GlobalTeminatNo] [varchar](20) NULL,
	[BankaMektubuTutari] [decimal](18, 2) NULL,
	[NakdiTeminatTutari] [decimal](18, 2) NULL,
	[DigerTutar] [decimal](18, 2) NULL,
	[DigerTutarReferansi] [varchar](20) NULL,
	[Aciklama] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbVergi]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbVergi](
	[KalemInternalNo] [varchar](30) NULL,
	[VergiKodu] [varchar](10) NULL,
	[Matrah] [numeric](18, 0) NULL,
	[Oran] [varchar](10) NULL,
	[VergiDegeri] [numeric](18, 0) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Islem]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Islem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RefNo] [varchar](30) NOT NULL,
	[Kullanici] [varchar](15) NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[BeyanInternalNo] [varchar](30) NULL,
	[BeyanTipi] [varchar](30) NULL,
	[IslemTipi] [varchar](10) NULL,
	[IslemDurumu] [varchar](30) NULL,
	[IslemSonucu] [varchar](500) NULL,
	[OlusturmaZamani] [datetime2](7) NULL,
	[IslemZamani] [datetime2](7) NULL,
	[Guidof] [varchar](50) NULL,
	[GonderimSayisi] [int] NOT NULL,
	[BeyanNo] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kullanici]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kullanici](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KullaniciKod] [varchar](15) NOT NULL,
	[KullaniciSifre] [varchar](15) NOT NULL,
	[Ad] [varchar](30) NOT NULL,
	[Soyad] [varchar](30) NOT NULL,
	[VergiNo] [varchar](15) NOT NULL,
	[FirmaAd] [varchar](150) NOT NULL,
	[MailAdres] [varchar](150) NOT NULL,
	[Aktif] [bit] NOT NULL,
	[Telefon] [varchar](20) NOT NULL,
	[SonIslemZamani] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Musteri]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Musteri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VergiNo] [varchar](15) NOT NULL,
	[FirmaAd] [varchar](150) NOT NULL,
	[Adres] [varchar](150) NOT NULL,
	[MailAdres] [varchar](150) NOT NULL,
	[Aktif] [bit] NOT NULL,
	[Telefon] [varchar](20) NOT NULL,
	[SonIslemZamani] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tarihce]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tarihce](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RefNo] [varchar](30) NOT NULL,
	[IslemInternalNo] [varchar](30) NOT NULL,
	[Kullanici] [varchar](15) NOT NULL,
	[Guid] [varchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[BeyanNo] [varchar](30) NULL,
	[TicaretTipi] [varchar](10) NOT NULL,
	[IslemTipi] [nchar](10) NOT NULL,
	[IslemDurumu] [varchar](30) NOT NULL,
	[IslemSonucu] [varchar](500) NULL,
	[Gumruk] [varchar](10) NOT NULL,
	[Rejim] [nchar](10) NOT NULL,
	[GonderilenVeri] [varchar](max) NULL,
	[GondermeZamani] [datetime2](7) NULL,
	[SonucVeri] [varchar](max) NULL,
	[SonucZamani] [datetime2](7) NULL,
	[ServistekiVeri] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[BeyannameKopyalama]    Script Date: 24.03.2020 22:05:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BeyannameKopyalama]
    @islemNo varchar(30),
	@yeniIslemNo varchar(30) OUTPUT 
AS

DECLARE @bNoInternalNo varchar(30);
BEGIN
SELECT   BeyanInternalNo as bNoInternalNo FROM BYTDb.dbo.Islem WHERE IslemInternalNo= @islemNo;
INSERT INTO BYTDb.dbo.DbBeyan  SELECT * FROM BYTDb.dbo.DbBeyan WHERE BeyanInternalNo=@bNoInternalNo;
set @yeniIslemNo=@bNoInternalNo;
RETURN  
END
GO
USE [master]
GO
ALTER DATABASE [BYTDb] SET  READ_WRITE 
GO
