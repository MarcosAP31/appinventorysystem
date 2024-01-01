import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../models/order';
import { OrderXProduct } from '../models/orderxproduct';
import { User } from '../models/user';
import { Product } from '../models/product';
@Injectable({
  providedIn: 'root'
})
export class StoreService {
  headers = new Headers()
  API_URI = 'https://localhost:7000/api';
  /*
  apitipoinventario = 'https://localhost:7000/api/Tipoinventario';
  apilocal = 'https://localhost:7000/api/Local';
  apifamilia = 'https://localhost:7000/api/Familia';
  apiunidadmedida = 'https://localhost:7000/api/Unidadmedida';
  apicategoria = 'https://localhost:7000/api/Categoria';
  apialmacen = 'https://localhost:7000/api/Almacen';
  apiarticulo = 'https://localhost:7000/api/Articulo';
  apiusuario = 'https://localhost:7000/api/Usuario'
  apiarea='https://localhost:7000/api/Area'
  apiarticulotipoinventario='https://localhost:7000/api/ArticuloTipoInventario'
  apiusuarioarea='https://localhost:7000/api/UsuarioArea'
  apiinventariocabecera='https://localhost:7000/api/InventarioCabecera'
  apiinventariodetalle='https://localhost:7000/api/InventarioDetalle'
  //token='lrgWUpXa4Zu0cygU3NIL';*/
  //url='https://www.covermanager.com/api/restaurant/get_reservs_basic/'+this.token;
  constructor(public http: HttpClient) { }

