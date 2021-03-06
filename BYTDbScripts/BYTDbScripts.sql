USE [master]
GO
/****** Object:  Database [BYTDb]    Script Date: 17.08.2020 20:33:56 ******/
CREATE DATABASE [BYTDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BYTDb', FILENAME = N'C:\Projeler\VeriTabanlari\BYTDb\BYTDb.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
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
/****** Object:  Sequence [dbo].[RefId1000]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefId1000] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefId4000]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefId4000] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefIdCIKONC]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefIdCIKONC] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefIdDENİTH]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefIdDENİTH] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefIdIghb]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefIdIghb] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefIdMesai]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefIdMesai] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefIdTIRİHR]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefIdTIRİHR] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefIdTR]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefIdTR] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
USE [BYTDb]
GO
/****** Object:  Sequence [dbo].[RefIdVARONC]    Script Date: 17.08.2020 20:33:56 ******/
CREATE SEQUENCE [dbo].[RefIdVARONC] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CACHE 
GO
/****** Object:  Table [dbo].[DbBelge]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbBelge](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[BelgeKodu] [nvarchar](10) NOT NULL,
	[BelgeAciklamasi] [nvarchar](1000) NULL,
	[Dogrulama] [nvarchar](10) NULL,
	[Referans] [nvarchar](30) NULL,
	[BelgeTarihi] [nvarchar](12) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbBeyan]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbBeyan](
	[RefNo] [nvarchar](30) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[BeyannameNo] [nvarchar](20) NULL,
	[Aciklamalar] [nvarchar](350) NULL,
	[AliciSaticiIliskisi] [nvarchar](9) NULL,
	[AliciVergiNo] [nvarchar](20) NULL,
	[AntrepoKodu] [nvarchar](9) NULL,
	[AsilSorumluVergiNo] [nvarchar](20) NULL,
	[BasitlestirilmisUsul] [nvarchar](9) NULL,
	[BankaKodu] [nvarchar](16) NULL,
	[BeyanSahibiVergiNo] [nvarchar](20) NULL,
	[BirlikKriptoNumarasi] [nvarchar](30) NULL,
	[BirlikKayitNumarasi] [nvarchar](30) NULL,
	[CikisUlkesi] [nvarchar](9) NULL,
	[CikistakiAracinKimligi] [nvarchar](35) NULL,
	[CikistakiAracinTipi] [nvarchar](9) NULL,
	[CikistakiAracinUlkesi] [nvarchar](9) NULL,
	[EsyaninBulunduguYer] [nvarchar](40) NULL,
	[GidecegiSevkUlkesi] [nvarchar](9) NULL,
	[GidecegiUlke] [nvarchar](9) NULL,
	[GirisGumrukIdaresi] [nvarchar](9) NULL,
	[GondericiVergiNo] [nvarchar](20) NULL,
	[Gumruk] [nvarchar](20) NULL,
	[IsleminNiteligi] [nvarchar](9) NULL,
	[KapAdedi] [int] NULL,
	[Konteyner] [nvarchar](9) NULL,
	[Kullanici] [nvarchar](15) NOT NULL,
	[LimanKodu] [nvarchar](9) NULL,
	[Mail1] [nvarchar](50) NULL,
	[Mail2] [nvarchar](50) NULL,
	[Mail3] [nvarchar](50) NULL,
	[Mobil1] [nvarchar](30) NULL,
	[Mobil2] [nvarchar](30) NULL,
	[MusavirVergiNo] [nvarchar](20) NULL,
	[OdemeAraci] [nvarchar](9) NULL,
	[MusavirReferansNo] [nvarchar](12) NULL,
	[ReferansTarihi] [nvarchar](12) NULL,
	[Rejim] [nvarchar](9) NOT NULL,
	[SinirdakiAracinKimligi] [nvarchar](35) NULL,
	[SinirdakiAracinTipi] [nvarchar](9) NULL,
	[SinirdakiAracinUlkesi] [nvarchar](9) NULL,
	[SinirdakiTasimaSekli] [nvarchar](9) NULL,
	[TasarlananGuzergah] [nvarchar](250) NULL,
	[TelafiEdiciVergi] [decimal](18, 2) NULL,
	[TescilStatu] [nvarchar](50) NULL,
	[TescilTarihi] [datetime2](7) NULL,
	[TeslimSekli] [nvarchar](9) NULL,
	[TeslimSekliYeri] [nvarchar](40) NULL,
	[TicaretUlkesi] [nvarchar](9) NULL,
	[ToplamFatura] [decimal](18, 2) NULL,
	[ToplamFaturaDovizi] [nvarchar](9) NULL,
	[ToplamNavlun] [decimal](18, 2) NULL,
	[ToplamNavlunDovizi] [nvarchar](9) NULL,
	[ToplamSigorta] [decimal](18, 2) NULL,
	[ToplamSigortaDovizi] [nvarchar](9) NULL,
	[ToplamYurtDisiHarcamalar] [decimal](18, 2) NULL,
	[ToplamYurtDisiHarcamalarDovizi] [nvarchar](9) NULL,
	[ToplamYurtIciHarcamalar] [decimal](18, 2) NULL,
	[VarisGumrukIdaresi] [nvarchar](9) NULL,
	[YukBelgeleriSayisi] [int] NULL,
	[YuklemeBosaltmaYeri] [nvarchar](40) NULL,
	[OlsuturulmaTarihi] [datetime2](7) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[BeyanInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbBeyannameAcma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbBeyannameAcma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[BeyannameNo] [nvarchar](20) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[Miktar] [decimal](18, 2) NOT NULL,
	[Aciklama] [nvarchar](100) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NOT NULL,
	[CaddeSokakNo] [nvarchar](150) NOT NULL,
	[Faks] [nvarchar](15) NULL,
	[IlIlce] [nvarchar](35) NOT NULL,
	[KimlikTuru] [nvarchar](9) NOT NULL,
	[No] [nvarchar](20) NOT NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[Telefon] [nvarchar](15) NULL,
	[Tip] [nvarchar](15) NULL,
	[UlkeKodu] [nvarchar](15) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_DbFirma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKalem]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKalem](
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[Aciklama44] [nvarchar](500) NULL,
	[Adet] [decimal](18, 0) NULL,
	[AlgilamaBirimi1] [nvarchar](9) NULL,
	[AlgilamaBirimi2] [nvarchar](9) NULL,
	[AlgilamaBirimi3] [nvarchar](9) NULL,
	[AlgilamaMiktari1] [decimal](18, 2) NULL,
	[AlgilamaMiktari2] [decimal](18, 2) NULL,
	[AlgilamaMiktari3] [decimal](18, 2) NULL,
	[BrutAgirlik] [decimal](18, 2) NULL,
	[Cins] [nvarchar](9) NULL,
	[EkKod] [nvarchar](9) NULL,
	[GirisCikisAmaci] [nvarchar](9) NULL,
	[GirisCikisAmaciAciklama] [nvarchar](300) NULL,
	[Gtip] [nvarchar](15) NOT NULL,
	[FaturaMiktari] [decimal](18, 2) NOT NULL,
	[FaturaMiktariDovizi] [nvarchar](9) NULL,
	[IkincilIslem] [nvarchar](9) NULL,
	[ImalatciFirmaBilgisi] [nvarchar](9) NULL,
	[ImalatciVergiNo] [nvarchar](15) NULL,
	[IstatistikiKiymet] [decimal](18, 2) NULL,
	[IstatistikiMiktar] [decimal](18, 2) NULL,
	[KalemIslemNiteligi] [nvarchar](9) NULL,
	[KalemSiraNo] [int] NOT NULL,
	[KullanilmisEsya] [nvarchar](9) NULL,
	[Marka] [nvarchar](70) NULL,
	[MahraceIade] [nvarchar](9) NULL,
	[MenseiUlke] [nvarchar](9) NULL,
	[Miktar] [decimal](18, 2) NULL,
	[MiktarBirimi] [nvarchar](9) NULL,
	[Muafiyetler1] [nvarchar](9) NULL,
	[Muafiyetler2] [nvarchar](9) NULL,
	[Muafiyetler3] [nvarchar](9) NULL,
	[Muafiyetler4] [nvarchar](9) NULL,
	[Muafiyetler5] [nvarchar](9) NULL,
	[MuafiyetAciklamasi] [nvarchar](500) NULL,
	[NavlunMiktari] [decimal](18, 2) NULL,
	[NavlunMiktariDovizi] [nvarchar](9) NULL,
	[NetAgirlik] [decimal](18, 2) NOT NULL,
	[Numara] [nvarchar](70) NULL,
	[Ozellik] [nvarchar](9) NULL,
	[ReferansTarihi] [nvarchar](12) NULL,
	[SatirNo] [nvarchar](20) NULL,
	[SigortaMiktari] [decimal](18, 2) NULL,
	[SigortaMiktariDovizi] [nvarchar](9) NULL,
	[SinirGecisUcreti] [decimal](18, 2) NULL,
	[StmIlKodu] [nvarchar](9) NULL,
	[TamamlayiciOlcuBirimi] [nvarchar](9) NULL,
	[TarifeTanimi] [nvarchar](350) NULL,
	[TicariTanimi] [nvarchar](350) NULL,
	[TeslimSekli] [nvarchar](9) NULL,
	[UluslararasiAnlasma] [nvarchar](9) NULL,
	[YurtDisiDiger] [decimal](18, 2) NULL,
	[YurtDisiDigerDovizi] [nvarchar](9) NULL,
	[YurtDisiDigerAciklama] [nvarchar](100) NULL,
	[YurtDisiDemuraj] [decimal](18, 2) NULL,
	[YurtDisiDemurajDovizi] [nvarchar](9) NULL,
	[YurtDisiFaiz] [decimal](18, 2) NULL,
	[YurtDisiFaizDovizi] [nvarchar](9) NULL,
	[YurtDisiKomisyon] [decimal](18, 2) NULL,
	[YurtDisiKomisyonDovizi] [nvarchar](9) NULL,
	[YurtDisiRoyalti] [decimal](18, 2) NULL,
	[YurtDisiRoyaltiDovizi] [nvarchar](9) NULL,
	[YurtIciBanka] [decimal](18, 2) NULL,
	[YurtIciCevre] [decimal](18, 2) NULL,
	[YurtIciDiger] [decimal](18, 2) NULL,
	[YurtIciDigerAciklama] [nvarchar](100) NULL,
	[YurtIciDepolama] [decimal](18, 2) NULL,
	[YurtIciKkdf] [decimal](18, 2) NULL,
	[YurtIciKultur] [decimal](18, 2) NULL,
	[YurtIciLiman] [decimal](18, 2) NULL,
	[YurtIciTahliye] [decimal](18, 2) NULL,
	[TimeStamp] [timestamp] NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[KalemInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKiymetBildirim]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKiymetBildirim](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KiymetInternalNo] [nvarchar](30) NOT NULL,
	[AliciSatici] [nvarchar](9) NOT NULL,
	[AliciSaticiAyrintilar] [nvarchar](300) NULL,
	[Edim] [nvarchar](9) NULL,
	[Emsal] [nvarchar](9) NULL,
	[FaturaTarihiSayisi] [nvarchar](300) NULL,
	[GumrukIdaresiKarari] [nvarchar](300) NULL,
	[Kisitlamalar] [nvarchar](9) NULL,
	[KisitlamalarAyrintilar] [nvarchar](300) NULL,
	[Munasebet] [nvarchar](9) NULL,
	[Royalti] [nvarchar](9) NULL,
	[RoyaltiKosullar] [nvarchar](300) NULL,
	[SaticiyaIntikal] [nvarchar](9) NULL,
	[SaticiyaIntikalKosullar] [nvarchar](300) NULL,
	[SehirYer] [nvarchar](300) NULL,
	[SozlesmeTarihiSayisi] [nvarchar](300) NULL,
	[Taahhutname] [nvarchar](9) NOT NULL,
	[TeslimSekli] [nvarchar](9) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKiymetBildirimKalem]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKiymetBildirimKalem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KiymetInternalNo] [nvarchar](30) NOT NULL,
	[KiymetKalemNo] [int] NOT NULL,
	[BeyannameKalemNo] [int] NOT NULL,
	[DigerOdemeler] [decimal](18, 2) NULL,
	[DigerOdemelerNiteligi] [nvarchar](100) NULL,
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
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbKonteyner]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbKonteyner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[UlkeKodu] [nvarchar](9) NOT NULL,
	[KonteynerNo] [nvarchar](20) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbMarka]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbMarka](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[MarkaAdi] [nvarchar](500) NOT NULL,
	[MarkaKiymeti] [decimal](18, 2) NOT NULL,
	[MarkaTescilNo] [nvarchar](20) NULL,
	[MarkaTuru] [nvarchar](9) NOT NULL,
	[Model] [nvarchar](30) NULL,
	[MotorGucu] [int] NULL,
	[MotorHacmi] [nvarchar](30) NULL,
	[MotorNo] [nvarchar](30) NULL,
	[MotorTipi] [nvarchar](20) NULL,
	[ModelYili] [nvarchar](30) NULL,
	[ReferansNo] [nvarchar](100) NULL,
	[Renk] [nvarchar](30) NULL,
	[SilindirAdet] [int] NOT NULL,
	[Vites] [nvarchar](20) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbOdemeSekli]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOdemeSekli](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[OdemeSekliKodu] [nvarchar](2) NOT NULL,
	[OdemeTutari] [decimal](18, 2) NOT NULL,
	[TBFID] [nvarchar](30) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_DbOdemeSekli] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbOzetBeyanAcma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOzetBeyanAcma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanNo] [nvarchar](30) NOT NULL,
	[IslemKapsami] [nvarchar](9) NULL,
	[Ambar] [nvarchar](9) NOT NULL,
	[BaskaRejim] [nvarchar](20) NOT NULL,
	[Aciklama] [nvarchar](1500) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbOzetBeyanAcmaTasimaSatir]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOzetBeyanAcmaTasimaSatir](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSatirNo] [int] NOT NULL,
	[AmbarKodu] [nvarchar](9) NULL,
	[Miktar] [decimal](18, 2) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbOzetBeyanAcmaTasimaSenet]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOzetBeyanAcmaTasimaSenet](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenediNo] [nvarchar](20) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucBelgeler]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucBelgeler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[BelgeKodu] [nvarchar](10) NOT NULL,
	[BelgeAciklamasi] [nvarchar](1000) NOT NULL,
	[Dogrulama] [nvarchar](10) NULL,
	[Referans] [nvarchar](30) NULL,
	[BelgeTarihi] [nvarchar](12) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucDigerBilgiler]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucDigerBilgiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[CiktiSeriNo] [nvarchar](10) NULL,
	[DovizKuruAlis] [nvarchar](10) NULL,
	[DovizKuruSatis] [nvarchar](10) NULL,
	[MuayeneMemuru] [nvarchar](20) NULL,
	[KalanKontor] [nvarchar](10) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucGumrukKiymeti]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucGumrukKiymeti](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[Miktar] [nvarchar](20) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucHatalar]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucHatalar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[HataKodu] [int] NOT NULL,
	[HataAciklamasi] [nvarchar](1000) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucHesapDetaylar]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucHesapDetaylar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[Miktar] [nvarchar](20) NULL,
	[Aciklama] [nvarchar](100) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucIstatistikiKiymet]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucIstatistikiKiymet](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[Miktar] [nvarchar](20) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucOzetBeyan]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucOzetBeyan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanNo] [nvarchar](20) NULL,
	[TescilTarihi] [nvarchar](20) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucSoruCevaplar]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucSoruCevaplar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[SoruKodu] [nvarchar](10) NOT NULL,
	[SoruCevap] [nvarchar](10) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucSorular]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucSorular](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[SoruKodu] [nvarchar](9) NOT NULL,
	[SoruAciklamasi] [nvarchar](1000) NOT NULL,
	[Tip] [nvarchar](10) NOT NULL,
	[Cevaplar] [nvarchar](max) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucToplamVergiler]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucToplamVergiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[VergiKodu] [nvarchar](10) NOT NULL,
	[VergiAciklamasi] [nvarchar](1000) NOT NULL,
	[Miktar] [nvarchar](20) NULL,
	[OdemeSekli] [nvarchar](3) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucToplananVergiler]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucToplananVergiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[Miktar] [nvarchar](20) NULL,
	[OdemeSekli] [nvarchar](3) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSonucVergiler]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSonucVergiler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[VergiKodu] [nvarchar](10) NOT NULL,
	[VergiAciklamasi] [nvarchar](1000) NOT NULL,
	[Miktar] [nvarchar](20) NULL,
	[Oran] [nvarchar](5) NULL,
	[OdemeSekli] [nvarchar](3) NULL,
	[Matrah] [nvarchar](20) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbSoruCevap]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbSoruCevap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[SoruKodu] [nvarchar](10) NOT NULL,
	[SoruCevap] [nvarchar](10) NULL,
	[SoruAciklamasi] [nvarchar](1000) NULL,
	[Tip] [nvarchar](10) NOT NULL,
	[Cevaplar] [nvarchar](max) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbTamamlayiciBilgi]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbTamamlayiciBilgi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[Gtip] [nvarchar](12) NOT NULL,
	[Bilgi] [nvarchar](20) NOT NULL,
	[Oran] [nvarchar](9) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbTeminat]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbTeminat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[TeminatSekli] [nvarchar](9) NOT NULL,
	[TeminatOrani] [decimal](18, 2) NOT NULL,
	[GlobalTeminatNo] [nvarchar](20) NULL,
	[BankaMektubuTutari] [decimal](18, 2) NULL,
	[NakdiTeminatTutari] [decimal](18, 2) NULL,
	[DigerTutar] [decimal](18, 2) NULL,
	[DigerTutarReferansi] [nvarchar](20) NULL,
	[Aciklama] [nvarchar](100) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbVergi]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbVergi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[KalemNo] [int] NOT NULL,
	[VergiKodu] [int] NOT NULL,
	[VergiAciklamasi] [nvarchar](1000) NULL,
	[Miktar] [decimal](18, 2) NOT NULL,
	[Oran] [nvarchar](5) NOT NULL,
	[OdemeSekli] [nvarchar](3) NOT NULL,
	[Matrah] [decimal](18, 2) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ighb]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ighb](
	[IghbInternalNo] [nvarchar](30) NOT NULL,
	[RefNo] [nvarchar](30) NOT NULL,
	[KullaniciKodu] [nvarchar](15) NOT NULL,
	[GumrukKodu] [nvarchar](9) NOT NULL,
	[IzinliGondericiVergiNo] [nvarchar](20) NOT NULL,
	[PlakaBilgisi] [nvarchar](100) NOT NULL,
	[TesisKodu] [nvarchar](30) NOT NULL,
	[TescilStatu] [nvarchar](50) NULL,
	[TescilTarihi] [datetime2](7) NULL,
	[OlsuturulmaTarihi] [datetime2](7) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[IghbInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IghbListe]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IghbListe](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IghbInternalNo] [nvarchar](30) NOT NULL,
	[TcgbNumarasi] [nvarchar](30) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Islem]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Islem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RefNo] [nvarchar](30) NOT NULL,
	[Kullanici] [nvarchar](15) NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[BeyanInternalNo] [nvarchar](30) NULL,
	[BeyanTipi] [nvarchar](30) NULL,
	[IslemTipi] [nvarchar](10) NULL,
	[IslemDurumu] [nvarchar](30) NULL,
	[IslemSonucu] [nvarchar](500) NULL,
	[OlusturmaZamani] [datetime2](7) NULL,
	[IslemZamani] [datetime2](7) NULL,
	[Guidof] [nvarchar](50) NULL,
	[GonderimSayisi] [int] NOT NULL,
	[BeyanNo] [nvarchar](30) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kullanici]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kullanici](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KullaniciKod] [nvarchar](15) NOT NULL,
	[KullaniciSifre] [nvarchar](15) NOT NULL,
	[Ad] [nvarchar](30) NOT NULL,
	[Soyad] [nvarchar](30) NOT NULL,
	[VergiNo] [nvarchar](15) NULL,
	[FirmaAd] [nvarchar](150) NULL,
	[MailAdres] [nvarchar](150) NULL,
	[Aktif] [bit] NOT NULL,
	[Telefon] [nvarchar](20) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KullaniciYetki]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KullaniciYetki](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KullaniciKod] [nvarchar](15) NOT NULL,
	[YetkiKodu] [nvarchar](15) NOT NULL,
	[Aktif] [bit] NOT NULL,
	[SonIslemZamani] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetimeoffset](7) NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [xml] NULL,
	[LogEvent] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mesai]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mesai](
	[MesaiInternalNo] [nvarchar](30) NOT NULL,
	[RefNo] [nvarchar](30) NOT NULL,
	[KullaniciKodu] [nvarchar](15) NOT NULL,
	[GumrukKodu] [nvarchar](9) NOT NULL,
	[MesaiID] [nvarchar](30) NOT NULL,
	[AracAdedi] [int] NULL,
	[BeyannameNo] [nvarchar](30) NULL,
	[DigerNo] [nvarchar](30) NULL,
	[FirmaVergiNo] [nvarchar](15) NOT NULL,
	[Adres] [nvarchar](150) NOT NULL,
	[EsyaninBulunduguYer] [nvarchar](100) NOT NULL,
	[EsyaninBulunduguYerAdi] [nvarchar](50) NULL,
	[EsyaninBulunduguYerKodu] [nvarchar](30) NULL,
	[GlobalHesaptanOdeme] [nvarchar](15) NULL,
	[GumrukSahasinda] [nvarchar](15) NULL,
	[IrtibatAdSoyad] [nvarchar](50) NOT NULL,
	[IrtibatTelefonNo] [nvarchar](20) NOT NULL,
	[IslemTipi] [nvarchar](20) NOT NULL,
	[OdemeYapacakFirmaVergiNo] [nvarchar](15) NULL,
	[NCTSSayisi] [int] NULL,
	[OZBYSayisi] [int] NULL,
	[Uzaklik] [int] NULL,
	[BaslangicZamani] [nvarchar](20) NULL,
	[TescilStatu] [nvarchar](50) NULL,
	[TescilTarihi] [datetime2](7) NULL,
	[OlsuturulmaTarihi] [datetime2](7) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[MesaiInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Musteri]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Musteri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VergiNo] [nvarchar](15) NOT NULL,
	[FirmaAd] [nvarchar](150) NOT NULL,
	[Adres] [nvarchar](150) NOT NULL,
	[MailAdres] [nvarchar](150) NOT NULL,
	[Aktif] [bit] NOT NULL,
	[Telefon] [nvarchar](20) NOT NULL,
	[SonIslemZamani] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbAbAcma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbAbAcma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[BeyannameNo] [nvarchar](20) NULL,
	[KalemNo] [int] NULL,
	[AcilanKalemNo] [int] NULL,
	[Miktar] [decimal](18, 2) NULL,
	[Kiymet] [decimal](18, 2) NULL,
	[TeslimSekli] [nvarchar](9) NULL,
	[DovizCinsi] [nvarchar](9) NULL,
	[OdemeSekli] [nvarchar](9) NULL,
	[IsleminNiteligi] [nvarchar](9) NULL,
	[TicaretUlkesi] [nvarchar](9) NULL,
	[Aciklama] [nvarchar](200) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbAbAcma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbAliciFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbAliciFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NOT NULL,
	[CaddeSokakNo] [nvarchar](150) NOT NULL,
	[Dil] [nvarchar](4) NOT NULL,
	[IlIlce] [nvarchar](35) NOT NULL,
	[No] [nvarchar](15) NOT NULL,
	[PostaKodu] [nvarchar](10) NOT NULL,
	[UlkeKodu] [nvarchar](4) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbAlici] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbAsilSorumluFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbAsilSorumluFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NOT NULL,
	[CaddeSokakNo] [nvarchar](150) NOT NULL,
	[Dil] [nvarchar](4) NOT NULL,
	[IlIlce] [nvarchar](35) NOT NULL,
	[No] [nvarchar](15) NOT NULL,
	[PostaKodu] [nvarchar](10) NOT NULL,
	[UlkeKodu] [nvarchar](4) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbAsilSorumlu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbBelgeler]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbBelgeler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[BelgeTipi] [nvarchar](4) NULL,
	[RefNo] [nvarchar](70) NULL,
	[BelgeDil] [nvarchar](4) NULL,
	[TamamlayiciOlcu] [nvarchar](26) NULL,
	[TamamlayiciOlcuDil] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbBelgeler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbBeyan]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbBeyan](
	[RefNo] [nvarchar](30) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[BeyannameNo] [nvarchar](20) NULL,
	[Rejim] [nvarchar](4) NOT NULL,
	[BeyanTipi] [nvarchar](4) NULL,
	[BeyanTipiDil] [nvarchar](15) NOT NULL,
	[HareketGumruk] [nvarchar](15) NOT NULL,
	[VarisGumruk] [nvarchar](15) NOT NULL,
	[SinirGumruk] [nvarchar](15) NOT NULL,
	[VarisUlke] [nvarchar](4) NOT NULL,
	[CikisUlke] [nvarchar](4) NOT NULL,
	[EsyaKabulYer] [nvarchar](17) NULL,
	[EsyaKabulYerDil] [nvarchar](4) NULL,
	[EsyaKabulYerKod] [nvarchar](17) NULL,
	[EsyaOnayYer] [nvarchar](17) NULL,
	[YuklemeYeri] [nvarchar](20) NULL,
	[EsyaYer] [nvarchar](20) NULL,
	[DahildeTasimaSekli] [nvarchar](4) NULL,
	[CikisTasimaSekli] [nvarchar](4) NULL,
	[CikisTasitKimligi] [nvarchar](4) NULL,
	[CikisTasitKimligiDil] [nvarchar](4) NULL,
	[CikisTasitUlke] [nvarchar](4) NULL,
	[SinirTasitKimligi] [nvarchar](40) NULL,
	[SinirTasitKimligiDil] [nvarchar](9) NULL,
	[SinirTasitUlke] [nvarchar](4) NULL,
	[SinirTasimaSekli] [nvarchar](40) NULL,
	[Konteyner] [bit] NULL,
	[Tanker] [bit] NULL,
	[KalemSayisi] [int] NULL,
	[ToplamKapSayisi] [int] NULL,
	[KalemToplamBrutKG] [decimal](18, 2) NULL,
	[Yer] [nvarchar](15) NULL,
	[YerTarihDil] [nvarchar](4) NULL,
	[Temsilci] [nvarchar](50) NOT NULL,
	[TemsilKapasite] [nvarchar](9) NOT NULL,
	[TemsilKapasiteDil] [nvarchar](4) NULL,
	[VarisGumrukYetkilisi] [nvarchar](17) NULL,
	[KontrolSonuc] [nvarchar](9) NULL,
	[SureSinir] [nvarchar](12) NULL,
	[OdemeAraci] [nvarchar](4) NULL,
	[RefaransNo] [nvarchar](10) NULL,
	[GuvenliBeyan] [int] NULL,
	[KonveyansRefNo] [nvarchar](35) NULL,
	[BosaltmaYer] [nvarchar](35) NULL,
	[YukBosYerDil] [nvarchar](4) NULL,
	[Dorse1] [nvarchar](50) NULL,
	[Dorse2] [nvarchar](50) NULL,
	[DamgaVergi] [decimal](18, 2) NULL,
	[MusavirKimlikNo] [nvarchar](15) NULL,
	[Kullanici] [nvarchar](15) NULL,
	[TescilStatu] [nvarchar](50) NULL,
	[TescilTarihi] [datetime2](7) NULL,
	[OlsuturulmaTarihi] [datetime2](7) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[NctsBeyanInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbBeyanSahibi]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbBeyanSahibi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbBeyanSahibi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbEkBilgi]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbEkBilgi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[EkBilgiKod] [nvarchar](5) NULL,
	[EkBilgi] [nvarchar](70) NULL,
	[Dil] [nvarchar](4) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[Ec2Ihr] [int] NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbEkBilgi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbGondericiFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbGondericiFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NOT NULL,
	[CaddeSokakNo] [nvarchar](150) NOT NULL,
	[Dil] [nvarchar](4) NOT NULL,
	[IlIlce] [nvarchar](35) NOT NULL,
	[No] [nvarchar](15) NOT NULL,
	[PostaKodu] [nvarchar](10) NOT NULL,
	[UlkeKodu] [nvarchar](4) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbGonderici] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbGuvenliAliciFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbGuvenliAliciFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[Dil] [nvarchar](4) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbGuvenliAlici] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbGuvenliGondericiFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbGuvenliGondericiFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[Dil] [nvarchar](4) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbGuvenliGondrici] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbHassasEsya]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbHassasEsya](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[Kod] [int] NULL,
	[Miktar] [decimal](18, 2) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbHassasEsya] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbKalem]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbKalem](
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[KalemSiraNo] [int] NOT NULL,
	[Gtip] [nvarchar](16) NOT NULL,
	[RejimKodu] [nvarchar](4) NULL,
	[TicariTanim] [nvarchar](210) NULL,
	[TicariTanimDil] [nvarchar](4) NULL,
	[Kiymet] [decimal](18, 2) NULL,
	[KiymetDoviz] [nvarchar](4) NULL,
	[BurutAgirlik] [decimal](18, 2) NOT NULL,
	[NetAgirlik] [decimal](18, 2) NULL,
	[VarisUlkesi] [nvarchar](4) NULL,
	[CikisUlkesi] [nvarchar](4) NULL,
	[TptChMOdemeKod] [nvarchar](4) NULL,
	[KonsimentoRef] [nvarchar](70) NULL,
	[UNDG] [nvarchar](4) NULL,
	[IhrBeyanNo] [nvarchar](20) NULL,
	[IhrBeyanTip] [nvarchar](9) NULL,
	[IhrBeyanParcali] [nvarchar](9) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[KalemInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbKalemAliciFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbKalemAliciFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[Dil] [nvarchar](4) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbKalemAlici] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbKalemGondericiFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbKalemGondericiFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[Dil] [nvarchar](4) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbKalemGonderici] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbKalemGuvenliAliciFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbKalemGuvenliAliciFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[Dil] [nvarchar](4) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbKalemGuvenliAliciFirma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbKalemGuvenliGondericiFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbKalemGuvenliGondericiFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[Dil] [nvarchar](4) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbKalemGuvenliGondericiFirma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbKap]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbKap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[MarkaNo] [nvarchar](42) NULL,
	[KapTipi] [nvarchar](4) NULL,
	[MarkaDil] [nvarchar](4) NULL,
	[KapAdet] [int] NULL,
	[ParcaSayisi] [int] NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbKap] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbKonteyner]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbKonteyner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[KonteynerNo] [nvarchar](50) NOT NULL,
	[Ulke] [nvarchar](4) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbKonteyner] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbMuhur]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbMuhur](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[MuhurNo] [nvarchar](50) NOT NULL,
	[Dil] [nvarchar](4) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbMuhur] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbObAcma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbObAcma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[IslemKapsami] [nvarchar](4) NULL,
	[OzetBeyanNo] [nvarchar](20) NULL,
	[AmbarIci] [nvarchar](9) NULL,
	[AmbarKodu] [nvarchar](9) NULL,
	[TasimaSenetNo] [nvarchar](20) NULL,
	[TasimaSatirNo] [int] NULL,
	[Miktar] [decimal](18, 2) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbObAcma] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbOncekiBelgeler]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbOncekiBelgeler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[KalemInternalNo] [nvarchar](30) NOT NULL,
	[BelgeTipi] [nvarchar](4) NULL,
	[RefNo] [nvarchar](35) NULL,
	[BelgeDil] [nvarchar](4) NULL,
	[TamamlayiciBilgi] [nvarchar](26) NULL,
	[TamamlayiciBilgiDil] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbOncekiBelgeler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbRota]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbRota](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[UlkeKodu] [nvarchar](4) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbRota] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbTasiyiciFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbTasiyiciFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NULL,
	[CaddeSokakNo] [nvarchar](150) NULL,
	[Dil] [nvarchar](4) NULL,
	[IlIlce] [nvarchar](35) NULL,
	[No] [nvarchar](15) NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[UlkeKodu] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbTasiyici] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbTeminat]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbTeminat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TeminatTipi] [nvarchar](4) NOT NULL,
	[GRNNo] [nvarchar](16) NOT NULL,
	[DigerRefNo] [nvarchar](35) NULL,
	[ErisimKodu] [nvarchar](4) NULL,
	[DovizCinsi] [nvarchar](4) NOT NULL,
	[Tutar] [decimal](18, 2) NOT NULL,
	[ECGecerliDegil] [int] NULL,
	[UlkeGecerliDegil] [nvarchar](4) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbTeminat] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbTransitGumruk]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbTransitGumruk](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NctsBeyanInternalNo] [nvarchar](30) NOT NULL,
	[Gumruk] [nvarchar](9) NOT NULL,
	[VarisTarihi] [nvarchar](20) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
 CONSTRAINT [PK_NbTransitGumruk] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObBeyan]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObBeyan](
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanNo] [nvarchar](20) NULL,
	[BeyanSahibiVergiNo] [nvarchar](20) NULL,
	[BeyanTuru] [nvarchar](9) NOT NULL,
	[Diger] [nvarchar](500) NULL,
	[DorseNo1] [nvarchar](25) NULL,
	[DorseNo1Uyrugu] [nvarchar](9) NULL,
	[DorseNo2] [nvarchar](25) NULL,
	[DorseNo2Uyrugu] [nvarchar](9) NULL,
	[EkBelgeSayisi] [int] NULL,
	[EmniyetGuvenlik] [nvarchar](9) NULL,
	[GrupTasimaSenediNo] [nvarchar](20) NULL,
	[GumrukIdaresi] [nvarchar](9) NULL,
	[KullaniciKodu] [nvarchar](15) NOT NULL,
	[Kurye] [nvarchar](9) NULL,
	[LimanYerAdiBos] [nvarchar](20) NULL,
	[LimanYerAdiYuk] [nvarchar](20) NULL,
	[OncekiBeyanNo] [nvarchar](20) NULL,
	[PlakaSeferNo] [nvarchar](25) NULL,
	[ReferansNumarasi] [nvarchar](25) NULL,
	[RefNo] [nvarchar](25) NULL,
	[Rejim] [nvarchar](9) NOT NULL,
	[TasimaSekli] [nvarchar](9) NULL,
	[TasitinAdi] [nvarchar](50) NULL,
	[TasiyiciVergiNo] [nvarchar](20) NULL,
	[TirAtaKarneNo] [nvarchar](20) NULL,
	[UlkeKodu] [nvarchar](9) NULL,
	[UlkeKoduYuk] [nvarchar](9) NULL,
	[UlkeKoduBos] [nvarchar](9) NULL,
	[YuklemeBosaltmaYeri] [nvarchar](20) NULL,
	[VarisCikisGumrukIdaresi] [nvarchar](9) NULL,
	[VarisTarihSaati] [nvarchar](12) NULL,
	[XmlRefId] [nvarchar](35) NOT NULL,
	[TescilStatu] [nvarchar](50) NULL,
	[TescilTarihi] [datetime2](7) NULL,
	[OlsuturulmaTarihi] [datetime2](7) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[OzetBeyanInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NOT NULL,
	[CaddeSokakNo] [nvarchar](150) NOT NULL,
	[Faks] [nvarchar](15) NULL,
	[IlIlce] [nvarchar](35) NOT NULL,
	[KimlikTuru] [nvarchar](9) NOT NULL,
	[No] [nvarchar](20) NOT NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[Telefon] [nvarchar](15) NULL,
	[Tip] [nvarchar](15) NULL,
	[UlkeKodu] [nvarchar](15) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObIhracat]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObIhracat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[BrutAgirlik] [decimal](18, 2) NOT NULL,
	[KapAdet] [int] NOT NULL,
	[Numara] [nvarchar](20) NOT NULL,
	[Parcali] [nvarchar](9) NOT NULL,
	[Tip] [nvarchar](9) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObOzetBeyanAcma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObOzetBeyanAcma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanAcmaBeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanNo] [nvarchar](30) NOT NULL,
	[IslemKapsami] [nvarchar](9) NULL,
	[Ambar] [nvarchar](9) NOT NULL,
	[BaskaRejim] [nvarchar](20) NOT NULL,
	[Aciklama] [nvarchar](1500) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObOzetBeyanAcmaTasimaSatir]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObOzetBeyanAcmaTasimaSatir](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanAcmaBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[AcmaSatirNo] [int] NOT NULL,
	[AmbarKodu] [nvarchar](9) NULL,
	[AmbardakiMiktar] [decimal](18, 2) NULL,
	[AcilacakMiktar] [decimal](18, 2) NULL,
	[MarkaNo] [nvarchar](60) NULL,
	[EsyaCinsi] [nvarchar](9) NULL,
	[Birim] [nvarchar](9) NULL,
	[ToplamMiktar] [decimal](18, 2) NULL,
	[KapatilanMiktar] [decimal](18, 2) NULL,
	[OlcuBirimi] [nvarchar](9) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObOzetBeyanAcmaTasimaSenet]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObOzetBeyanAcmaTasimaSenet](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[OzetBeyanAcmaBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenediNo] [nvarchar](20) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObSatirEsya]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObSatirEsya](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSatirInternalNo] [nvarchar](30) NOT NULL,
	[BmEsyaKodu] [nvarchar](15) NULL,
	[BrutAgirlik] [decimal](18, 2) NULL,
	[EsyaKodu] [nvarchar](12) NULL,
	[EsyaninTanimi] [nvarchar](150) NULL,
	[KalemFiyati] [decimal](18, 2) NULL,
	[KalemFiyatiDoviz] [nvarchar](9) NOT NULL,
	[KalemSiraNo] [int] NOT NULL,
	[NetAgirlik] [decimal](18, 2) NULL,
	[OlcuBirimi] [nvarchar](9) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObTasimaSatir]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObTasimaSatir](
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSatirInternalNo] [nvarchar](30) NOT NULL,
	[BrutAgirlik] [decimal](18, 2) NULL,
	[KapAdet] [int] NOT NULL,
	[KapCinsi] [nvarchar](9) NULL,
	[KonteynerTipi] [nvarchar](9) NOT NULL,
	[MarkaNo] [nvarchar](60) NOT NULL,
	[MuhurNumarasi] [nvarchar](35) NOT NULL,
	[NetAgirlik] [decimal](18, 2) NULL,
	[OlcuBirimi] [nvarchar](9) NULL,
	[SatirNo] [int] NULL,
	[KonteynerYukDurumu] [nvarchar](9) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[TasimaSatirInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObTasimaSenet]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObTasimaSenet](
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenediNo] [nvarchar](20) NOT NULL,
	[AcentaAdi] [nvarchar](150) NOT NULL,
	[AcentaVergiNo] [nvarchar](20) NOT NULL,
	[AliciAdi] [nvarchar](150) NOT NULL,
	[AliciVergiNo] [nvarchar](20) NOT NULL,
	[AmbarHarici] [nvarchar](9) NOT NULL,
	[BildirimTarafiAdi] [nvarchar](150) NULL,
	[BildirimTarafiVergiNo] [nvarchar](20) NULL,
	[DuzenlendigiUlke] [nvarchar](9) NULL,
	[EmniyetGuvenlik] [nvarchar](9) NULL,
	[EsyaninBulunduguYer] [nvarchar](16) NULL,
	[FaturaDoviz] [nvarchar](9) NULL,
	[FaturaToplami] [decimal](18, 2) NULL,
	[GondericiAdi] [nvarchar](150) NULL,
	[GondericiVergiNo] [nvarchar](20) NULL,
	[Grup] [nvarchar](9) NULL,
	[Konteyner] [nvarchar](9) NULL,
	[NavlunDoviz] [nvarchar](9) NULL,
	[NavlunTutari] [decimal](18, 2) NULL,
	[OdemeSekli] [nvarchar](9) NULL,
	[OncekiSeferNumarasi] [nvarchar](20) NULL,
	[OncekiSeferTarihi] [nvarchar](20) NULL,
	[OzetBeyanNo] [nvarchar](20) NULL,
	[Roro] [nvarchar](9) NULL,
	[SenetSiraNo] [int] NOT NULL,
	[AktarmaYapilacak] [nvarchar](9) NULL,
	[AktarmaTipi] [nvarchar](20) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[TasimaSenetInternalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObTasitUgrakUlke]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObTasitUgrakUlke](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[UlkeKodu] [nvarchar](30) NOT NULL,
	[LimanYerAdi] [nvarchar](30) NOT NULL,
	[HareketTarihSaati] [nvarchar](12) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObTasiyiciFirma]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObTasiyiciFirma](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[AdUnvan] [nvarchar](150) NOT NULL,
	[CaddeSokakNo] [nvarchar](150) NOT NULL,
	[Faks] [nvarchar](15) NULL,
	[IlIlce] [nvarchar](35) NOT NULL,
	[KimlikTuru] [nvarchar](9) NOT NULL,
	[No] [nvarchar](20) NOT NULL,
	[PostaKodu] [nvarchar](10) NULL,
	[Telefon] [nvarchar](15) NULL,
	[Tip] [nvarchar](15) NULL,
	[UlkeKodu] [nvarchar](15) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObTeminat]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObTeminat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TeminatSekli] [nvarchar](9) NOT NULL,
	[TeminatOrani] [decimal](18, 2) NOT NULL,
	[GlobalTeminatNo] [nvarchar](20) NULL,
	[BankaMektubuTutari] [decimal](18, 2) NULL,
	[NakdiTeminatTutari] [decimal](18, 2) NULL,
	[DigerTutar] [decimal](18, 2) NULL,
	[DigerTutarReferansi] [nvarchar](20) NULL,
	[Aciklama] [nvarchar](100) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObUgrakUlke]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObUgrakUlke](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OzetBeyanInternalNo] [nvarchar](30) NOT NULL,
	[TasimaSenetInternalNo] [nvarchar](30) NOT NULL,
	[UlkeKodu] [nvarchar](9) NOT NULL,
	[LimanYerAdi] [nvarchar](30) NOT NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tarihce]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tarihce](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RefNo] [nvarchar](30) NOT NULL,
	[IslemInternalNo] [nvarchar](30) NOT NULL,
	[Kullanici] [nvarchar](15) NOT NULL,
	[Guid] [nvarchar](50) NOT NULL,
	[GonderimNo] [int] NOT NULL,
	[BeyanNo] [nvarchar](30) NULL,
	[TicaretTipi] [nvarchar](10) NOT NULL,
	[IslemTipi] [nchar](10) NOT NULL,
	[IslemDurumu] [nvarchar](30) NOT NULL,
	[IslemSonucu] [nvarchar](500) NULL,
	[Gumruk] [nvarchar](10) NOT NULL,
	[Rejim] [nchar](10) NOT NULL,
	[GonderilecekVeri] [nvarchar](max) NULL,
	[OlusturmaZamani] [datetime2](7) NULL,
	[GonderilenVeri] [nvarchar](max) NULL,
	[GondermeZamani] [datetime2](7) NULL,
	[SonucVeri] [nvarchar](max) NULL,
	[SonucZamani] [datetime2](7) NULL,
	[ServistekiVeri] [nvarchar](max) NULL,
	[ImzaliVeri] [nvarchar](max) NULL,
	[SonIslemZamani] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Yetki]    Script Date: 17.08.2020 20:33:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Yetki](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[YetkiKodu] [nvarchar](15) NOT NULL,
	[YetkiAdi] [nvarchar](50) NOT NULL,
	[Aciklama] [nvarchar](500) NOT NULL,
	[Aktif] [bit] NOT NULL,
	[SonIslemZamani] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[BeyannameKopyalama]    Script Date: 17.08.2020 20:33:56 ******/
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
