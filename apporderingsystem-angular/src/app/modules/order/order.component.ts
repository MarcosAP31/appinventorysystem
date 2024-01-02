import { Component, OnInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { Order } from 'src/app/models/order';
import { OrderXProduct } from 'src/app/models/orderxproduct';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { StoreService } from 'src/app/service/store.service';
import Swal from 'sweetalert2';
import { DatePipe } from '@angular/common';
import { subscribe } from 'diagnostics_channel';
@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  @ViewChild('selectState') selectState: any;
  @ViewChild('ReceptionDate') ReceptionDate: any;
  @ViewChild('DispatchedDate') DispatchedDate: any;
  @ViewChild('DeliveryDate') DeliveryDate: any;
  @ViewChild('ClientId') ClientId: any;
  @ViewChild('ProductId') ProductId: any;
  formOrder: FormGroup;
  formEditOrder: FormGroup;
  orders: any;
  users: any;
  totalprice: number = 0;
  quantityorder: number = 0;
  orderxproducts: any;
  elements: { productid: number, sku: string, product: string, price: number, quantity: number }[] = [];
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  creating = true;
  noValorderido = true;
  orderid: number = 0;
  idproduct: number = 0;
  productsku:string="";
  productprice: number = 0;
  productdescription: string = "";
  pipe = new DatePipe('en-US');
  todayWithPipe: any;
  clients: any;
  products: any;
  finalprice: number = 0;
  addedproduct: boolean = false;
  recalledprod: boolean = false;
  invalidate: boolean = false;
  existprod: boolean = false;
  idproductelement: number = 0;
  constructor(
    public form: FormBuilder,
    private storeService: StoreService,
  ) {
    this.formOrder = this.form.group({
      ReceptionDate: [''],
      DispatchedDate: [''],
      DeliveryDate: [''],
      ProductId: [''],
      Quantity: [''],
      DeliveryMan: [''],
    });
    this.formEditOrder = this.form.group({
      OrderDate: [''],
      ReceptionDate: [''],
      DispatchedDate: [''],
      DeliveryDate: [''],
      TotalPrice: [''],
      Seller: [''],
      DeliveryMan: [''],
      State: ['']

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

  // Método para detener la carga
  stopLoading() {
    Swal.close();
  }

  // Método para obtener los proveedores
  get() {
    this.storeService.getOrders(localStorage.getItem('token')).subscribe(response => {
      this.orders = response;
      this.dtTrigger.next(0);
    });
    this.storeService.getProducts(localStorage.getItem('token')).subscribe(response => {
      this.products = response;
    })
    this.storeService.getUsersByRole('Repartidor', localStorage.getItem('token')).subscribe(r => {
      this.users = r;
    })
  }

  ngOnInit(): void {
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true
    };
    this.get();
    this.todayWithPipe = this.pipe.transform(Date.now(), 'dd/MM/yyyy  h:mm:ss a');
  }

  // Método para editar un proveedor
  editElement(idproduct: any) {
    for (const element of this.elements) {
      if (element.productid == idproduct) {
        console.log(idproduct)
        this.formOrder.setValue({
          ReceptionDate: this.formOrder.value.ReceptionDate,
          DispatchedDate: this.formOrder.value.DispatchedDate,
          DeliveryDate: this.formOrder.value.DeliveryDate,
          ProductId: element.productid,
          Quantity: element.quantity,
          DeliveryMan: this.formOrder.value.DeliveryMan,
        });
        console.log(element)
        this.idproductelement = element.productid;
        break;
      }
    }
    this.ReceptionDate.nativeElement.disabled = false;
    this.DispatchedDate.nativeElement.disabled = false;
    this.DeliveryDate.nativeElement.disabled = false;
    this.ProductId.nativeElement.disabled = false;
  }
  deleteElement(idproduct: any) {
    this.storeService.getProduct(idproduct, localStorage.getItem('token')).subscribe((p: any) => {

      for (const element of this.elements) {
        if (element.productid == idproduct) {
          const indice = this.elements.indexOf(element);
          this.finalprice = this.finalprice - (element.quantity * element.price);
          this.elements.splice(indice, 1); // Elimina 1 elemento a partir del índice encontrado
        }
      }
    })
  }
  editOrder(orderid: any) {
    this.finalprice = 0;
    /*for (const element of this.elements) {

      const indice = this.elements.indexOf(element);
      this.elements.splice(indice, 1); // Elimina 1 elemento a partir del índice encontrado
    }*/
    this.elements.length = 0;
    console.log(this.elements)
    this.creating = false;
    this.storeService.getOrder(orderid, localStorage.getItem('token')).subscribe((response: any) => {
      if (response.State == "Recibido") {
        this.selectState.nativeElement.disabled = true;
      } else {
        this.selectState.nativeElement.disabled = false;
      }
      this.orderid = response.OrderId;
      this.formEditOrder.setValue({
        OrderDate: response.OrderDate,
        ReceptionDate: response.ReceptionDate,
        DispatchedDate: response.DispatchedDate,
        DeliveryDate: response.DeliveryDate,
        TotalPrice: response.TotalPrice,
        Seller: response.Seller,
        DeliveryMan: response.DeliveryMan,
        State: response.State
      });
    });
    this.storeService.getProductsByOrderId(orderid, localStorage.getItem('token')).subscribe(r => {
      this.orderxproducts = r;
    })

    this.formEditOrder = this.form.group({
      OrderDate: [''],
      ReceptionDate: [''],
      DispatchedDate: [''],
      DeliveryDate: [''],
      TotalPrice: [''],
      Seller: [''],
      DeliveryMan: [''],
      State: ['']
    });
  }
  // Método para eliminar un proveedor
  deleteOrder(orderid: any) {
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

        this.storeService.deleteOrder(orderid, localStorage.getItem('token')).subscribe(r => {
          Swal.fire({
            allowOutsideClick: false,
            icon: 'success',
            title: 'Éxito',
            text: '¡Se ha eliminado correctamente!',
          }).then((result) => {
            window.location.reload();
          });
        }, err => {
          console.log(err);

          if (err.OrderDate == 'HttpErrorResponse') {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Error al conectar',
              text: 'Error de comunicación con el servorderidor',
            });
            return;
          }
          Swal.fire({
            allowOutsideClick: false,
            icon: 'error',
            title: err.OrderDate,
            text: err.message,
          });
        });

      } else if (result.isDenied) {
        // Acción cancelada
      }
    });
  }

  // Método para guardar el proveedor
  submit() {
    this.finalprice = 0;
    var order = new Order();
    for (const element of this.elements) {
      this.finalprice = this.finalprice + (element.quantity * element.price);
    }
    this.storeService.getUser(Number(localStorage.getItem('userId')), localStorage.getItem('token')).subscribe((user: any) => {

      order.OrderDate = this.todayWithPipe;
      order.ReceptionDate = this.formOrder.value.ReceptionDate;
      order.DispatchedDate = this.formOrder.value.DispatchedDate;
      order.DeliveryDate = this.formOrder.value.DeliveryDate;
      order.TotalPrice = this.finalprice;
      order.Seller = user.Name + " " + user.LastName;
      this.storeService.getUser(this.formOrder.value.DeliveryMan, localStorage.getItem('userId')).subscribe((u: any) => {
        order.DeliveryMan = u.Name + " " + u.LastName;
      })
      order.Status = "Por atender";
    })

    if (!this.creating) {
      order.OrderId = this.orderid;
    }

    var solicitud = this.creating ? this.storeService.insertOrder(order, localStorage.getItem('token')) : this.storeService.updateOrder(order, localStorage.getItem('token'));
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

        solicitud.subscribe((r: any) => {
          for (const element of this.elements) {
            var orderxproduct = new OrderXProduct();
            orderxproduct.OrderId = r;
            orderxproduct.ProductId = element.productid;
            orderxproduct.Quantity = element.quantity;
            orderxproduct.Subtotal = element.quantity * element.price;
            this.storeService.insertOrderXProduct(orderxproduct, localStorage.getItem('token')).subscribe(() => { })
          }
          this.elements.length = 0;
          console.log(order.OrderDate);
          Swal.fire({
            allowOutsideClick: false,
            icon: 'success',
            title: 'Éxito',
            text: '¡Se ha guardado correctamente!',
          }).then(() => {
            window.location.reload();
          });
        }, err => {
          console.log(err);

          if (err.Name == 'HttpErrorResponse') {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Error al conectar',
              text: 'Error de comunicación con el servorderxproductidor',
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
        // Acción cancelada
      }
    });

  }
  submitOrder() {
    var order = new Order();
    var quantityorder = 0;
    this.storeService.getUser(Number(localStorage.getItem('userId')), localStorage.getItem('token')).subscribe((user: any) => {
      order.OrderDate = this.todayWithPipe;
      order.ReceptionDate = this.formEditOrder.value.ReceptionDate;
      order.DispatchedDate = this.formEditOrder.value.DispatchedDate;
      order.DeliveryDate = this.formEditOrder.value.DeliveryDate;
      order.TotalPrice = this.formEditOrder.value.TotalPrice;
      order.Seller=user.Name+" "+user.LastName;
      this.storeService.getUser(this.formOrder.value.DeliveryMan, localStorage.getItem('userId')).subscribe((u: any) => {
        order.DeliveryMan = u.Name + " " + u.LastName;
      })
      order.Status = this.formEditOrder.value.State;
      
    })

  }
  //Metodo para agregar productos a una ordern
  addProduct() {

    if (this.formOrder.value.DeliveryDate != "" && this.formOrder.value.ClientId != "" && this.formOrder.value.ProductId != "" && this.formOrder.value.UbicationId != "" && this.formOrder.value.Quantity != "") {
      if (this.idproductelement != 0) {
        this.ReceptionDate.nativeElement.disabled = false;
        this.DispatchedDate.nativeElement.disabled = false;
        this.DeliveryDate.nativeElement.disabled = false;
        this.ProductId.nativeElement.disabled = false;
        this.deleteElement(this.idproductelement);
      }
      this.storeService.getProduct(this.formOrder.value.ProductId,localStorage.getItem('token')).subscribe((p: any) => {
        this.productsku=p.SKU;
        this.productdescription = p.Name;
        this.productprice = p.Price;

        for (const element of this.elements) {
          if (element.productid == this.formOrder.value.ProductId) {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'El producto ya está agregado en la orden'
            });
            this.addedproduct = true;
            break;
          }
        }
        if (this.addedproduct == false) {
          this.elements.push({
            productid: this.formOrder.value.ProductId,
            sku:this.productsku,
            product: this.productdescription,
            price: this.productprice,
            quantity: this.formOrder.value.Quantity,
          });
          this.finalprice = this.finalprice + (this.formOrder.value.Quantity * p.SalePrice);
        }
       
        this.addedproduct = false;
        this.idproduct = 0; this.productsku="";this.productdescription = ""; this.productprice = 0;
      })
    } else {
      console.log("hola");
    }
    this.idproductelement = 0;
  }
  
  
  // Método para cerrar el modal y limpiar el formulario
  closeModal() {

    this.formOrder = this.form.group({
      ReceptionDate: [''],
      DispatchedDate: [''],
      DeliveryDate: [''],
      ProductId: [''],
      Quantity: [''],
      DeliveryMan: [''],
    });
    this.formEditOrder = this.form.group({
      OrderDate: [''],
      ReceptionDate: [''],
      DispatchedDate: [''],
      DeliveryDate: [''],
      TotalPrice: [''],
      Seller: [''],
      DeliveryMan: [''],
      State: ['']

    });
    this.elements.length = 0;
    this.finalprice = 0;
  }

}