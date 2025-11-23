export interface InventoryMovement {
    id: number;
    skuCode: string;
    quantity: number;
    inventoryMovementType: number;
    kioskId: number;
    productId: number;
    orderId: number;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updateBy: number;
}