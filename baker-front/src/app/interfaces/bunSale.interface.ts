import { BunType } from "./bunType.interface";
import { QualityMonitoring } from "./qualityMonitoring.interface";

export interface BunSale {
    id: string,
    bunTypeId: string,
    price:number,
    bakedTime: Date,

    bunType: BunType,
    monitoring: QualityMonitoring
}