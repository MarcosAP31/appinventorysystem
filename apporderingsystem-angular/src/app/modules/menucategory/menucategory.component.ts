import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { MenuCategory } from 'src/app/models/menucategory';
import { FormGroup, FormBuilder } from '@angular/forms';
import { StoreService } from 'src/app/service/store.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-menucategory',
  templateUrl: './menucategory.component.html',
  styleUrls: ['./menucategory.component.css']
})
export class MenuCategoryComponent implements OnInit {
  // Propiedades
  show: boolean = false;
  formMenuCategory: FormGroup;
  menucategories: any;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  creating = true;
  noValido = true;
  id = 0;

  constructor(
    public form: FormBuilder,
    private storeService: StoreService
  ) {
    // Inicializar formulario
    this.formMenuCategory = this.form.group({
      Name: ['']
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

  // Método para obtener los menucategoryos y proveedores
  get() {
    this.storeService.getMenuCategories().subscribe(response => {
      this.menucategories = response;
      this.dtTrigger.next(0);
    });
  }

  ngOnInit(): void {
    // Configuraciones de DataTables
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true
    };
    this.get();
  }

  // Método para editar un menucategoryo
  edit(id: any) {
    this.creating = false;
    //this.formMenuCategory.get('Amount')?.disable();
    this.storeService.getMenuCategory(id).subscribe((response: any) => {
      this.id = response.CategoryId;
      this.formMenuCategory.setValue({
        Name: response.Name
      });
    });
    this.formMenuCategory = this.form.group({
      Name: ['']
    });
  }

  // Método para eliminar un menucategoryo
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
        this.storeService.deleteMenuCategory(id).subscribe(() => {
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

  // Método para guardar un menucategory
  submit() {
    var menucategory = new MenuCategory();
    menucategory.Name = this.formMenuCategory.value.Name;
    var solicitud = this.creating ? this.storeService.insertMenuCategory(menucategory) : this.storeService.updateMenuCategory(this.id, menucategory);
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
          console.log(r);
          if (this.creating == true && r.message==='MenuCategory with the same name already exists' ) {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Error al registrar',
              text: 'Ya existe una categoría con ese nombre.',
            });
          }else{
            Swal.fire({
              allowOutsideClick: false,
              icon: 'success',
              title: 'Éxito',
              text: 'Se ha guardado correctamente!',
            }).then((result) => {
              window.location.reload();
            });
          }
          

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
    this.formMenuCategory = this.form.group({
      Name: ['']
    });

    this.show = false;

  }
}
