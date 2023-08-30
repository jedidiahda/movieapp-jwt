import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Credentials } from '../models/credentials.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { GoogleLoginProvider,FacebookLoginProvider  } from '@abacritt/angularx-social-login';
import { Subject } from 'rxjs';
import { ExternalAuthDto } from '../models/ExternalAuthDto';
import { AuthResponseDto } from '../models/AuthResponseDto';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  basedUrl: string = environment.basedUrl;
  jwtDecode: any;
  #authError!: string;
  public isExternalAuth: boolean = false;


  constructor(
    private http: HttpClient,
    private jwtService: JwtHelperService,
    public externalAuthService: SocialAuthService,
    private router:Router
  ) {
    //this.jwtDecode = this.jwtService.decodeToken(this.getToken());
  }

  register(user: Credentials): Promise<Credentials | undefined> {
    user.role = 'user';
    return this.http
      .post<Credentials>(`${this.basedUrl}/Account`, user)
      .toPromise();
  }

  login(user: Credentials): Promise<any> {
    this.isExternalAuth = false;
    return this.http
      .post<any>(`${this.basedUrl}/Account/login`, user)
      .toPromise();
  }

  logout() {
    localStorage.removeItem(environment.userTokenName);
    
    if(this.isExternalAuth == true){
      this.signOutExternal();
    }
    
    this.router.navigate(['/home']);
  }

  setToken(token: any) {
    localStorage.setItem(environment.userTokenName, token);
  }

  getToken(): any {
    return localStorage.getItem(environment.userTokenName);
  }

  get name(): string {
    this.jwtDecode = this.jwtService.decodeToken(this.getToken());
    return this.jwtDecode ? this.jwtDecode.Name : '';
  }

  get customerId(): string {
    this.jwtDecode = this.jwtService.decodeToken(this.getToken());

    return this.jwtDecode ? this.jwtDecode.CustomerId : '';
  }

  public get userRole(): string {
    this.jwtDecode = this.jwtService.decodeToken(this.getToken());
    return this.jwtDecode ? this.jwtDecode.Role : '';
  }

  get isLoggedIn(): boolean {
    let token = this.getToken();
    return token == null || token == '' ? false : true;
  }

  get authMessage(): string {
    return this.#authError;
  }

  set authMessage(err: string) {
    this.#authError = err;
  }

  public signInWithGoogle = () => {
    this.externalAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  };
  public signOutExternal = () => {
    this.externalAuthService.signOut();
  };

  public externalLogin = (body: ExternalAuthDto) => {
    this.isExternalAuth = true;
    return this.http.post<AuthResponseDto>(
      `${this.basedUrl}/Account/ExternalLogin`,
      body
    );
  };

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");
 
    return !this.jwtService.isTokenExpired(token);
  }

}
