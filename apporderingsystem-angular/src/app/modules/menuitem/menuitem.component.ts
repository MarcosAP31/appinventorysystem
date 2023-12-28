import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'src/app/models/menuitem';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { StoreService } from 'src/app/service/store.service';
import { Request, Response } from 'express'

import Swal from 'sweetalert2';
import { DatePipe } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-menuitem',
  templateUrl: './menuitem.component.html',
  styleUrls: ['./menuitem.component.css']
})
export class MenuItemComponent implements OnInit {
  // Propiedades
  image: any = "../../../assets/upload.png";
  show: boolean = false;
  title = 'fileUpload';
  images = '';
  imgURL = '/assets/noimage.png';
  multipleImages = [];
  imagenes: any = [];
  file: any;
  previsualizacion: any;
  pipe = new DatePipe('en-US');
  todayWithPipe: any;
  formMenuItem: FormGroup;
  menuitems: any;
  menucategories: any;
  suppliers: any;
  ubications: any;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  creating = true;
  noValido = true;
  id = 0;

  constructor(
    private http: HttpClient,
    private sanitizer: DomSanitizer,
    public form: FormBuilder,
    private storeService: StoreService
  ) {
    // Inicializar formulario
    this.formMenuItem = this.form.group({
      Name: [''],
      Description: [''],
      CategoryId: [''],
      Price: [''],
      Image: ['']
    });
  }

  // Método para mostrar un cuadro de carga
  isLoading() {
    Swal.fire({
      allowOutsideClick: false,
      width: '200px',
      text: 'Cargando...',
    });
    Swal.showLoading();
  }

  // Método para detener el cuadro de carga
  stopLoading() {
    Swal.close();
  }

  // Método para obtener los menuitemos y proveedores
  get() {
    this.storeService.getMenuItems().subscribe(response => {
      this.menuitems = response;
      this.dtTrigger.next(0);
    });
    this.storeService.getMenuCategories().subscribe(response => {
      this.menucategories = response;
    });
  }

  ngOnInit(): void {
    // Configuraciones de DataTables
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true
    };
    this.get();
    this.todayWithPipe = this.pipe.transform(Date.now(), 'dd/MM/yyyy  h:mm:ss a');
  }

  // Método para seleccionar una imagen
  selectImage(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (event: any) => {
        this.imgURL = event.target.result;
      }
      this.images = file;
    }
    this.show = true;
  }

  // Método para seleccionar múltiples imágenes
  selectMultipleImage(event: any) {
    if (event.target.files.length > 0) {
      this.multipleImages = event.target.files;
    }
  }

  // Método para mostrar las imágenes
  mostrarImg() {
    this.http.get<any>('http://localhost:3000/apiorderingsystem/upload').subscribe(res => {
      this.imagenes = res;
      const reader = new FileReader();
      reader.onload = (this.imagenes);
      console.log(this.imagenes);
    });
  }

  // Método para editar un menuitemo
  edit(id: any) {
    this.creating = false;
    //this.formMenuItem.get('Amount')?.disable();
    this.storeService.getMenuItem(id).subscribe((response: any) => {
      this.id = response.ItemId;
      this.formMenuItem.setValue({
        Name: response.Name,
        Description: response.Description,
        CategoryId: response.CategoryId,
        Price: response.Price,
        Image: ''
      });
    });
    this.formMenuItem = this.form.group({
      Name: [''],
      Description: [''],
      CategoryId: [''],
      Price: [''],
      Image: ['']
    });
  }

  // Método para eliminar un menuitemo
  delete(id: any) {
    Swal.fire({
      title: 'Confirmación',
      text: '¿Seguro de eliminar el registro?',
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

        this.storeService.getMenuItem(id).subscribe((r: any) => {
          this.storeService.getFileByName(r.Image).subscribe((res: any) => {
            this.http.delete<any>(`http://localhost:3000/apiorderingsystem/file/${res.FileId}`).subscribe(re => {
              console.log(re, location.reload());
            });
          })
        });

        this.storeService.deleteMenuItem(id).subscribe(() => {
          Swal.fire({
            allowOutsideClick: false,
            icon: 'success',
            title: 'Éxito',
            text: '¡Se ha eliminado correctamente!',
          }).then((result) => {
            window.location.reload();
          });
        }, (err: any) => {
          console.log(err);

          if (err.Description == "HttpErrorResponse") {
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
            title: err.Description,
            text: err.message,
          });
        });
      }
    });
  }

  // Método para guardar un menuitemo
  submit() {
    var menuitem = new MenuItem();
    menuitem.CategoryId = this.formMenuItem.value.CategoryId;
    menuitem.Name = this.formMenuItem.value.Name;
    menuitem.Description = this.formMenuItem.value.Description;
    menuitem.Price = this.formMenuItem.value.Price;
    var string = this.formMenuItem.value.Image;
    var splits = string.split("\\", 3);
    menuitem.Image = splits[2];
    var solicitud = this.creating ? this.storeService.insertMenuItem(menuitem) : this.storeService.updateMenuItem(this.id, menuitem);
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

        const formData = new FormData();
        formData.append('file', this.images);
        if (this.creating == false) {
          if (this.formMenuItem.value.Image != '') {
            this.storeService.getMenuItem(this.id).subscribe((re: any) => {
              this.storeService.getFileByName(re.Image).subscribe((res: any) => {
                this.http.delete<any>(`http://localhost:3000/apiorderingsystem/file/${res.FileId}`).subscribe(() => { });
                this.http.post<any>('http://localhost:3000/apiorderingsystem/saveimg', formData).subscribe(() => { });
              })
            });
          }
        } else {
          
          this.http.post<any>('http://localhost:3000/apiorderingsystem/saveimg', formData).subscribe(() => { });
        }


        solicitud.subscribe((r: any) => {
          if (this.creating == true && r.message === 'MenuItem with the same name already exists') {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Error al registrar',
              text: 'Ya existe un producto con ese nombre.',
            });
          }
          Swal.fire({
            allowOutsideClick: false,
            icon: 'success',
            title: 'Éxito',
            text: 'Se ha guardado correctamente!',
          }).then((result) => {
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
    this.formMenuItem = this.form.group({
      Name: [''],
      Description: [''],
      Category: [''],
      Price: [''],
      Image: ['']
    });
    this.show = false;
  }
}
