export interface OrderDetail {
    id: number;
    line: number;
    skuCode: string;
    quantity: number;
    purchaseOrderId: number;
    productId: number;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updatedBy: number;
}