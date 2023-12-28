import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Order } from 'src/app/models/order';
import { FormGroup, FormBuilder } from '@angular/forms';
import { StoreService } from 'src/app/service/store.service';
import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, Observer } from 'rxjs';
import Swal from 'sweetalert2';
import pdfMake from 'pdfmake/build/pdfmake';
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
pdfMake.vfs = pdfFonts.pdfMake.vfs;

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  // Propiedades
  order: any = {}

  selectedPaymentState: any;
  show: boolean = false;
  totalprice: any = 0;
  formOrder: FormGroup;
  orders: any;
  users: any;
  orderxitems: any;
  pipe = new DatePipe('en-US');
  todayWithPipe: any;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject<any>();
  creating = true;
  noValido = true;
  id = 0;

  constructor(
    private http: HttpClient,
    public form: FormBuilder,
    private storeService: StoreService
  ) {
    // Inicializar formulario
    this.formOrder = this.form.group({
      Status: [''],
      PaymentStatus: [''],
      TotalAmount: [''],
      UserId: ['']
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

  // Método para obtener los orderos y proveedores
  get() {
    this.storeService.getOrders().subscribe(response => {
      this.orders = response;
      this.dtTrigger.next(0);
    });
    this.storeService.getUsers().subscribe(response => {
      this.users = response;
    });
  }

  ngOnInit(): void {
    // Configuraciones de DataTables
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true
    };
    this.get();
    this.todayWithPipe = this.pipe.transform(Date.now(), 'ddMMyyyyhmmssa');
  }

  // Function to handle payment state change
  onPaymentStateChange() {
    console.log(this.formOrder.value.PaymentStatus);
    this.selectedPaymentState = this.formOrder.value.PaymentStatus;
  }

  // Function to generate ticket
  generateTicket(path: string) {
    
    const orderDate = new Date(this.order.OrderDate);
    const formattedDate = `${orderDate.getDate()}/${orderDate.getMonth() + 1}/${orderDate.getFullYear()}`;
    const formattedTime = `${orderDate.getHours()}:${orderDate.getMinutes()}:${orderDate.getSeconds()}`;
    const pdfContent = {
      content: [
        {
          text: 'Ticket',
          style: 'header',
          alignment: 'center',
        },
        {
          text: 'Order Information',
          style: 'subheader',
          margin: [0, 10, 0, 5],
        },
        {
          columns: [
            {
              width: 'auto',
              text: `Order Date: ${formattedDate} ${formattedTime}`,
              margin: [0, 0, 10, 0],
            },
            {
              width: 'auto',
              text: 'Order State: ' + this.order.Status,
              margin: [0, 0, 10, 0],
            },
            {
              width: 'auto',
              text: 'Payment State: ' + this.order.PaymentStatus,
            },
          ],
        },
        {
          text: 'Total Price: S/.' + this.order.TotalAmount.toFixed(2),
          margin: [0, 0, 0, 10],
        },
        {
          text: 'User: ' + this.order.username,
          margin: [0, 0, 0, 10],
        },
        {
          text: 'Order Items',
          style: 'subheader',
          margin: [0, 10, 0, 5],
        },
        {
          table: {
            headerRows: 1,
            widths: ['auto', '*', 'auto', 'auto', 'auto'],
            body: [
              ['ID', 'Product', 'Price', 'Amount', 'Subtotal'],
              ...this.orderxitems.map((item: any) => [
                item.ItemId,
                item.menuitemName,
                'S/.' + item.menuitemPrice.toFixed(2),
                item.Quantity,
                'S/.' + item.Subtotal.toFixed(2),
              ]),
            ],
          },
        },
        {
          text: 'Total Price: S/.' + this.totalprice.toFixed(2),
          margin: [0, 10, 0, 0],
        },
      ],
      styles: {
        header: {
          fontSize: 18,
          bold: true,
          margin: [0, 0, 0, 10],
        },
        subheader: {
          fontSize: 14,
          bold: true,
          margin: [0, 10, 0, 5],
        },
      },
    };

    pdfMake.createPdf(pdfContent).getBlob((blob: Blob) => {
      const file = new File([blob], path, { type: 'application/pdf' });
      console.log('Archivo convertido:', file);

      const formData = new FormData();
      formData.append('file', file);

      this.http.post<any>('http://localhost:3000/apiorderingsystem/savedoc', formData).subscribe(
        (res) =>
          console.log(res, Swal.fire({
            icon: 'success',
            title: 'Ticket generated!',
            text: 'The ticket has been successfully generated and saved.'
          }).then((result) => {
            if (result.isConfirmed) {
              location.reload();
            }
          }))
      );
    });
  }

  // Método para editar un ordero
  edit(id: any) {
    this.creating = false;

    this.storeService.getItemsByOrder(id).subscribe(r => {
      this.orderxitems = r;
      console.log(this.orderxitems)
    })
    this.storeService.getTotalPriceByOrder(id).subscribe(r => {
      this.totalprice = r;
      console.log(this.totalprice)
    })
    //this.formOrder.get('Amount')?.disable();
    this.storeService.getOrder(id).subscribe((response: any) => {
      this.order = response;
      this.id = response.OrderId;
      this.formOrder.setValue({
        Status: response.Status,
        PaymentStatus: response.PaymentStatus,
        TotalAmount: response.TotalAmount,
        UserId: response.UserId
      });
    });
    this.formOrder = this.form.group({
      Status: [''],
      PaymentStatus: [''],
      TotalAmount: [''],
      UserId: ['']
    });
  }

  // Método para eliminar un ordero
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
        this.storeService.deleteOrder(id).subscribe(() => {
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

  // Método para guardar un order
  submit() {
    var order = new Order();
    order.UserId = this.formOrder.value.UserId;
    order.TotalAmount = this.formOrder.value.TotalAmount;
    order.Status = this.formOrder.value.Status;
    order.PaymentStatus = this.formOrder.value.PaymentStatus;
    if (this.selectedPaymentState === 'Paid') {
      order.Path = this.todayWithPipe + ".pdf";
    } else {
      order.Path = '';
    }
    var solicitud = this.creating ? this.storeService.insertOrder(order) : this.storeService.updateOrder(this.id, order);
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
        solicitud.subscribe(() => {
          this.generateTicket(order.Path);
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
    this.formOrder = this.form.group({
      Status: [''],
      PaymentStatus: [''],
      TotalAmount: [''],
      UserId: ['']
    });
    this.show = false;

  }
}
