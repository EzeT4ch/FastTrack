import { OrderDetail } from "./order-detail.interface";

export interface PurchaseOrder {
    id: number;
    externalOrderKey: string;
    status: number;
    totalLines: number;
    totalQuantity: number;
    kioskId: number;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updatedBy: number;
}


export interface PurchaseOrderWithRelacional {
    id: number;
    externalOrderKey: string;
    status: number;
    totalLines: number;
    totalQuantity: number;
    kioskId: number;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updatedBy: number;

    orderDetails: OrderDetail[];
}