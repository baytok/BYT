using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.IO;

namespace BYT.UI
{

    public class Imzala
    {
    

        public static byte[] TLVGetLength(int len, ref int returnLen)
        {
            byte[] r = null;
            if (len < 0x80)
            {
                r = new byte[1];
                r[0] = BitConverter.GetBytes(len)[0];
                returnLen = 1;
            }
            else if (len < 0x00010000)
            {
                if (len < 0x0100)
                {
                    r = new byte[2];
                    r[0] = (byte)0x81;
                    r[1] = (byte)(len & 0x0000ff);
                    returnLen = 2;
                }
                else
                {
                    r = new byte[3];
                    r[0] = (byte)0x82;
                    r[1] = (byte)((len & 0x00ff00) >> 8);
                    r[2] = (byte)(len & 0x0000ff);
                    returnLen = 3;
                }
            }
            return r;
        }

        public struct TLVStruct
        {
            public int cbData;
            public byte[] pbData;
        }

        public static TLVStruct TLVLength(int len)
        {
            TLVStruct r = new TLVStruct();
            int x = 0;
            byte[] p = TLVGetLength(len, ref x);
            r.cbData = x;
            r.pbData = p;
            return r;
        }

        static public void TLVAddTag(ref TLVStruct blob, byte tag)
        {
            TLVStruct r, len;
            len = TLVLength(blob.cbData);
            r.cbData = 1 + len.cbData + blob.cbData;
            r.pbData = new byte[r.cbData];
            r.pbData[0] = tag;

            Buffer.BlockCopy(len.pbData, 0, r.pbData, 1, len.cbData);
            Buffer.BlockCopy(blob.pbData, 0, r.pbData, 1 + len.cbData, blob.cbData);
            blob = r;
        }

        static public TLVStruct MakeTLV(ref TLVStruct data1, ref TLVStruct data2)
        {
            TLVStruct r, len;
            len = TLVLength(data1.cbData + data2.cbData);
            r.cbData = 1 + len.cbData + data1.cbData + data2.cbData;
            r.pbData = new byte[r.cbData];
            r.pbData[0] = 0x30;
            Buffer.BlockCopy(len.pbData, 0, r.pbData, 1, len.cbData);
            Buffer.BlockCopy(data1.pbData, 0, r.pbData, 1 + len.cbData, data1.cbData);
            Buffer.BlockCopy(data2.pbData, 0, r.pbData, 1 + len.cbData + data1.cbData, data2.cbData);
            return r;
        }

        static public string MesajImzala(string msg, string imzasahibi)
        {
            Encoding unicode = Encoding.UTF8;
            byte[] msgBytes = unicode.GetBytes(msg);
            X509Certificate2 signerCert = GetSignerCert(imzasahibi);


            // OCSP
            /*
            System.Security.Cryptography.X509Certificates.X509Chain ch=new X509Chain();
            ch.Build(signerCert);
          
            int a = 0;
            X509Certificate certE = null;

            foreach (X509ChainElement elem in ch.ChainElements)
            {

                certE = new X509Certificate(elem.Certificate.RawData);

                if (a == 1) break;
                a++;

            }
            

           
             OcspSorgu ocsp = new OcspSorgu();
             Org.BouncyCastle.X509.X509CertificateParser xparse = new Org.BouncyCastle.X509.X509CertificateParser();
             Org.BouncyCastle.X509.X509Certificate xcert = xparse.ReadCertificate(signerCert.GetRawCertData());
             Org.BouncyCastle.X509.X509Certificate xcaCert = xparse.ReadCertificate(certE.GetRawCertData());



             string etugraurl = "http://ocsp.e-tugra.com.tr/";
             string turktrust = "http://ocsp.turktrust.com.tr/";
             string eguvenurl = "http://ocsp2.e-guven.com/";
             string tubitak = "http://ocsp3.kamusm.gov.tr/";
             string url = "";


             string eshs = signerCert.GetIssuerName();
             if (eshs.Contains("EBG"))
                 url = etugraurl;
             else if (eshs.Contains("e-Guven"))
                 url = eguvenurl;
             else if (eshs.Contains("TÜRKTRUST"))
                 url = turktrust;
             else
                 url = tubitak;

             string snc = ocsp.CheckCertificate(xcert, xcaCert, url);


             if (snc == "REVOKED" || snc == "UNKNOWN" )
             {

                 return "Sertifika Doğrulanamadı";
             }

             else if (snc == "HATA")
             {

                 return "Sertifika Doğrulanama Hata Oluştu.";

             }

         

            //END OCSP
             * */

            byte[] encodedSignedCms = SignMsg(msgBytes, signerCert);
            string result = Convert.ToBase64String(encodedSignedCms);

            if (!VerifyMsg(msgBytes, encodedSignedCms))
            {
                throw new Exception("Mesaj Doğrulanamadı");
            }
            return result;
        }

