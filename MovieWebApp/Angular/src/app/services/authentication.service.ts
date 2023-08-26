import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Credentials } from '../models/credentials.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { GoogleLoginProvider } from '@abacritt/angularx-social-login';
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

  private authChangeSub = new Subject<boolean>();
  private extAuthChangeSub = new Subject<SocialUser>();
  public authChanged = this.authChangeSub.asObservable();
  public extAuthChanged = this.extAuthChangeSub.asObservable();

  constructor(
    private http: HttpClient,
    private jwtService: JwtHelperService,
    private externalAuthService: SocialAuthService,
    private router:Router
  ) {
    //console.log('hi auth service')
    this.jwtDecode = this.jwtService.decodeToken(this.getToken());

    this.externalAuthService.authState.subscribe((user) => {
      //console.log(user);
      this.signInWithGoogle();
      this.extAuthChanged.subscribe( user => {
        const externalAuth: ExternalAuthDto = {
          provider: user.provider,
          idToken: user.idToken
        }
        this.validateExternalAuth(externalAuth);
      })
      this.extAuthChangeSub.next(user);
      this.isExternalAuth = true;
    });
  }

  private validateExternalAuth(externalAuth: ExternalAuthDto) {
    this.externalLogin( externalAuth)
      .subscribe({
        next: (res) => {
            //console.log(res.token);
            //localStorage.setItem("token", res.token);
            this.setToken(res.token);
            this.sendAuthStateChangeNotification(res.isAuthSuccessful);
            this.router.navigate(['/home']);
      },
        error: (err: HttpErrorResponse) => {
          // this.errorMessage = err.message;
          // this.showError = true;
          console.log(err);
          this.signOutExternal();
        }
      });
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

    if(this.isExternalAuth)
      this.signOutExternal();

    this.sendAuthStateChangeNotification(false);

    this.router.navigate(['/home']);
  }

  setToken(token: any) {
    localStorage.setItem(environment.userTokenName, token);
  }

  getToken(): any {
    return localStorage.getItem(environment.userTokenName);
  }

  get name(): string {
    return this.jwtDecode ? this.jwtDecode.Name : '';
  }

  get customerId(): string {
    let jwtDecode = this.jwtService.decodeToken(this.getToken());
    //console.log(jwtDecode)
    return jwtDecode ? jwtDecode.CustomerId : '';
  }

  public get userRole(): string {
    //console.log(this.jwtDecode)
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
    return this.http.post<AuthResponseDto>(
      `${this.basedUrl}/Account/ExternalLogin`,
      body
    );
  };

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");
 
    return !this.jwtService.isTokenExpired(token);
  }

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }
}
