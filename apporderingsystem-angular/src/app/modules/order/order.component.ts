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
  @ViewChild('selectUbication') selectUbication: any;
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
    this.storeService.getUserByRole('Repartidor', localStorage.getItem('token')).subscribe(r => {
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
    this.DeliveryDate.nativeElement.disabled = true;
    this.ProductId.nativeElement.disabled = true;
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
      if (response.State == "Received") {
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
    for (const element of this.elements) {
      this.finalprice = this.finalprice + (element.quantity * element.price);
    }
    this.storeService.getUser(Number(localStorage.getItem('userId')), localStorage.getItem('token')).subscribe((user: any) => {
      var order = new Order();
      order.OrderDate = this.todayWithPipe;
      order.ReceptionDate = this.formOrder.value.ReceptionDate;
      order.DispatchedDate = this.formOrder.value.DispatchedDate;
      order.DeliveryDate = this.formOrder.value.DeliveryDate;
      order.TotalPrice = this.finalprice;
      order.Seller = this.formOrder.value.Seller;
      order.DeliveryMan = this.formOrder.value.Seller;
      order.Status = "Por atender";
    })

    if (!this.creating) {
      order.OrderId = this.orderid;
    }

    var solicitud = this.creating ? this.storeService.insertOrder(order) : this.storeService.updateOrder(this.orderid, order);
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
          for (const ub of this.ubs) {
            ubid = ub.UbicationId;

            this.storeService.updateUbication(ub.UbicationId, ub).subscribe(() => { });
          }
          for (const element of this.elements) {
            var orderxproduct = new OrderXProduct();
            orderxproduct.Quantity = element.quantity;
            orderxproduct.OrderId = r;
            orderxproduct.ProductId = element.productid;
            this.storeService.insertOrderXProduct(orderxproduct).subscribe(() => { })
            this.storeService.getProduct(element.productid).subscribe((resp: any) => {
              resp.Quantity = resp.Quantity - element.quantity;
              this.storeService.updateProduct(element.productid, resp).subscribe(() => {
              })
            })

          }
          this.elements.length = 0;
          this.ubs.length = 0;
          console.log(order.OrderDate);
          Swal.fire({
            allowOutsideClick: false,
            icon: 'success',
            title: 'Éxito',
            text: '¡Se ha guardado correctamente!',
          }).then((result) => {
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
    order.OrderDate = this.todayWithPipe;
    order.State = this.formEditOrder.value.State;
    order.DeliveryDate = this.formEditOrder.value.DeliveryDate;
    order.TotalPrice = this.formEditOrder.value.TotalPrice;
    order.ClientId = this.formEditOrder.value.ClientId;
    order.UserId = Number(localStorage.getItem('userId'));
    if (this.formEditOrder.value.State == "Cancelado") {
      this.storeService.getUbication(this.formEditOrder.value.UbicationId).subscribe((ub: any) => {
        this.storeService.getOrderXProductByOrderId(this.orderid).subscribe((orderxproducts: any) => {
          for (const orderxproduct of orderxproducts) {
            quantityorder = quantityorder + orderxproduct.Quantity;
          }
          console.log(quantityorder);
          if (quantityorder > (ub.Capacity - ub.Quantity)) {
            Swal.fire({
              allowOutsideClick: false,
              icon: 'error',
              title: 'Excede la cantidad de stock del almacén',
              text: 'La capacidad del almacén es de ' + ub.Capacity + ' productos y actualmente tiene ' + ub.Quantity + ' productos.'
            });
            this.invalidate = true;
          }
        })
      })
    }
    if (this.formEditOrder.value.State == "Cancelado" || this.formEditOrder.value.State == "Despachado") {
      if (this.invalidate == false) {
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

            this.storeService.updateOrder(this.orderid, order).subscribe(r => {
              if (this.formEditOrder.value.State == "Despachado") {
                this.storeService.getOrderXProductByOrderId(this.orderid).subscribe((orderxproducts: any) => {
                  for (const orderxproduct of orderxproducts) {
                    var output = new Output();
                    output.Date = this.todayWithPipe;
                    output.Quantity = orderxproduct.Quantity;
                    output.ProductId = orderxproduct.ProductId;
                    output.ClientId = order.ClientId;
                    output.UserId = order.UserId;
                    this.storeService.insertOutput(output).subscribe(() => { })
                    this.storeService.getProduct(output.ProductId).subscribe((p: any) => {
                      console.log(orderxproduct.Quantity)
                      var operation = new Operation();
                      operation.Date = output.Date;
                      operation.Description = 'Venta de ' + orderxproduct.Quantity + ' ' + p.Description + '(s)';
                      operation.ProductId = p.ProductId;
                      operation.UserId = Number(localStorage.getItem('userId'));
                      this.storeService.insertOperation(operation).subscribe(() => { })
                    })
                  }
                })
              }
              if (this.formEditOrder.value.State == "Cancelado") {
                this.storeService.getUbication(this.formEditOrder.value.UbicationId).subscribe((ub: any) => {
                  this.storeService.getOrderXProductByOrderId(this.orderid).subscribe((orderxproducts: any) => {
                    for (const orderxproduct of orderxproducts) {
                      this.storeService.getProduct(orderxproduct.ProductId).subscribe((p: any) => {
                        p.Quantity = p.Quantity + orderxproduct.Quantity;
                        this.storeService.updateProduct(orderxproduct.ProductId, p).subscribe(() => {
                        })
                        ub.Quantity = ub.Quantity + Number(orderxproduct.Quantity);
                        if (ub.Description == "El almacén no tiene productos") {
                          ub.Description = ub.Quantity + " " + p.Description + "(s)";
                        } else {
                          const splits: string[] = ub.Description.split(',');
                          for (let i = 0; i < splits.length; i++) {
                            if (splits[i].includes(p.Description)) {
                              const array: string[] = splits[i].split(' ');
                              const quantityprodub = Number(array[0]) + Number(orderxproduct.Quantity);
                              console.log(array[0]);
                              ub.Description = ub.Description.replace(array[0] + " " + array[1], quantityprodub + " " + array[1]);
                              this.existprod = true;
                              break;
                            }
                          }
                          if (this.existprod == false) {
                            ub.Description = ub.Description + "," + orderxproduct.Quantity + " " + p.Description + "(s)";
                          }
                        }
                        this.existprod = false;
                        var operation = new Operation();
                        operation.Date = this.todayWithPipe;
                        operation.Description = 'Se devolvió ' + orderxproduct.Quantity + ' ' + p.Description + '(s) a ' + ub.Name;
                        operation.ProductId = p.ProductId;
                        operation.UserId = Number(localStorage.getItem('userId'));
                        this.storeService.insertOperation(operation).subscribe(() => { })
                        this.storeService.updateUbication(ub.UbicationId, ub).subscribe(() => { });
                      })
                    }

                  })
                })
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
          }
        });
      }
    }
    this.invalidate = false;
    quantityorder = 0;
  }
  //Metodo para agregar productos a una ordern
  addProduct() {

    if (this.formOrder.value.DeliveryDate != "" && this.formOrder.value.ClientId != "" && this.formOrder.value.ProductId != "" && this.formOrder.value.UbicationId != "" && this.formOrder.value.Quantity != "") {
      if (this.idproductelement != 0 && this.idubication != 0) {
        this.DeliveryDate.nativeElement.disabled = false;
        this.ClientId.nativeElement.disabled = false;
        this.ProductId.nativeElement.disabled = false;
        this.UbicationId.nativeElement.disabled = false;
        this.deleteElement(this.idproductelement, this.idubication);
      }
      this.storeService.getUbication(this.formOrder.value.UbicationId).subscribe((ub: any) => {
        this.storeService.getProduct(this.formOrder.value.ProductId).subscribe((p: any) => {
          this.productdescription = p.Description;
          this.productprice = p.SalePrice;

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
            ub.Quantity = ub.Quantity - Number(this.formOrder.value.Quantity);
            const splits: string[] = ub.Description.split(',');
            for (let i = 0; i < splits.length; i++) {
              if (splits[i].includes(p.Description)) {
                const array: string[] = splits[i].split(' ');
                const quantityprodub = Number(array[0]) - Number(this.formOrder.value.Quantity);
                if (quantityprodub < 0) {
                  this.exceed = true;
                  Swal.fire({
                    allowOutsideClick: false,
                    icon: 'error',
                    title: 'Excede la cantidad de stock',
                    text: 'En ' + ub.Name + ' hay ' + array[0] + " " + p.Description + "(s)"
                  });
                } else {
                  if (ub.Quantity == 0) {
                    ub.Description = "El almacén no tiene productos";
                  } else {
                    if (quantityprodub >= 0) {
                      if (quantityprodub == 0) {
                        if (i == splits.length - 1) {
                          ub.Description = ub.Description.replace(',' + splits[i], '');
                        } else {
                          ub.Description = ub.Description.replace(splits[i] + ',', '');
                        }
                      } else {
                        ub.Description = ub.Description.replace(array[0] + " " + array[1], quantityprodub + " " + array[1]);
                      }
                    }
                  }
                  this.elements.push({
                    productid: this.formOrder.value.ProductId,
                    product: this.productdescription,
                    price: this.productprice,
                    quantity: this.formOrder.value.Quantity,
                    ubicationid: ub.UbicationId
                  });
                  this.finalprice = this.finalprice + (this.formOrder.value.Quantity * p.SalePrice);
                  break;
                }
              }
            }
            if (this.exceed == false) {
              for (let i = 0; i < this.ubs.length; i++) {
                if (this.ubs[i].UbicationId == ub.UbicationId) {
                  const splitsubs: string[] = this.ubs[i].Description.split(',');
                  for (let j = 0; j < splits.length; j++) {
                    if (splitsubs[j].includes(p.Description)) {
                      const array: string[] = splitsubs[j].split(' ');
                      const quantityprodub = Number(array[0]) - Number(this.formOrder.value.Quantity);
                      if (quantityprodub == 0) {
                        if (j == splits.length - 1) {
                          this.ubs[i].Description = this.ubs[i].Description.replace(',' + splitsubs[j], '');
                        } else {
                          this.ubs[i].Description = this.ubs[i].Description.replace(splitsubs[j] + ',', '');
                        }
                      } else {
                        this.ubs[i].Description = this.ubs[i].Description.replace(array[0] + " " + array[1], quantityprodub + " " + array[1]);
                      }
                      this.ubs[i].Quantity = this.ubs[i].Quantity - this.formOrder.value.Quantity;
                      this.recalledprod = true;
                      //console.log(this.ubs[i]);
                      break;
                    }
                  }
                }
              }
              if (this.recalledprod == false) {
                this.ubs.push(ub);
              }
            }
            console.log(this.ubs);
            //console.log(ub.Quantity);
            //dsa
            //this.storeService.updateUbication(this.formOrder.value.UbicationId,ub).subscribe(()=>{});
            /*for (const element of this.elements) {
              console.log(element)
              this.finalprice = this.finalprice + (element.quantity * element.price);
              console.log(this.finalprice)
            }*/
          }
          this.recalledprod = false;
          this.exceed = false;
          this.addedproduct = false;
          this.idproduct = 0; this.productdescription = ""; this.productprice = 0;
        })
      })
    } else {
      console.log("hola");
    }
    this.idproductelement = 0;
    this.idubication = 0;
  }
  updateUbications() {
    this.ubications.length = 0;
    this.storeService.getProduct(this.formOrder.value.ProductId).subscribe((p: any) => {
      this.storeService.getUbications().subscribe((ubs: any) => {
        for (let i = 0; i < ubs.length; i++) {
          if (ubs[i].Description.includes(p.Description)) {
            this.ubications.push({
              UbicationId: ubs[i].UbicationId,
              Name: ubs[i].Name
            });
            //console.log(this.ubications)
          }
        }
      });
    })
  }
  updateOldUbications() {
    if (this.formEditOrder.value.State == "Cancelado") {
      this.showUbication = true;
      this.storeService.getUbications().subscribe((ubs: any) => {
        this.oldubications = ubs;
      });
    } else {
      this.showUbication = false;
      //this.selectUbication.nativeElement.disabled = false;
    }
  }
  // Método para cerrar el modal y limpiar el formulario
  closeModal() {

    this.formOrder = this.form.group({
      DeliveryDate: [''],
      ProductId: [''],
      UbicationId: [''],
      ClientId: [''],
      Quantity: ['']
    });
    this.formEditOrder = this.form.group({
      OrderDate: [''],
      State: [''],
      DeliveryDate: [''],
      TotalPrice: [''],
      ClientId: [''],
      UbicationId: ['']
    });
    this.showUbication = false;
    this.elements.length = 0;
    this.ubs.length = 0;
    this.finalprice = 0;
  }

}