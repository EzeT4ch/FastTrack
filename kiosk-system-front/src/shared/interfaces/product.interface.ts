import { CurrentInventory } from "./current-inventory.interface";
import { InventoryMovement } from "./inventory-movement.interface";
import { OrderDetail } from "./order-detail.interface";

export interface Product {
    id: number;
    kioskId: number;
    skuCode: string;
    name: string;
    status: number;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updateBy: number;
}

export interface ProductWithRelacional {
    id: number;
    kioskId: number;
    skuCode: string;
    name: string;
    status: number;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updateBy: number;

    currentInventories: CurrentInventory[];
    orderDetails: OrderDetail[];
    inventoryMovement: InventoryMovement[];
}