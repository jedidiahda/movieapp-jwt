import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @ViewChild('registerForm')
  registerForm!: NgForm;
  errorMsg: string = '';

  constructor(
    public authService: AuthenticationService,
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.registerForm.setValue({
        firstName: '',
        lastName: '',
        gender: '',
        address: '',
        email: '',
        password: '',
        repeatPassword: '',
      });
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.authService
      .register(this.registerForm.value)
      .catch((err) => (this.errorMsg = err));

      this.createCustomer();
    }
  }

  createCustomer(){
    console.log(this.registerForm.value)
    this.customerService
    .createOne(this.registerForm.value)
    .then((customer) => this.router.navigate(['/login']))
    .catch((err) => console.log(err));
  }
}