        static public byte[] SignMsg(Byte[] msg, X509Certificate2 signerCert)
        {
            ContentInfo contentInfo = new ContentInfo(msg);

            SignedCms signedCms = new SignedCms(contentInfo, false);
            CmsSigner cmsSigner = new CmsSigner(signerCert);

            TLVStruct issuerGeneralNames;
            issuerGeneralNames.pbData = signerCert.IssuerName.RawData;
            issuerGeneralNames.cbData = issuerGeneralNames.pbData.Length;
            TLVAddTag(ref issuerGeneralNames, 0xa4);
            TLVAddTag(ref issuerGeneralNames, 0x30);

            TLVStruct serial;
            serial.pbData = signerCert.GetSerialNumber();
            serial.pbData = serial.pbData.Reverse().ToArray();
            //Array.Reverse(serial.pbData);
            serial.cbData = serial.pbData.Length;
            TLVAddTag(ref serial, 0x02);

            TLVStruct issuerSerial = MakeTLV(ref issuerGeneralNames, ref serial);

            TLVStruct hash;
            hash.pbData = signerCert.GetCertHash();
            hash.cbData = hash.pbData.Length;
            TLVAddTag(ref hash, 0x04);

            TLVStruct eSSCertID = MakeTLV(ref hash, ref issuerSerial);
            TLVAddTag(ref eSSCertID, 0x30);
            TLVAddTag(ref eSSCertID, 0x30);

            AsnEncodedData asndata = new AsnEncodedData("1.2.840.113549.1.9.16.2.12", eSSCertID.pbData);

            Pkcs9AttributeObject o = new Pkcs9AttributeObject(asndata);
            cmsSigner.SignedAttributes.Add(o);

            cmsSigner.SignerIdentifierType = SubjectIdentifierType.SubjectKeyIdentifier;

            cmsSigner.IncludeOption = X509IncludeOption.EndCertOnly;
            try
            {
                signedCms.ComputeSignature(cmsSigner, false);
            }
            catch (Exception e)
            {
                throw new Exception("İmzalama Sırasında '" + e.Message + "' hatası oluştu", e);
            }

            return signedCms.Encode();
        }
        static public bool VerifyMsg(byte[] msgBytes, byte[] encodedSignedCms)
        {
            ContentInfo contentInfo = new ContentInfo(msgBytes);
            SignedCms signedCms = new SignedCms(contentInfo, false);
            signedCms.Decode(encodedSignedCms);
            try
            {
                signedCms.CheckSignature(true);
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                throw new Exception("Sertifika Bulunamadı");
            }
            return true;
        }
        static public string PlainText(byte[] encodedSignedCms)
        {


            try
            {

                SignedCms signedCms = new SignedCms();

                signedCms.Decode(encodedSignedCms);

                if (signedCms.Detached)
                    throw new InvalidOperationException("İmza Açılamıyor");

                byte[] decodedSignedCms = signedCms.ContentInfo.Content;
                MemoryStream ms = new MemoryStream(decodedSignedCms);
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                return sr.ReadToEnd();



            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return e.Message;
            }
            return null;
        }
        static public string CertificatesInfo(byte[] encodedSignedCms)
        {

            string info;

            try
            {
                string TCKimlikNo = "";
             
                //signedCms.Decode(encodedSignedCms);
                //signedCms.CheckSignature(true);


                SignedCms signedCmss = new SignedCms();
                signedCmss.Decode(encodedSignedCms);
                X509Certificate2 signerCert = new X509Certificate2(); ;

                if (signedCmss.Certificates.Count > 0)
                {

                    for (int i = 0; i < signedCmss.Certificates.Count; i++)
                    {
                        if (signedCmss.Certificates[i].Subject.Contains("SERIALNUMBER"))
                            signerCert = signedCmss.Certificates[i];
                    }
                }



                string fortckml = signerCert.Subject.ToString();

                info = "[Subject]\n" + signerCert.Subject.ToString();
                info += "\n[Issuer]" + signerCert.Issuer.ToString();
                info += "\n[Serial Number]\n" + signerCert.SerialNumber.ToString();
                info += "\n[Not Before]\n" + signerCert.NotBefore.ToString();
                info += "\n[Not After]\n" + signerCert.NotAfter.ToString();
                info += "\n[Thumbprint]\n" + signerCert.Thumbprint.ToString();
                info += "\n[Versiyon]\n" + signerCert.Version.ToString();
                
                
                //string[] pipes = fortckml.Split(',');
                //if (pipes.Length > 0)
                //{
                //    for (int i = 0; i < pipes.Length; i++)
                //    {

                //        if (pipes[i].Trim().Contains("OID"))
                //        {
                //            TCKimlikNo = pipes[i].Substring(pipes[i].IndexOf("=") + 1, pipes[i].ToString().Length - pipes[i].IndexOf("=") - 1);

                //        }
                //    }
                //}

                //if (TCKimlikNo == "")
                //{
                //    info = "[Subject]\n" + signedCms.Certificates[1].Subject.ToString();
                //    info += "\n[Issuer]" + signedCms.Certificates[1].Issuer.ToString();
                //    info += "\n[Serial Number]\n" + signedCms.Certificates[1].SerialNumber.ToString();
                //    info += "\n[Not Before]\n" + signedCms.Certificates[1].NotBefore.ToString();
                //    info += "\n[Not After]\n" + signedCms.Certificates[1].NotAfter.ToString();
                //    info += "\n[Thumbprint]\n" + signedCms.Certificates[1].Thumbprint.ToString();

                //    fortckml = signedCms.Certificates[1].Subject.ToString();
                //    pipes = fortckml.Split(',');
                //    if (pipes.Length > 0)
                //    {
                //        for (int i = 0; i < pipes.Length; i++)
                //        {

                //            if (pipes[i].Trim().Contains("OID"))
                //            {
                //                TCKimlikNo = pipes[i].Substring(pipes[i].IndexOf("=") + 1, pipes[i].ToString().Length - pipes[i].IndexOf("=") - 1);

                //            }
                //        }
                //    }

                //    info += "\n[TC Kimlik No]\n" + TCKimlikNo;
                //}
                //else
                //{
                    //info = "[Subject]\n" + signedCms.Certificates[0].Subject.ToString();
                    //info += "\n[Issuer]" + signedCms.Certificates[0].Issuer.ToString();
                    //info += "\n[Serial Number]\n" + signedCms.Certificates[0].SerialNumber.ToString();
                    //info += "\n[Not Before]\n" + signedCms.Certificates[0].NotBefore.ToString();
                    //info += "\n[Not After]\n" + signedCms.Certificates[0].NotAfter.ToString();
                    //info += "\n[Thumbprint]\n" + signedCms.Certificates[0].Thumbprint.ToString();
                    //info += "\n[Versiyon]\n" + signedCms.Certificates[0].Version.ToString();


               // }

                return info;

            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return e.Message;
            }
            return null;
        }
        static public bool CertificatesControl(byte[] encodedSignedCms)
        {
            try
            {


                SignedCms signedCms = new SignedCms();
                signedCms.Decode(encodedSignedCms);
                signedCms.CheckSignature(true);
                X509Certificate2 cert = signedCms.Certificates[signedCms.Certificates.Count - 1];

                bool chainIsValid = false;
                X509Chain chain = new X509Chain();
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                //chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
                chainIsValid = chain.Build(cert);
                return chainIsValid;





            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return false;
            }

        }
        static public X509Certificate2 GetSignerCert(string signerserialnumber)
        {
            X509Certificate2 secilensertifika = null;
            X509Store storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certColl = storeMy.Certificates.Find(X509FindType.FindBySerialNumber, signerserialnumber, false);
            bool sertifikabulundu = (certColl.Count > 0);
            if (sertifikabulundu)
            {
                secilensertifika = certColl[0];
            }
            else
            {
                X509Certificate2Collection collection = (X509Certificate2Collection)storeMy.Certificates;
                X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Sertifika Seçimi", "Lütfen Hangi Sertifika ile imzalayacağınızı seçiniz", X509SelectionFlag.SingleSelection);
                if (scollection.Count > 0)
                {
                    secilensertifika = scollection[0];
                    sertifikabulundu = true;
                }
            }
            if (!sertifikabulundu)
            {
                throw new Exception("Sertifika Bulunamadı");
            }
            storeMy.Close();
            return secilensertifika;
        }
    }
}


