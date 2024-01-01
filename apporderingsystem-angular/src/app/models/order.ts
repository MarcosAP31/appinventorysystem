export class Order{
    OrderId!:number;
    OrderDate!:Date;
    ReceptionDate!:Date;
    DispatchedDate!:Date;
    DeliveryDate!:Date;
    TotalPrice!:number;
    Seller!:string;
    DeliveryMan!:string;
    Status!:string;
}