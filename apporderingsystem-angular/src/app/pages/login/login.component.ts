import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { StoreService } from 'src/app/service/store.service';
import { DatePipe } from '@angular/common';
import Swal from 'sweetalert2';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  show: boolean = false;
  title = 'fileUpload';
  formLogin: FormGroup;
  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    public form: FormBuilder,
    private storeService: StoreService,
    private router: Router
  ) {
    // Inicializar el formulario
    this.formLogin = this.form.group({
      email: [''],
      password: ['']
    });
  }
  // Método para mostrar el mensaje de carga
  isLoading() {
    Swal.fire({
      allowOutsideClick: false,
      width: '200px',
      text: 'Cargando...',
    });
    Swal.showLoading();
  }

  // Método para ocultar el mensaje de carga
  stopLoading() {
    Swal.close();
  }
  ngOnInit(): void {

    // Redireccionar al inicio si ya hay un token de acceso en las cookies
    if (this.cookieService.check('token_access')==false) {
      this.router.navigateByUrl('login');
    }

    
    /* Comentar este bloque temporalmente
    if (this.storeService.estaAutenticado()) {
      this.router.navigateByUrl("inicio");
      var r = this.storeService.obtenerUserLogeado();
      if (r.administrador) {
        this.router.navigate(['/mantenimiento/empresa']);
      } else if (r.supervisor) {
        this.router.navigate(['/inventario/apertura']);
      } else {
        this.router.navigate(['/inventario/toma']);
      }
    }
    */
  }
  
  /*redirectFromHash() {
    if (window.location.hash && window.location.hash === '#/') {
      window.location.replace(window.location.href.replace('#/', ''));
    }
  }*/
  submit() {
    // Create a new user with the form data
    const user = new User();
    user.email = this.formLogin.value.email;
    user.password = this.formLogin.value.password;
  
    // Show a loading message while authentication is in progress
  
    // Call the authentication service
    this.storeService.login(user).subscribe(
      (token: any) => {
        console.log(token)
        if (token == null) {
          // If authentication fails, display an error message and close the loading message
          Swal.fire({
            allowOutsideClick: false,
            icon: 'info',
            title: 'Authentication Error',
            text: 'Incorrect email or password',
          });
        } else {
          // If authentication succeeds, store the access token and perform additional verifications
          Swal.fire({
            allowOutsideClick: false,
            icon: 'info',
            title: 'Login',
            text: 'Logging in...',
          });
          Swal.showLoading();
  
          // Store the token in local storage
          localStorage.setItem('token', token);
  
          // Verify the token on the server
          this.storeService.validateLogin(token).subscribe(
            () => {
              this.cookieService.set('token_access', token);
              // Get user information using the authenticated email
              this.storeService.getUserByEmail(this.formLogin.value.email,token).subscribe(
                (res: any) => {
                  // Store user information in local storage
                  localStorage.setItem('name', res.name);
                  console.log(res);
                  localStorage.setItem('userId', res.userId);
                },
                (error: any) => {
                  console.error('Error fetching user information:', error);
                }
              );
  
              // Navigate to the 'inicio' page upon successful login
              this.router.navigateByUrl('inicio');
            },
            (error: any) => {
              console.error('Token verification failed:', error);
              Swal.fire({
                allowOutsideClick: false,
                icon: 'info',
                title: 'Verification Error',
                text: 'Token verification failed',
              });
            }
          );
  
          // Close the loading message
          Swal.close();
        }
      },
      (error: any) => {
        console.error('Authentication failed:', error);
        Swal.fire({
          allowOutsideClick: false,
          icon: 'info',
          title: 'Authentication Error',
          text: 'Authentication request failed',
        });
      }
    );
  }

}
