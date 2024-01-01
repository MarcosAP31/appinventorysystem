import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DataTablesModule } from "angular-datatables";
import { RouterModule } from '@angular/router';
import { NO_ERRORS_SCHEMA,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { UserComponent } from './modules/user/user.component';
import { ProductComponent } from './modules/product/product.component';
import { OrderComponent } from './modules/order/order.component';
import { InicioComponent } from './pages/inicio/inicio.component';
import { LoginComponent } from './pages/login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MantenimientoComponent } from './sections/mantenimiento/mantenimiento.component';
import { InventarioComponent } from './sections/inventario/inventario.component';
import { JwtInterceptor } from './shared/helpers/jwt.interceptor';
import { CookieService } from 'ngx-cookie-service';
@NgModule({
  declarations: [
    AppComponent,
    MantenimientoComponent,
    InventarioComponent,
    UserComponent,
    ProductComponent,
    InicioComponent,
    OrderComponent,
    LoginComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    DataTablesModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgbModule,
    RouterModule,
  ],
  exports:[
    DataTablesModule,
    
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass:JwtInterceptor,multi:true},
    CookieService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
