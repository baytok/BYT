export class AppConsts {

    static remoteServiceBaseUrl: "https://localhost:44345/api/BYT/";
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish

    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly localization = {
        defaultLocalizationSourceName: 'BYT'
    };

    static readonly authorization = {
        encryptedAuthTokenName: 'enc_auth_token'
    };
}
