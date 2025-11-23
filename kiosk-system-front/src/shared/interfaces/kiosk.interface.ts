import { CurrentInventory } from "./current-inventory.interface";
import { InventoryMovement } from "./inventory-movement.interface";
import { Product } from "./product.interface";
import { PurchaseOrder } from "./purchase-order.interface";

export interface Kiosk {
    id: number;
    code: string;
    name: string;
    email: string;
    address: string;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updatedBy: number;
}

export interface KioskWithRelacional {
    id: number;
    code: string;
    name: string;
    email: string;
    address: string;
    dateAdded: Date;
    addedBy: number;
    lastUpdate: Date;
    updatedBy: number;

    products?: Product[];
    currentInventory: CurrentInventory[];
    purchaseOrder: PurchaseOrder[];
    inventoryMovement: InventoryMovement[]
}