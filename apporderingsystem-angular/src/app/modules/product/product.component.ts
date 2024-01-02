import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Product } from 'src/app/models/product';
import { FormGroup, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { StoreService } from 'src/app/service/store.service';

import Swal from 'sweetalert2';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  show: boolean = false;
  formProduct: FormGroup;
  products: any;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  creating = true;
  noValido = true;
  product:any;

  constructor(
    private http: HttpClient,
    private sanitizer: DomSanitizer,
    public form: FormBuilder,
    private storeService: StoreService
  ) {
    this.formProduct = this.form.group({
      SKU: [''],
      Name: [''],
      Kind: [''],
      Label: [''],
      Price: [''],
      UnitMeasurement: ['']
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
    this.storeService.getProducts(localStorage.getItem('token')).subscribe(response => {
      this.products = response;
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
  edit(productid: any) {
    this.creating = false;
    this.storeService.getProduct(productid,localStorage.getItem('token')).subscribe(
      (response: any) => {
        this.product = response;
        this.formProduct.setValue({
          SKU:response.sku,
          Name: response.name,
          Kind: response.kind,
          Label: response.label,
          Price:response.price,
          UnitMeasurement: response.unitMeasurement
        });
        console.log(this.product);
      }
    );
    this.formProduct = this.form.group({
      SKU:[''],
      Name: [''],
      Kind: [''],
      Label: [''],
      Price: [''],
      UnitMeasurement: ['']
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
        this.storeService.deleteProduct(id,localStorage.getItem('token')).subscribe(r => {
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
    var product = new Product();
    if(this.creating==false){
      product.productId=this.product.productId;
    }
    product.sku = this.formProduct.value.SKU;
    product.name = this.formProduct.value.Name;
    product.kind = this.formProduct.value.Kind;
    product.label = this.formProduct.value.Label;
    product.price=this.formProduct.value.Price;
    product.unitMeasurement = this.formProduct.value.UnitMeasurement;
    
    var solicitud = this.creating ? this.storeService.insertProduct(product,localStorage.getItem('token')) : this.storeService.updateProduct(product,localStorage.getItem('token'));
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
          if (this.creating == true && r.message === 'Product with the same name already exists') {
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
    this.formProduct = this.form.group({
      SKU:[''],
      Name: [''],
      Kind: [''],
      Label: [''],
      Price: [''],
      UnitMeasurement: ['']
    });
  }
}
