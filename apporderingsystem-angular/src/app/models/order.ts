export class Order{
    orderId!:number;
    orderDate!:Date | null;
    receptionDate!:Date | null;
    dispatchedDate!:Date | null;
    deliveryDate!:Date | null;
    totalPrice!:number;
    seller!:string;
    deliveryMan!:string;
    status!:string;
}