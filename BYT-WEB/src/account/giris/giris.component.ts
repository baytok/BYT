import { Component, OnInit,InjectionToken,Inject } from '@angular/core';
import { AppSessionService } from '../../shared/session/app-session.service';
import { GirisService } from './giris.service';
import { Router } from "@angular/router";
import {AppServisDurumKodlari} from '../../shared/AppEnums';
import { KullaniciModel, KullaniciSonucModel, } from './giris-service-proxies';
import { BeyannameServiceProxy ,SessionServiceProxy} from '../../shared/service-proxies/service-proxies';
import {
  KullaniciServisDto
 } from '../../shared/service-proxies/service-proxies';
 import { MatSnackBar } from "@angular/material/snack-bar";
 import { accountModuleAnimation } from '../../shared/animations/routerTransition'; 

 declare const pwsigner: any;

@Component({
  selector: 'app-giris',
  animations: [accountModuleAnimation()],
  templateUrl: './giris.component.html',
  styleUrls: ['./giris.component.scss']
})
export class GirisComponent implements OnInit {
  ErrorMessages: any[] = [];
  submitting = false;
  tcAdSoyad:string;
  eimzaGiris: boolean = true;
  public cadesSignature: any;
  public selectedCardReader: any;
  public selectedCertificate: any;
  public pinCode: any;
    //Kart Okuyucu
    public cardReaders: any[] = []

