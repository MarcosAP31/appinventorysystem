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
  filterForm: FormGroup;
  formOrder: FormGroup;
  formEditOrder: FormGroup;
  orders: any;
  order: any;
  deliverymans: any;
  sellers: any;
  quantityorder: number = 0;
  orderxproducts: any;
  elements: { productid: number, sku: string, product: string, price: number, quantity: number }[] = [];
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  creating = true;
  noValorderido = true;
  orderid: number = 0;
  idproduct: number = 0;
  productsku: string = "";
  productprice: number = 0;
  productdescription: string = "";
  pipe = new DatePipe('en-US');
  todayWithPipe: any;
  clients: any;
  products: any;
  finalprice: any = 0;
  addedproduct: boolean = false;
  recalledprod: boolean = false;
  invalidate: boolean = false;
  existprod: boolean = false;
  idproductelement: number = 0;
  constructor(
    public form: FormBuilder,
    private storeService: StoreService,
  ) {
    this.filterForm = this.form.group({
      StartDate: [''],
      EndDate: ['']
    })
    this.formOrder = this.form.group({
      ProductId: [''],
      Quantity: [''],
      Seller: [''],
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
    this.storeService.getUsersByRole('Vendedor', localStorage.getItem('token')).subscribe(r => {
      this.sellers = r;
    })
    this.storeService.getUsersByRole('Repartidor', localStorage.getItem('token')).subscribe(r => {
      this.deliverymans = r;
    })
    /*this.storeService.getUsersByRole('Repartidor', localStorage.getItem('token')).subscribe(r => {
      this.users = r;
    })*/
  }

  ngOnInit(): void {
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true
    };
    this.get();
    this.todayWithPipe = this.pipe.transform(Date.now(), 'dd/MM/yyyy h:mm:ss a');
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
          Seller: this.formOrder.value.Seller,
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

  formatDate(datetimeString: string | Date): string {
    const date = typeof datetimeString === 'string' ? new Date(datetimeString) : datetimeString;

    if (isNaN(date.getTime())) {
      // Handle invalid date
      return 'Invalid Date';
    }

    const day = ('0' + date.getDate()).slice(-2);
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const year = date.getFullYear();
    const hours = ('0' + date.getHours()).slice(-2);
    const minutes = ('0' + date.getMinutes()).slice(-2);
    const seconds = ('0' + date.getSeconds()).slice(-2);
    const meridian = date.getHours() >= 12 ? 'PM' : 'AM';

    //return `${day}/${month}/${year} ${hours}:${minutes}:${seconds} ${meridian}`;
    return `${day}/${month}/${year}`;
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
      if (response.status == "Recibido") {
        this.selectState.nativeElement.disabled = true;
      } else {
        this.selectState.nativeElement.disabled = false;
      }
      this.order = response;
      // Similarly, format other date values
      var formattedOrderDate = this.formatDate(response.orderDate);
      var formattedReceptionDate:any=null;
      var formattedDispatchedDate:any=null;
      var formattedDeliveryDate:any=null;
      if(response.receptionDate!=null){
        formattedReceptionDate = response.receptionDate.toString().split('T')[0];
      }
      if(response.dispatchedDate!=null){
        formattedDispatchedDate = response.dispatchedDate.toString().split('T')[0];
      }
      if(response.deliveryDate!=null){
        formattedDeliveryDate = response.deliveryDate.toString().split('T')[0];
      }
      
      this.storeService.getUserByFullName(this.order.seller, localStorage.getItem('token')).subscribe((s: any) => {
        this.storeService.getUserByFullName(this.order.deliveryMan, localStorage.getItem('token')).subscribe((d: any) => {
          this.formEditOrder.setValue({
            OrderDate: formattedOrderDate,
            ReceptionDate: formattedReceptionDate,
            DispatchedDate: formattedDispatchedDate,
            DeliveryDate: formattedDeliveryDate,
            TotalPrice: response.totalPrice,
            Seller: s.userId,
            DeliveryMan: d.userId,
            State: response.status
          });
        })

      })

    });
    this.storeService.getProductsByOrderId(orderid, localStorage.getItem('token')).subscribe(r => {
      this.orderxproducts = r;
      console.log(r);
    })
    this.storeService.getTotalPriceByOrderId(orderid, localStorage.getItem('token')).subscribe(r => {
      this.finalprice = r
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

  convertToDateTime(dateString: string | null): Date | null {
    if (dateString) {
      // Assuming your date-time format is "dd/MM/yyyy hh:mm:ss a"
      const dateTimeParts = dateString.split(' ');
  
      if (dateTimeParts.length === 3) {
        const datePart = dateTimeParts[0];
        const timePart = dateTimeParts[1] + ' ' + dateTimeParts[2];
  
        const dateParts = datePart.split('/');
        const timeParts = timePart.split(':');
  
        if (dateParts.length === 3 && timeParts.length === 3) {
          const day = parseInt(dateParts[0], 10);
          const month = parseInt(dateParts[1], 10) - 1;
          const year = parseInt(dateParts[2], 10);
  
          const hour = parseInt(timeParts[0], 10);
          const minute = parseInt(timeParts[1], 10);
          const second = parseInt(timeParts[2], 10);
  
          // Create a Date object with the provided date and time
          const date = new Date(year, month, day, hour, minute, second);
  
          // Set the time zone to Peru (GMT-5) using toLocaleString
          const options = { timeZone: 'America/Lima' };
          const peruTimeString = date.toLocaleString('en-US', options);
  
          // Convert the formatted time string back to a Date object
          const peruDate = new Date(peruTimeString);
  
          return peruDate;
        }
      }
    }
  
    return null;
  }

  // Add a helper method to convert string to Date
  convertToDate(dateString: string | null): Date | null {
    if (dateString) {
      // Assuming your date format is "dd/MM/yyyy"
      const parts = dateString.split('/');

      if (parts.length === 3) {
        const day = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10) - 1;
        const year = parseInt(parts[2], 10);

        const date = new Date(year, month, day);

        return date;
      }
    }

    return null;
  }

  applyFilter() {
    const startDate = this.pipe.transform(this.filterForm.value.StartDate, 'yyyy-MM-dd 00:00:00.000');
    const endDate = this.pipe.transform(this.filterForm.value.EndDate, 'yyyy-MM-dd 00:00:00.000');
    console.log(startDate)
    this.storeService.getOrdersByRange(startDate, endDate, localStorage.getItem('token')).subscribe(r => {
      this.orders = r;
      console.log(this.orders)
    })
  }
  // Método para guardar el proveedor
  submit() {
    this.finalprice = 0;
    var order = new Order();
    for (const element of this.elements) {
      this.finalprice = this.finalprice + (element.quantity * element.price);
    }

    this.storeService.getUser(this.formOrder.value.Seller, localStorage.getItem('token')).subscribe((user: any) => {

      order.orderDate = this.convertToDate(this.todayWithPipe); // Assuming you want to use the current date and time
      console.log(order.orderDate);
      
      order.totalPrice = this.finalprice;
      order.seller = user.name + " " + user.lastName;

      this.storeService.getUser(this.formOrder.value.DeliveryMan, localStorage.getItem('token')).subscribe((u: any) => {
        order.deliveryMan = u.name + " " + u.lastName;
      })
      order.status = "Por atender";
      console.log(order)
    })

    if (!this.creating) {
      order.orderId = this.orderid;
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
          console.log(r)
          for (const element of this.elements) {
            var orderxproduct = new OrderXProduct();
            orderxproduct.orderId = r;
            orderxproduct.productId = element.productid;
            orderxproduct.quantity = element.quantity;
            orderxproduct.subtotal = element.quantity * element.price;
            this.storeService.insertOrderXProduct(orderxproduct, localStorage.getItem('token')).subscribe(() => { })
          }
          this.elements.length = 0;
          console.log(order.orderDate);
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
  updateOrder() {
    var order = new Order();
    this.storeService.getUser(this.formEditOrder.value.Seller, localStorage.getItem('token')).subscribe((user: any) => {
      order.orderId = this.order.orderId;
      order.orderDate = this.order.orderDate;
      order.receptionDate = this.formEditOrder.value.ReceptionDate;
      order.dispatchedDate = this.formEditOrder.value.DispatchedDate;
      order.deliveryDate = this.formEditOrder.value.DeliveryDate;
      order.totalPrice = this.formEditOrder.value.TotalPrice;
      order.seller = user.name + " " + user.lastName;
      this.storeService.getUser(this.formEditOrder.value.DeliveryMan, localStorage.getItem('token')).subscribe((u: any) => {
        order.deliveryMan = u.name + " " + u.lastName;
      })
      order.status = this.formEditOrder.value.State;

    })
    var solicitud = this.storeService.updateOrder(order, localStorage.getItem('token'));
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

        solicitud.subscribe(() => {
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
      this.storeService.getProduct(this.formOrder.value.ProductId, localStorage.getItem('token')).subscribe((p: any) => {
        this.productsku = p.sku;
        this.productdescription = p.name;
        this.productprice = p.price;

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
            sku: this.productsku,
            product: this.productdescription,
            price: this.productprice,
            quantity: this.formOrder.value.Quantity,
          });
          this.finalprice = this.finalprice + (this.formOrder.value.Quantity * p.price);
        }

        this.addedproduct = false;
        this.idproduct = 0; this.productsku = ""; this.productdescription = ""; this.productprice = 0;
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