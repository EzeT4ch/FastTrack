export interface CurrentInventory {
    id: number;
    quantity: number;
    kioskId: number;
    productId: number;
    dateAdded: Date;
    addedBy: number;
    lastUpdated: Date;
    updatedBy: number;
}