  //Products
  getProducts(token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/Product?token=${token}`);
  }
  getProduct(productId: number,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/Product/${productId}?token=${token}`);
  }
  insertProduct(product: Product,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.post(`${this.API_URI}/Product/insert?token=${token}`, product,
    { responseType: 'text' });
  }
  updateProduct(product:Product, token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.put(`${this.API_URI}/Product/update?token=${token}`, product,
    { responseType: 'text' });
  }
  deleteProduct(productId: number,token:any): Observable<any> {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.delete(`${this.API_URI}/Product/${productId}?token=${token}`);
  }
  
  //Users
  getUsers(token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/User?token=${token}`);
  }
  getUser(userId: number,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/User/${userId}?token=${token}`);
  }
  getUserByEmail(email: string,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/User/email/${email}?token=${token}`);
  }
  getUsersByRole(role: string,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/User/role/${role}?token=${token}`);
  }
  insertUser(user: User,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.post(`${this.API_URI}/User/insert?token=${token}`, user,
    { responseType: 'text' });
  }
  updateUser(user:User, token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.put(`${this.API_URI}/User/update?token=${token}`, user,
    { responseType: 'text' });
  }
  deleteUser(userId: number,token:any): Observable<any> {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.delete(`${this.API_URI}/User/${userId}?token=${token}`);
  }
  login(user: any): Observable<any> {
    return this.http.post(`${this.API_URI}/User/login`, user,
    { responseType: 'text' });
  }
  validateLogin(token: any): Observable<any> {
    //let caracter=/\b[+]\b/g
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.post(`${this.API_URI}/User/validatelogin?token=${token}`, token,
    { responseType: 'text' });
  }

  //Orders
  getOrders(token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/Order?token=${token}`);
  }
  getOrder(orderId: number,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/Order/${orderId}?token=${token}`);
  }
  insertOrder(order: Order,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.post(`${this.API_URI}/Order/insert?token=${token}`, order,
    { responseType: 'text' });
  }
  updateOrder(order:Order, token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.put(`${this.API_URI}/Order/update?token=${token}`, order,
    { responseType: 'text' });
  }
  deleteOrder(orderId: number,token:any): Observable<any> {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.delete(`${this.API_URI}/Order/${orderId}?token=${token}`);
  }

  //OrderXProducts
  getOrderXProducts(token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/OrderXProduct?token=${token}`);
  }
  getOrderXProduct(orderxproductId: number,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/OrderXProduct/${orderxproductId}?token=${token}`);
  }
  insertOrderXProduct(orderxproduct: OrderXProduct,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.post(`${this.API_URI}/OrderXProduct/insert?token=${token}`, orderxproduct,
    { responseType: 'text' });
  }
  updateOrderXProduct(orderxproduct: OrderXProduct,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.put(`${this.API_URI}/OrderXProduct/update?token=${token}`, orderxproduct,
    { responseType: 'text' });
  }
  deleteOrderXProduct(orderxproductId: number,token:any): Observable<any> {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.delete(`${this.API_URI}/OrderXProduct/${orderxproductId}?token=${token}`);
  }
  getProductsByOrderId(orderId:number,token:any) {
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/OrderXProduct/orderid/${orderId}?token=${token}`);
  }
  getTotalPriceByOrderId(orderId:number,token:any){
    let caracter=new RegExp('[+]','g')
    token=token.replace(caracter,'%2B')
    let caracter1=new RegExp('[/]','g')
    token=token.replace(caracter1,'%2F')
    return this.http.get(`${this.API_URI}/OrderXProduct/totalprice/${orderId}?token=${token}`);
  }
  /*
    getMenuCategory(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiempresa}/${id}?token=${token}`);
    }
  
    insertarTipoInventario(form: any, token: any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apitipoinventario + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarTipoInventario(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apitipoinventario + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    eliminarTipoInventario(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apitipoinventario}/${id}?token=${token}`);
    }
    obtenerTiposInventario(token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apitipoinventario}?token=${token}`);
    }
  
    obtenerTipoInventario(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apitipoinventario}/${id}?token=${token}`);
    }
    insertarLocal(form: any, token: any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apilocal + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarLocal(form: any, token: any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apilocal + '/update?token='}${token}`, form,
        { responseType: 'text' });
  
    }
    eliminarLocal(id: any, token: any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apilocal}/${id}?token=${token}`);
    }
    obtenerLocalesPorEmpresa(token: any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apilocal}?token=${token}`);
    }
    obtenerLocalesPorEmpresaId(id:any,token:any):Observable<any>{
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apilocal}/empresa/${id}?token=${token}`);
    }
    /*obtenerLocalesPorEmpresa():Observable<any>{
      return this.http.get(this.apilocal+'/empresa');
    }
    obtenerlocalesPorEmpresa(id:any):Observable<any>{
      return this.http.get(`${this.apilocal}/empresa/${id}?id=${id}`);
    }
    obtenerLocal(id: any, token: any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apilocal}/${id}?token=${token}`);
    }
  
  
    insertarFamilia(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apifamilia + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarFamilia(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apifamilia + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    eliminarFamilia(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apifamilia}/${id}?token=${token}`);
    }
    obtenerFamiliasPorEmpresa(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apifamilia}?token=${token}`);
    }
    obtenerFamilia(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apifamilia}/${id}?token=${token}`);
    }
  
    insertarUnidadMedida(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiunidadmedida + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarUnidadMedida(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apiunidadmedida + '/update?token='}${token}`, form,
        { responseType: 'text' });
  
      //form.nombre_usuario=localStorage.getItem('usuario');
      //form.token=localStorage.getItem('token');
      
    }
    eliminarUnidadMedida(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apiunidadmedida}/${id}?token=${token}`);
    }
    obtenerUnidadesMedida(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiunidadmedida}?token=${token}`);
    }
    obtenerUnidadMedida(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiunidadmedida}/${id}?token=${token}`);
    }
  
    insertarCategoria(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apicategoria + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarCategoria(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apicategoria + '/update?token='}${token}`, form,
        { responseType: 'text' });
  
    }
    eliminarCategoria(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apicategoria}/${id}?token=${token}`);
    }
    obtenerCategoriasPorEmpresa(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apicategoria}?token=${token}`);
    }
    obtenerCategoria(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apicategoria}/${id}?token=${token}`);
    }
  
    insertarAlmacen(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apialmacen + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarAlmacen(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apialmacen + '/update?token='}${token}`, form,
        { responseType: 'text' });
      //form.nombre_usuario=localStorage.getItem('usuario');
      //form.token=localStorage.getItem('token');
  
    }
    eliminarAlmacen(id: any, token: any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apialmacen}/${id}?token=${token}`);
    }
    obtenerAlmacenesPorEmpresa(token: any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apialmacen}?token=${token}`);
    }
    obtenerAlmacenesPorLocal(id:any,token:any):Observable<any>{
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apialmacen}/local/${id}?token=${token}`);
    }
    /*obtenerAlmacenesPorLocal():Observable<any>{
      return this.http.get(this.apialmacen+'/local');
    }
    obtenerAlmacen(id: any, token: any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apialmacen}/${id}?token=${token}`);
    }
  
    insertarArticulo(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiarticulo + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarArticulo(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apiarticulo + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    eliminarArticulo(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apiarticulo}/${id}?token=${token}`);
    }
    obtenerArticulosPorEmpresa(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarticulo}?token=${token}`);
    }
    obtenerArticulo(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarticulo}/${id}?token=${token}`);
    }
  
    insertarUsuario(form: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiusuario + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarUsuario(form: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apiusuario + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    eliminarUsuario(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apiusuario}/${id}?token=${token}`);
    }
    obtenerUsuariosPorEmpresa(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiusuario}?token=${token}`);
    }
    obtenerUsuariosPorEmpresaId(id:any,token:any):Observable<any>{
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiusuario}/empresa/${id}?token=${token}`);
    }
    obtenerUsuario(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiusuario}/${id}?token=${token}`);
    }
    obtenerUsuarioPorNombreUsuario(usuario: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiusuario + '/nombreUsuario'}/${usuario}?token=${token}`);
    }
    login(usuario: any): Observable<any> {
      return this.http.post(this.apiusuario + '/login', usuario,
        { responseType: 'text' });
    }
    validarLogin(token: any): Observable<any> {
      //let caracter=/\b[+]\b/g
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiusuario + '/vallogin?token='}${token}`, token,
        { responseType: 'text' });
    }
  
    insertarArea(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiarea + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarArea(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apiarea + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    eliminarArea(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apiarea}/${id}?token=${token}`);
    }
    obtenerAreasPorEmpresa(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarea}?token=${token}`);
    }
    obtenerAreasPorAlmacen(id:any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarea}/almacen/${id}?token=${token}`);
    }
    obtenerAreasPorLocalAlmacen(id:any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarea}/local/${id}?token=${token}`);
    }
    obtenerArea(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarea}/${id}?token=${token}`);
    }
  
  
  
  
    insertarArticuloTipoInventario(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiarticulotipoinventario + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarArticuloTipoInventario(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apiarticulotipoinventario + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    eliminarArticuloTipoInventario(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apiarticulotipoinventario}/${id}?token=${token}`);
    }
    obtenerArticuloTiposInventarioPorEmpresa(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarticulotipoinventario}?token=${token}`);
    }
    obtenerArticulosTipoInventarioPorAreaId(id:any,token:any):Observable<any>{
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarticulotipoinventario}/area/${id}?token=${token}`);
    }
    obtenerArticuloTipoInventario(articuloid: any,areaid:any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiarticulotipoinventario}/${articuloid}%2C${areaid}?articuloId=${articuloid}&areaId=${areaid}&token=${token}`);
    }
  
  
    insertarUsuarioArea(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiusuarioarea + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarUsuarioArea(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apiusuarioarea + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    eliminarUsuarioArea(usuarioid: any,areaid:any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.delete(`${this.apiusuarioarea}/${usuarioid}%2C${areaid}?usuarioId=${usuarioid}&areaId=${areaid}&token=${token}`);
    }
    obtenerUsuariosAreaPorEmpresa(token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiusuarioarea}?token=${token}`);
    }
    obtenerUsuarioArea(usuarioid: any,areaid:any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiusuarioarea}/${usuarioid}%2C${areaid}?usuarioId=${usuarioid}&areaId=${areaid}&token=${token}`);
    }
  
    insertarInventarioCabecera(form: any, token: any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiinventariocabecera + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    actualizarInventarioCabecera(form: any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.put(`${this.apiinventariocabecera + '/update?token='}${token}`, form,
        { responseType: 'text' });
    }
    obtenerInventariosCabecera(token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiinventariocabecera}?token=${token}`);
    }
    obtenerInventariosCabeceraPorEstado(estado:string,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiinventariocabecera}/apertura/${estado}?token=${token}`);
    }
    obtenerInventarioCabecera(id: any,token:any): Observable<any> {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiinventariocabecera}/${id}?token=${token}`);
    }
    obtenerInventariosCabeceraPorFecha(fecha:any,token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      
      return this.http.get(`${this.apiinventariocabecera}/fecha/${fecha}?token=${token}`);
    }
    insertarInventarioDetalle(form: any, token: any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.post(`${this.apiinventariodetalle + '/insert?token='}${token}`, form,
        { responseType: 'text' });
    }
    
    obtenerInventariosDetalle(token:any) {
      let caracter=new RegExp('[+]','g')
      token=token.replace(caracter,'%2B')
      let caracter1=new RegExp('[/]','g')
      token=token.replace(caracter1,'%2F')
      return this.http.get(`${this.apiinventariodetalle}?token=${token}`);
    }
    /*
    getRestaurantes(){
      return this.http.get('https://www.covermanager.com/api/restaurant/list/lrgWUpXa4Zu0cygU3NIL');
    }*/

  /*
  getReservasRango(desde:any,hasta:any){
    return this.http.get('https://localhost:7023/api/reservas/rango?fecha1='+desde+'&fecha2='+hasta);
  }
  login(usuario:object){
    return this.http.post('https://localhost:7023/api/Usuario',usuario);
  }*/
}
