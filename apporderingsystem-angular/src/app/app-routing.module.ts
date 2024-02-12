import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './modules/user/user.component';
import { ProductComponent } from './modules/product/product.component';
import { InicioComponent } from './pages/inicio/inicio.component';
import { OrderComponent } from './modules/order/order.component';
import { LoginComponent } from './pages/login/login.component';
import { MantenimientoComponent } from './sections/mantenimiento/mantenimiento.component';
import { SecurityGuard } from './security.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent,
    
    
  },
  {
    path: 'inicio',
    component: InicioComponent,
    canActivate: [SecurityGuard],
    children: [
      {
        path: 'mantenimiento',
        component: MantenimientoComponent,
        children: [
          { path: 'user', component: UserComponent, canActivate: [SecurityGuard] },
          { path: 'product', component: ProductComponent, canActivate: [SecurityGuard] },
          { path: 'order', component: OrderComponent, canActivate: [SecurityGuard] },
        ]
      },
      {
        path: '',
        redirectTo: 'mantenimiento',
        pathMatch: 'full'
      }
    ]
  },
  // Agrega una ruta para manejar cualquier otra ruta no definida
  {
    path: '**',
    redirectTo: '/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
