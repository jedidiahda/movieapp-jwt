import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ExternalAuthDto } from 'src/app/models/ExternalAuthDto';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {
  @ViewChild('loginForm')
  loginForm!:NgForm;
  errorMsg:string='';

  constructor(private router:Router,
    private authService:AuthenticationService) { 
    }

  ngOnInit(): void {
    setTimeout(()=>{
      this.loginForm.setValue({
        email:'',
        password:''
      })
    });


  }

  onSubmit():void{
    this.authService.login(this.loginForm.value).then(response =>{
      this.authService.setToken(response.token);
      this.authService.sendAuthStateChangeNotification(true);
      this.router.navigate(['/home']);
    }).catch(err => console.log(err));
  }

  onRegisterClick():void{
    this.router.navigate(['/register']);
  }
}
