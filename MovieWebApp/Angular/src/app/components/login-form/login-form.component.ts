import {
  FacebookLoginProvider,
  GoogleLoginProvider,
  SocialAuthService,
} from '@abacritt/angularx-social-login';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ExternalAuthDto } from 'src/app/models/ExternalAuthDto';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css'],
})
export class LoginFormComponent implements OnInit {
  @ViewChild('loginForm')
  loginForm!: NgForm;
  errorMsg: string = '';

  user: any;

  constructor(
    private router: Router,
    //private readonly _authService: SocialAuthService,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.loginForm.setValue({
        email: '',
        password: '',
      });
    });

    this.authService.externalAuthService.authState.subscribe((user) => {
      this.user = user;
      if (user) {
        this.externalLogin(user);
      }
    });
  }

  onSubmit(): void {
    this.authService
      .login(this.loginForm.value)
      .then((response) => {
        if(response.result.token != undefined){
          this.authService.setToken(response.result.token);
          this.router.navigated = false;
          this.router.navigate(['/home']);
        }else{
          this.errorMsg = response.message;
        }
      })
      .catch((err) => console.log(err));
  }

  onRegisterClick(): void {
    this.router.navigate(['/register']);
  }

  signInWithGoogle(): void {
    this.authService.externalAuthService.signIn(
      GoogleLoginProvider.PROVIDER_ID
    );
  }
  signInWithFB(): void {
    this.authService.externalAuthService.signIn(
      FacebookLoginProvider.PROVIDER_ID
    );
  }

  refreshGoogleToken(): void {
    this.authService.externalAuthService.refreshAuthToken(
      GoogleLoginProvider.PROVIDER_ID
    );
  }

  externalLogin(user: any) {
    this.user = user;
    const externalAuth: ExternalAuthDto = {
      provider: this.user.provider,
      idToken: this.user.idToken ? this.user.idToken : this.user.authToken,
      firstName: this.user.firstName,
      lastName: this.user.lastName,
      email: this.user.email,
      photoUrl: this.user.photoUrl,
    };
    this.authService.externalLogin(externalAuth).subscribe({
      next: (res) => {
        this.authService.setToken(res.token);
        this.router.navigated = false;
        this.router.navigate(['/home']);
      },
      error: (err: HttpErrorResponse) => {
        console.log(err);
        this.authService.externalAuthService.signOut();
      },
    });
  }
}
