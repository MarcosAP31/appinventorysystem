import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { User } from 'src/app/models/user';
import { FormGroup, FormBuilder } from '@angular/forms';
import { StoreService } from 'src/app/service/store.service';

import Swal from 'sweetalert2';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  show: boolean = false;
  formUser: FormGroup;
  users: any;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  creating = true;
  noValido = true;
  user:any;

  constructor(
    public form: FormBuilder,
    private storeService: StoreService
  ) {
    this.formUser = this.form.group({
      Code:[''],
      Name: [''],
      LastName: [''],
      Phone: [''],
      Position: [''],
      Role: [''],
      Email: [''],
      Password: ['']
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

  // Método para obtener los usuarios
  get() {
    this.storeService.getUsers(localStorage.getItem('token')).subscribe(response => {
      this.users = response;
      console.log(this.users);
      this.dtTrigger.next(0);
    });
  }

  ngOnInit(): void {
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true
    };
    this.get();
  }

  // Método para editar un usuario
  edit(userid: any) {
    this.creating = false;
    this.storeService.getUser(userid,localStorage.getItem('token')).subscribe(
      (response: any) => {
        this.user = response;
        this.formUser.setValue({
          Code:response.code,
          Name: response.name,
          LastName: response.lastName,
          Phone: response.phone,
          Position:response.position,
          Role:response.role,
          Email: response.email,
          Password: response.password
        });
        console.log(this.user);
      }
    );
    this.formUser = this.form.group({
      Code:[''],
      Name: [''],
      LastName: [''],
      Phone: [''],
      Position: [''],
      Role: [''],
      Email: [''],
      Password: ['']
    });
  }

  // Método para eliminar un usuario
  delete(id: any) {
    Swal.fire({
      title: 'Confirmación',
      text: 'Seguro de eliminar el registro?',
      showDenyButton: true,
      showCancelButton: false,
      confirmButtonText: `Eliminar`,
      denyButtonText: `Cancelar`,
      allowOutsideClick: false,
      icon: 'info'
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire({
          allowOutsideClick: false,
          icon: 'info',
          title: 'Eliminando registro',
          text: 'Cargando...',
        });
        Swal.showLoading();
        this.storeService.deleteUser(id,localStorage.getItem('token')).subscribe(r => {
          Swal.fire({
            allowOutsideClick: false,
            icon: 'success',
            title: 'Éxito',
            text: 'Se ha eliminado correctamente!',
          }).then((result) => {
            window.location.reload();
          });
        }, err => {
          console.log(err);
          if (err.Name == "HttpErrorResponse") {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Error al conectar',
              text: 'Error de comunicación con el servidor',
            });
            return;
          }
          Swal.fire({
            allowOutsideClick: false,
            icon: 'error',
            title: err.Name,
            text: err.message,
          });
        });
      } else if (result.isDenied) {

      }
    });
  }

  // Método para guardar o actualizar un usuario
  submit() {
    var user = new User();
    if(this.creating==false){
      user.userId=this.user.userId;
    }
    user.code=this.formUser.value.Code;
    user.name = this.formUser.value.Name;
    user.lastName = this.formUser.value.LastName;
    user.phone = this.formUser.value.Phone;
    user.position=this.formUser.value.Position;
    user.role=this.formUser.value.Role;
    user.email = this.formUser.value.Email;
    user.password = this.formUser.value.Password;
    var solicitud = this.creating ? this.storeService.insertUser(user,localStorage.getItem('token')) : this.storeService.updateUser(user,localStorage.getItem('token'));
    Swal.fire({
      title: 'Confirmación',
      text: '¿Seguro de guardar el registro?',
      showDenyButton: true,
      showCancelButton: false,
      confirmButtonText: `Guardar`,
      denyButtonText: `Cancelar`,
      allowOutsideClick: false,
      icon: 'info'
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire({
          allowOutsideClick: false,
          icon: 'info',
          title: 'Guardando registro',
          text: 'Cargando...',
        });
        Swal.showLoading();
        console.log("holaaaa")
        solicitud.subscribe((r: any) => {
          console.log(r)
          if (this.creating == true && r.message === 'User with the same name already exists') {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Error al registrar',
              text: 'Ya existe un usuario con ese nombre.',
            });
          }
          Swal.fire({
            allowOutsideClick: false,
            icon: 'success',
            title: 'Éxito',
            text: 'Se ha guardado correctamente!',
          }).then(() => {
            window.location.reload();
          });

        }, err => {
          console.log(err);

          if (err.name == "HttpErrorResponse") {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Error al conectar',
              text: 'Error de comunicación con el servidor',
            });
            return;
          }
          Swal.fire({
            allowOutsideClick: false,
            icon: 'error',
            title: err.name,
            text: err.message,
          });
        });
      };
    });
  }

  // Método para cerrar el modal
  closeModal() {
    this.formUser = this.form.group({
      Code:[''],
      Name: [''],
      LastName: [''],
      Phone: [''],
      Position: [''],
      Role: [''],
      Email: [''],
      Password: ['']
    });
  }
}