    //Sertifika
    public certificates: any[] = [];
    // dataToBeSigned= " İşbu oturum taahhütnamesi ile şahsıma ait elektronik imzamı kullanarak oturum başlattığımı ve şahsıma ait kimlik kartı sertifikamı kullanarak bu metni imzaladığımı taahhüt ederim." + new Date().toLocaleDateString() + new Date().toLocaleTimeString() ;
    dataToBeSigned="test verisi";
    initialize = () => {
    console.log('calling initialized');
    pwsigner.setLicenseKey("uq8SKpkdRkWW8GzUVQROWbz6nIzfKQbYj513yUSrsiLGDglpixP6n75BWYTzPQDbq96zur6eTE0twVHYGUbPlNnclr3LiEZuWzNFjvjk1FE9SM6fxjqeTw9ajnM76an2MYpCh4gSackOU9FCc+LqsX9BakArK40zuCZCdD2OLgIFeLTnpsAMhhLru68uTCWP2WPB/Th69vNsVIZYOXPfd+A2QMSVFSQlS8gMPVoCx7PdcuwFpx8QI8GJDNApjDrleypH3m4leKT8ly7sf3owt06o9y3NVUxf18vISkJ+HQ06WFKzQN2I3pfYNzRpTW8KUNX/TLfR3+tFeVuicPbWAg==");
    pwsigner.initialize(this.onArkSignerInitialized);
  };
  constructor(
    private _UserSession: AppSessionService,
    public   girisService: GirisService,     
    private router:Router,
    private snackBar: MatSnackBar,
    private _beyanSession: SessionServiceProxy,

   
     ) { 
      this.ErrorMessages[pwsigner.codes.RESPONSE_SUCCESSFUL] = 'İşlem başarı ile gerçekleştirilmiştir.';
      this.ErrorMessages[pwsigner.codes.ERROR_NO_CLIENT] = 'PW Signer Client Uygulaması Bulunamamıştır. Bilgisayarınıza İndirmek İster misiniz?';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_INITIALIZE_SIGNER] = 'PW Signer Client Uygulaması Başlatılamamıştır.';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_LOAD_LICENSE] = 'PW Signer Client Lisansı Geçersizdir.';
      this.ErrorMessages[pwsigner.codes.ERROR_UNDEFINED_ACTION] = 'Desteklenmeyen işlem';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_START_PWSIGNER_CLIENT] = 'PW Signer Client Uygulaması Başlatılamamıştır.';
      this.ErrorMessages[pwsigner.codes.ERROR_INVALID_CERTIFICATE] = 'Kullanmak istediğiniz sertifika geçersizdir.';
      this.ErrorMessages[pwsigner.codes.ERROR_INVALID_CERTIFICATE_SERIAL_NUMBER] = 'Kullanmak istediğiniz sertifikanın seri numarası geçersizdir.';
      this.ErrorMessages[pwsigner.codes.ERROR_NO_SIGNING_CERTIFICATE_FOUND] = 'Herhangi bir imzalama sertifikası bulunamamıştır.';
      this.ErrorMessages[pwsigner.codes.ERROR_NO_TERMINAL_FOUND] = 'Takılı herhangi bir akıllı kart bulunamamıştır.';
      this.ErrorMessages[pwsigner.codes.ERROR_NO_TERMINAL_FOR_PROVIDED_SLOT_ID_AND_CARD_TYPE] = 'Geçersiz slot numarası ve kart tipi.';
      this.ErrorMessages[pwsigner.codes.ERROR_SIGNER_IS_NOT_INITIALIZED] = 'PW Signer Client uygulaması başlatılmamıştır.';
      this.ErrorMessages[pwsigner.codes.ERROR_SMART_CARD_EXCEPTION] = 'Akıllı kart uygulaması hatası.';
      this.ErrorMessages[pwsigner.codes.ERROR_UNDEFINED_SIGNER_EXCEPTION] = 'Tanımsız hata. Site yöneticisi ile iletişime geçiniz.';
  
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_LOAD_POLICY_FILE] = 'e-imza politikası dosyası yüklenememektedir.';
  
      this.ErrorMessages[pwsigner.codes.ERROR_INVALID_CMS_CONTENT_TO_SIGN] = 'İmzalanmak istenen veri geçersizdir.';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_SET_SIGNING_TIME] = 'İmzalama zamanı set edilememektedir.';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_LOGIN_TO_THE_SIGNER] = 'Akıllı karta login olunamamaktadır.';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_SIGN_CMS_DOCUMENT] = 'Veri imzalanamamaktadır.';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_LOGOUT_SIGNER] = 'Akıllı karttan logout olunamamaktadır.';
      this.ErrorMessages[pwsigner.codes.ERROR_CANNOT_VALIDATE_CERTIFICATE] = 'İmzalama sertifikası doğrulanamamaktadır.';
  
      this.ErrorMessages[pwsigner.codes.ERROR_CERTIFICATE_REVOCATION_CHECK_FAILURE] = 'İmzalama sertifikası geçerliliği kontrol edilememektedir.';
      this.ErrorMessages[pwsigner.codes.ERROR_CERTIFICATE_SELF_CHECK_FAILURE] = 'İmzalama sertifikası geçerliliği kontrol edilememektedir.';
      this.ErrorMessages[pwsigner.codes.ERROR_CERTIFICATE_NO_TRUSTED_CERT_FOUND] = 'Akıllı Kart Üzerinde Geçerli İmzalama Sertifikası Bulunmamaktadır.';
      this.ErrorMessages[pwsigner.codes.ERROR_CERTIFICATE_PATH_VALIDATION_FAILURE] = 'İmzalama sertifikası zinciri doğrulanamamaktadır.';
      this.ErrorMessages[pwsigner.codes.ERROR_CERTIFICATE_NOT_CHECKED] = 'İmzalama sertifikası geçerliliği kontrol edilememektedir.';
      this.ErrorMessages[pwsigner.codes.ERROR_INVALID_URL] = 'Lisans ile farklı bir adres üzerinden uygulamayı çalıştırmaya çalışıyorsunuz.';
      this.ErrorMessages[pwsigner.codes.ERROR_INVALID_PIN] = 'Hatalı şifre girdiniz.';
      this.ErrorMessages[pwsigner.codes.ERROR_NO_DRIVER_FOUND] = 'Yüklü driver bulunamamıştır.';
      this.ErrorMessages[pwsigner.codes.ERROR_NO_EXTENSION] = 'You are currently using Chrome Browser. The PW WebSigner Extension is not installed yet. If you would like to sign documents, please install the extension first. If you confirm, a new tab with the installation page will be opened.';
  
  
      this.ErrorMessages[pwsigner.codes.ERROR_NOT_BROWSER_CHROME] = 'Lütfen Chrome tarayıcı kullanınız.';
      this.ErrorMessages[pwsigner.codes.ERROR_DISCONNECT] = '';
      this.ErrorMessages[pwsigner.codes.ERROR_UNDEFINED] = 'Tanımsız Hata. Lütfen web sitesi yöneticileri ile iletişime geçiniz.';
      this.ErrorMessages[pwsigner.codes.ERROR_PIN_LOCKED] = 'PIN bloke edilmiş. Kullanmak için blokeyi kaldırınız.';
      this.ErrorMessages[pwsigner.codes.ERROR_TASK_TIMEOUT] = 'İşlem zaman aşımına uğramıştır.';
  
      this.ErrorMessages[pwsigner.codes.ERROR_NO_PRIVATE_KEY_FOUND_CORRESPONDING_TO_PUBLIC_KEY] = 'Seçili Sertifika ile ilişkilendirilmiş imzalama anahtarı bulunamamıştır. Sistem Yetkilisi ile iletişime geçiniz.';
   
     }

  ngOnInit() {

  this.initialize();
      
  }
  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000
    });
  }

    
  login() {
    console.log(this.girisService.kullaniciModel.kullanici);    
    this.submitting = true;   
    const promise = this.girisService
    .getKullaniciGiris(this.girisService.kullaniciModel.kullanici,this.girisService.kullaniciModel.sifre)
    .toPromise();
  promise.then(
    result => {
  
      const servisSonuc = new KullaniciServisDto();
      servisSonuc.init(result);
      
      if (servisSonuc.ServisDurumKodu===AppServisDurumKodlari.Available ) {   
          var token = JSON.parse(servisSonuc.Sonuc).Token;
          this._beyanSession.token=token;
          var kullaniciKod=JSON.parse(servisSonuc.Sonuc).KullaniciKod;
          var kullaniciAdi=JSON.parse(servisSonuc.Sonuc).KullaniciAdi;
          var yetkiler= JSON.parse(servisSonuc.Sonuc).Yetkiler;

        this.girisService.setLoginInfo(kullaniciKod,token,kullaniciAdi,yetkiler);
        this.router.navigateByUrl('/app');
      }
      else 
      {  
         this.openSnackBar(servisSonuc.Sonuc , "Tamam");
      }
    },
    err => {
      console.log(err);
    }
  );
   
  }

  onArkSignerInitialized = (code: any, json: any) => {
    console.log('method onArkSignerInitialized');
    //check if the task is executed successfully
    if (code == pwsigner.codes.RESPONSE_SUCCESSFUL) {
      pwsigner.smartCard.listTerminals(this.onArkSignerListTerminals, {});
    } else {
      // $('#DataContainer').unmask();
      this.processErrorCode(code, json);
    }
  }

  onArkSignerListTerminals = (code: any, json: any) => {
    console.log('method onArkSignerListTerminals');
    if (code == pwsigner.codes.RESPONSE_SUCCESSFUL) {
      var terminals = pwsigner.parseJSON(json);
      console.log(terminals);

      this.cardReaders = [];

      for (var i = 0; i < terminals.length; i++) {
        var o = {
          terminal: atob(terminals[i].terminal),
          slotId: atob(terminals[i].slotId),
          library: atob(terminals[i].library),
          smartCardType: atob(terminals[i].smartCardType)
        }

        this.cardReaders.push(o);
      }

      if (this.cardReaders.length > 0) {
        // Set the selected card reader
        this.selectedCardReader = this.cardReaders[0];
        // $scope.$apply();

        pwsigner.smartCard.listCertificates(this.onArkSignerListCertificates, {
          library: this.selectedCardReader.library,
          slotId: '' + this.selectedCardReader.slotId + ''
        });
      }
    }
    else {
      //$('#DataContainer').unmask();
      this.processErrorCode(code, json);
    }
  }

  selectedCardReaderChanged = (item: any) => {
    console.log('selected item:', item);
    pwsigner.smartCard.listCertificates(this.onArkSignerListCertificates, {
      library: item.library,
      slotId: '' + item.slotId + ''
    });
  };

  onArkSignerListCertificates = (code: any, json: any) => {
    console.log('method onArkSignerListCertificates');
    console.log('code', code);
    console.log('json', json);
    if (code == pwsigner.codes.RESPONSE_SUCCESSFUL) {
      var certs = pwsigner.parseJSON(json);
      console.log(certs);

      this.certificates = [];

      for (var i = 0; i < certs.length; i++) {
        var o = {
          commonName: atob(certs[i].commonName),
          serialNumber: atob(certs[i].serialNumber),
          certificateBase64: btoa(certs[i].certificateHex)
        };

        this.certificates.push(o);
      }

      // Set the selected card reader
      this.selectedCertificate = this.certificates[0];
      //  $scope.$apply();
    }
    else {
      //$('#DataContainer').unmask();
      this.processErrorCode(code, json);
    }
  }

  signCAdES_BES_Attached = () => {
    try {
      pwsigner.cades.sign(this.onArkSigner_SignCADES_BES_Attached, {
        library: this.selectedCardReader.library,
        slotId: '' + this.selectedCardReader.slotId + '',
        certSerialNumber: this.selectedCertificate.serialNumber,
        dataBase64: btoa(this.dataToBeSigned),
        pincode: this.pinCode,
        isAttached: true,
        addSigningTime: true
      });
    } catch (err) {
      //$('#DataContainer').unmask();
      console.log(err);
    }
  }

  onArkSigner_SignCADES_BES_Attached = (code: any, json: any) => {
    console.log('method onArkSignerListCertificates');
    console.log('code', code);
    console.log('json', json);
    if (code == pwsigner.codes.RESPONSE_SUCCESSFUL) {
      var signature = pwsigner.parseJSON(json);
      this.cadesSignature = signature[0].signature;
      //    $scope.$apply();
    } else {
      //$('#DataContainer').unmask();
      this.processErrorCode(code, json);
    }
  }

  processErrorCode = (code: any, json: any) => {
    var text = this.ErrorMessages[code];
    if (!text) {
      text = 'Tanımsız hata: ' + code;
    }
    alert(text);
  }

  hexToBase64 = (str: any) => {
    return btoa(String.fromCharCode.apply(null,
      str.replace(/\r|\n/g, "").replace(/([\da-fA-F]{2}) ?/g, "0x$1 ").replace(/ +$/, "").split(" "))
    );
  }

  oneimzaLogin(){
    this.eimzaGiris=!this.eimzaGiris;
  }
}
